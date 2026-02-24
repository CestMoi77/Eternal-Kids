namespace EternalKids.Application.DTOs;

public sealed record AddCartItemRequest(
    Guid ProductId,
    string ProductType,
    string EventType,
    string PackageType,
    int GuestCount,
    int Quantity,
    Dictionary<string, object?> Options);

public sealed record CartTotalsResult(
    decimal Subtotal,
    decimal DiscountTotal,
    decimal GrandTotal,
    decimal DepositAmount,
    decimal DepositRate);
