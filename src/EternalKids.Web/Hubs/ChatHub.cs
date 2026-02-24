using EternalKids.Application.Contracts;
using EternalKids.Application.DTOs;
using Microsoft.AspNetCore.SignalR;

namespace EternalKids.Web.Hubs;

public class ChatHub(IAiConversationService aiConversationService) : Hub
{
    public async Task JoinSession(string sessionId)
    {
        if (!Guid.TryParse(sessionId, out var parsedSessionId))
        {
            throw new HubException("Ongeldige sessionId.");
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, parsedSessionId.ToString());
    }

    public async Task SendMessage(ChatHubUserMessage request)
    {
        if (!Guid.TryParse(request.SessionId, out var sessionId))
        {
            throw new HubException("Ongeldige sessionId.");
        }

        var aiResult = await aiConversationService.SendAsync(new AiUserMessage(
            SessionId: sessionId,
            Message: request.Message,
            EventType: request.EventType,
            PackageType: request.PackageType,
            GuestCount: request.GuestCount,
            Quantity: request.Quantity));

        await Clients.Group(sessionId.ToString()).SendAsync("ReceiveAssistantMessage", new
        {
            sessionId = aiResult.SessionId,
            message = aiResult.AssistantMessage,
            uiActions = aiResult.UiActions,
            tokensIn = aiResult.TokensIn,
            tokensOut = aiResult.TokensOut
        });
    }
}

public sealed record ChatHubUserMessage(
    string SessionId,
    string Message,
    string? EventType,
    string? PackageType,
    int? GuestCount,
    int Quantity = 1);
