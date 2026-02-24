using EternalKids.Application.Contracts;
using EternalKids.Application.DTOs;

namespace EternalKids.Application.Services;

public class AiModelClientMock : IAiModelClient
{
    public Task<AiModelResponse> GenerateAsync(AiModelRequest request, CancellationToken ct = default)
    {
        var toolCalls = new List<AiToolCall>();

        var userText = request.UserMessage?.ToLowerInvariant() ?? string.Empty;
        if (request.ToolResults is null || request.ToolResults.Count == 0)
        {
            if (userText.Contains("prijs") || userText.Contains("kosten") || userText.Contains("pakket"))
            {
                // Simpele tool-call simulatie richting PricingService flow.
                var argsJson = "{\"eventType\":\"Kinderfeest\",\"packageType\":\"Basic\",\"guestCount\":30,\"quantity\":1}";
                toolCalls.Add(new AiToolCall("get_price", argsJson));
            }

            return Task.FromResult(new AiModelResponse(
                Succeeded: true,
                Model: "ek-ai-mock-v1",
                AssistantMessage: "Ik kijk direct naar de actuele prijs voor je.",
                ToolCalls: toolCalls,
                TokensIn: 42,
                TokensOut: 18));
        }

        var toolSummary = string.Join("; ", request.ToolResults.Select(x => x.ResultJson));
        return Task.FromResult(new AiModelResponse(
            Succeeded: true,
            Model: "ek-ai-mock-v1",
            AssistantMessage: $"Top! Ik heb de actuele prijs opgehaald: {toolSummary}",
            ToolCalls: Array.Empty<AiToolCall>(),
            TokensIn: 60,
            TokensOut: 40));
    }
}
