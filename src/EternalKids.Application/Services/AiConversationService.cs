using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Linq;
using EternalKids.Application.Contracts;
using EternalKids.Application.DTOs;
using EternalKids.Domain.Entities;
using EternalKids.Domain.Enums;
using EternalKids.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EternalKids.Application.Services;

public class AiConversationService(
    EternalKidsContext dbContext,
    IAiModelClient aiModelClient,
    IPricingService pricingService) : IAiConversationService
{
    private const string PriceToolName = "get_price";

    public async Task<AiConversationResult> SendAsync(AiUserMessage message, CancellationToken ct = default)
    {
        var session = await GetOrCreateSessionAsync(message.SessionId, ct);

        var userChatMessage = new ChatMessage
        {
            Id = Guid.NewGuid(),
            ChatSessionId = session.Id,
            Role = ChatRole.User,
            Content = message.Message,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        dbContext.ChatMessages.Add(userChatMessage);
        await dbContext.SaveChangesAsync(ct);

        var systemPrompt = BuildSystemPrompt();
        var tools = BuildTools();

        var firstResponse = await aiModelClient.GenerateAsync(
            new AiModelRequest(systemPrompt, message.Message, tools),
            ct);

        var toolResults = new List<AiToolExecutionResult>();

        foreach (var toolCall in firstResponse.ToolCalls.Where(t => t.Name.Equals(PriceToolName, StringComparison.OrdinalIgnoreCase)))
        {
            var toolResult = await ExecutePriceToolAsync(message, toolCall, ct);
            toolResults.Add(toolResult);

            dbContext.ChatMessages.Add(new ChatMessage
            {
                Id = Guid.NewGuid(),
                ChatSessionId = session.Id,
                Role = ChatRole.Tool,
                Content = toolResult.ResultJson,
                CreatedAtUtc = DateTimeOffset.UtcNow
            });
        }

        var finalResponse = toolResults.Count == 0
            ? firstResponse
            : await aiModelClient.GenerateAsync(
                new AiModelRequest(systemPrompt, message.Message, tools, toolResults),
                ct);

        dbContext.ChatMessages.Add(new ChatMessage
        {
            Id = Guid.NewGuid(),
            ChatSessionId = session.Id,
            Role = ChatRole.Assistant,
            Content = finalResponse.AssistantMessage,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            TokenUsagePrompt = finalResponse.TokensIn,
            TokenUsageCompletion = finalResponse.TokensOut
        });

        dbContext.AiRequestLogs.Add(CreateRequestLog(session.Id, message.Message, firstResponse));

        if (toolResults.Count > 0)
        {
            dbContext.AiRequestLogs.Add(CreateRequestLog(session.Id, message.Message, finalResponse));
        }

        await dbContext.SaveChangesAsync(ct);

        var uiActions = BuildUiActions(toolResults);

        return new AiConversationResult(
            SessionId: session.Id,
            AssistantMessage: finalResponse.AssistantMessage,
            UiActions: uiActions,
            TokensIn: finalResponse.TokensIn,
            TokensOut: finalResponse.TokensOut);
    }

    private async Task<ChatSession> GetOrCreateSessionAsync(Guid sessionId, CancellationToken ct)
    {
        var session = await dbContext.ChatSessions.FirstOrDefaultAsync(x => x.Id == sessionId, ct);

        if (session is not null)
        {
            return session;
        }

        session = new ChatSession
        {
            Id = sessionId == Guid.Empty ? Guid.NewGuid() : sessionId,
            Channel = "Planner",
            AnonymousTokenHash = "chat-session",
            StartedAtUtc = DateTimeOffset.UtcNow
        };

        dbContext.ChatSessions.Add(session);
        await dbContext.SaveChangesAsync(ct);

        return session;
    }

    private async Task<AiToolExecutionResult> ExecutePriceToolAsync(AiUserMessage message, AiToolCall toolCall, CancellationToken ct)
    {
        using var document = JsonDocument.Parse(toolCall.ArgumentsJson);
        var root = document.RootElement;

        var eventType = root.TryGetProperty("eventType", out var eventTypeEl) && eventTypeEl.ValueKind == JsonValueKind.String
            ? eventTypeEl.GetString()
            : message.EventType;

        var packageType = root.TryGetProperty("packageType", out var packageTypeEl) && packageTypeEl.ValueKind == JsonValueKind.String
            ? packageTypeEl.GetString()
            : message.PackageType;

        var guestCount = root.TryGetProperty("guestCount", out var guestCountEl) && guestCountEl.TryGetInt32(out var parsedGuests)
            ? parsedGuests
            : message.GuestCount;

        var quantity = root.TryGetProperty("quantity", out var quantityEl) && quantityEl.TryGetInt32(out var parsedQuantity)
            ? parsedQuantity
            : message.Quantity;

        if (string.IsNullOrWhiteSpace(eventType) || string.IsNullOrWhiteSpace(packageType) || guestCount is null)
        {
            var missingResultJson = JsonSerializer.Serialize(new
            {
                success = false,
                error = "Ontbrekende parameters voor prijsberekening. Vereist: eventType, packageType, guestCount."
            });

            return new AiToolExecutionResult(PriceToolName, missingResultJson);
        }

        var priceResult = await pricingService.CalculateAsync(
            new PriceCalculationRequest(eventType, packageType, guestCount.Value, quantity),
            ct);

        var resultJson = JsonSerializer.Serialize(new
        {
            success = true,
            eventType,
            packageType,
            guestCount,
            quantity,
            unitPrice = priceResult.UnitPrice,
            lineTotal = priceResult.LineTotal,
            source = priceResult.PriceSource
        });

        return new AiToolExecutionResult(PriceToolName, resultJson);
    }

    private static IReadOnlyCollection<AiToolDefinition> BuildTools() =>
    [
        new AiToolDefinition(
            Name: PriceToolName,
            Description: "Haal actuele prijs op uit Prijzenbeheer op basis van eventType, packageType en guestCount.",
            JsonSchema: "{\"type\":\"object\",\"properties\":{\"eventType\":{\"type\":\"string\"},\"packageType\":{\"type\":\"string\"},\"guestCount\":{\"type\":\"integer\"},\"quantity\":{\"type\":\"integer\"}},\"required\":[\"eventType\",\"packageType\",\"guestCount\"]}")
    ];

    private static string BuildSystemPrompt() =>
        "Je bent de Eternal Kids AI Planner. Gebruik altijd tool-calling voor echte prijzen. " +
        "Verzin nooit prijzen. Adviseer in het Nederlands en geef duidelijke vervolgstappen.";

    private static IReadOnlyCollection<AiUiAction> BuildUiActions(IEnumerable<AiToolExecutionResult> toolResults)
    {
        var hasPrice = toolResults.Any(x => x.Name.Equals(PriceToolName, StringComparison.OrdinalIgnoreCase));

        if (!hasPrice)
        {
            return
            [
                new AiUiAction("ask_more", "Vul eventtype, pakket en aantal gasten in")
            ];
        }

        return
        [
            new AiUiAction("add_to_cart", "Toevoegen aan winkelwagen"),
            new AiUiAction("checkout", "Ga naar afrekenen")
        ];
    }

    private static AiRequestLog CreateRequestLog(Guid sessionId, string prompt, AiModelResponse response)
    {
        var toolCallsJson = response.ToolCalls.Count == 0
            ? null
            : JsonSerializer.Serialize(response.ToolCalls);

        return new AiRequestLog
        {
            Id = Guid.NewGuid(),
            ChatSessionId = sessionId,
            Model = response.Model,
            PromptHash = ComputeSha256(prompt),
            ToolCallsJson = toolCallsJson,
            ResponseHash = ComputeSha256(response.AssistantMessage),
            TokensIn = response.TokensIn,
            TokensOut = response.TokensOut,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            Succeeded = response.Succeeded,
            ErrorCode = response.ErrorCode,
            ErrorMessage = response.ErrorMessage
        };
    }

    private static string ComputeSha256(string value)
    {
        var bytes = SHA256.HashData(Encoding.UTF8.GetBytes(value));
        return Convert.ToHexString(bytes).ToLowerInvariant();
    }
}
