using EternalKids.Domain.Enums;

namespace EternalKids.Domain.Entities;

public class ChatMessage
{
    public Guid Id { get; set; }
    public Guid ChatSessionId { get; set; }
    public ChatRole Role { get; set; }
    public string Content { get; set; } = string.Empty;
    public DateTimeOffset CreatedAtUtc { get; set; }
    public int? LatencyMs { get; set; }
    public int? TokenUsagePrompt { get; set; }
    public int? TokenUsageCompletion { get; set; }
    public Guid? LinkedOrderId { get; set; }

    public ChatSession ChatSession { get; set; } = null!;
}
