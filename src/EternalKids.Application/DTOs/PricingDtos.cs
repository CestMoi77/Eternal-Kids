namespace EternalKids.Application.DTOs;

public sealed record PriceCalculationRequest(
    string EventType,
    string PackageType,
    int GuestCount,
    int Quantity = 1,
    decimal? OverrideUnitPrice = null);

public sealed record PriceCalculationResult(
    decimal UnitPrice,
    decimal LineTotal,
    string PriceSource,
    string EventType,
    int GuestCount);
