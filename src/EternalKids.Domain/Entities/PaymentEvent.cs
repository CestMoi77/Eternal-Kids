namespace EternalKids.Domain.Entities;

public class PaymentEvent
{
    public Guid Id { get; set; }
    public Guid PaymentId { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string RawPayload { get; set; } = string.Empty;
    public DateTimeOffset ReceivedAtUtc { get; set; }

    public Payment Payment { get; set; } = null!;
}
