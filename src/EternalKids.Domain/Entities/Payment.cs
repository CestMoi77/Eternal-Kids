using EternalKids.Domain.Enums;

namespace EternalKids.Domain.Entities;

public class Payment
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public string Provider { get; set; } = string.Empty;
    public string ProviderPaymentId { get; set; } = string.Empty;
    public string ProviderCheckoutUrl { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "EUR";
    public PaymentStatus Status { get; set; }
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; }

    public Order Order { get; set; } = null!;
    public ICollection<PaymentEvent> Events { get; set; } = new List<PaymentEvent>();
}
