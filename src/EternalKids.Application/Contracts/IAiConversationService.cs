using EternalKids.Application.DTOs;

namespace EternalKids.Application.Contracts;

public interface IAiConversationService
{
    Task<AiConversationResult> SendAsync(AiUserMessage message, CancellationToken ct = default);
}
