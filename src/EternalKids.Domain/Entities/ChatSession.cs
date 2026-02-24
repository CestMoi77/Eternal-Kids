namespace EternalKids.Domain.Entities;

public class ChatSession
{
    public Guid Id { get; set; }
    public string Channel { get; set; } = string.Empty;
    public string AnonymousTokenHash { get; set; } = string.Empty;
    public Guid? CustomerId { get; set; }
    public DateTimeOffset StartedAtUtc { get; set; }
    public DateTimeOffset? EndedAtUtc { get; set; }
    public string? UtmSource { get; set; }
    public string? UtmCampaign { get; set; }
    public string? Referrer { get; set; }

    public ICollection<ChatMessage> Messages { get; set; } = new List<ChatMessage>();
}
