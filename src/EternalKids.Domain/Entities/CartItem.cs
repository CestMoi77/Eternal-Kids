using EternalKids.Domain.Enums;

namespace EternalKids.Domain.Entities;

public class CartItem
{
    public Guid Id { get; set; }
    public Guid CartId { get; set; }
    public ProductType ProductType { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }

    // Flexibele productopties, bv. halal/allergieën/beker-verdeling
    public string? OptionsJson { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }

    public Cart Cart { get; set; } = null!;
}
