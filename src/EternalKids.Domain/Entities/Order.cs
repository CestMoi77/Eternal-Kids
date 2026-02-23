using EternalKids.Domain.Enums;

namespace EternalKids.Domain.Entities;

public class Order
{
    public Guid Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }

    // Verplicht voor kanaalanalyse: AI Planner / Web / Admin
    public OrderChannel Channel { get; set; }

    public Guid? CustomerId { get; set; }
    public DateOnly? EventDate { get; set; }
    public string? EventType { get; set; }
    public int? GuestCount { get; set; }

    public decimal Subtotal { get; set; }
    public decimal DiscountTotal { get; set; }
    public decimal GrandTotal { get; set; }

    public bool DepositRequired { get; set; }
    public decimal DepositRate { get; set; }

    // 50% aanbetaling (of andere policy) bedrag
    public decimal DepositAmount { get; set; }

    public string? CouponCodeApplied { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; }
    public byte[] RowVersion { get; set; } = Array.Empty<byte>();

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
