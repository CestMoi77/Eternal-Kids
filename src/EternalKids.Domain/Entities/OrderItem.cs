using EternalKids.Domain.Enums;

namespace EternalKids.Domain.Entities;

public class OrderItem
{
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public ProductType ProductType { get; set; }
    public Guid ProductId { get; set; }

    public string DescriptionSnapshot { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal LineTotal { get; set; }

    // Flexibele productopties, bv. halal/allergieën/beker-verdeling
    public string? OptionsJson { get; set; }

    public string? FulfillmentStatus { get; set; }

    public Order Order { get; set; } = null!;
}
