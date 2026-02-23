namespace EternalKids.Domain.Entities;

public class PriceRule
{
    public Guid Id { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string PackageType { get; set; } = string.Empty;
    public int MinGuests { get; set; }
    public int MaxGuests { get; set; }
    public decimal Price { get; set; }
    public bool IsActive { get; set; } = true;
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; }
}
