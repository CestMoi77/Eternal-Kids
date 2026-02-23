namespace EternalKids.Domain.Entities;

public class Cart
{
    public Guid Id { get; set; }
    public string AnonymousTokenHash { get; set; } = string.Empty;
    public Guid? CustomerId { get; set; }
    public string Currency { get; set; } = "EUR";
    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset UpdatedAtUtc { get; set; }
    public DateTimeOffset ExpiresAtUtc { get; set; }

    public ICollection<CartItem> Items { get; set; } = new List<CartItem>();
}
