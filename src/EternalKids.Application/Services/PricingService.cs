using EternalKids.Application.Contracts;
using EternalKids.Application.DTOs;
using EternalKids.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EternalKids.Application.Services;

public class PricingService(EternalKidsContext dbContext) : IPricingService
{
    public async Task<PriceCalculationResult> CalculateAsync(PriceCalculationRequest request, CancellationToken ct = default)
    {
        if (request.GuestCount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request.GuestCount), "GuestCount moet groter zijn dan 0.");
        }

        if (request.Quantity <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(request.Quantity), "Quantity moet groter zijn dan 0.");
        }

        if (request.OverrideUnitPrice.HasValue)
        {
            var overrideLineTotal = request.OverrideUnitPrice.Value * request.Quantity;
            return new PriceCalculationResult(
                UnitPrice: request.OverrideUnitPrice.Value,
                LineTotal: overrideLineTotal,
                PriceSource: "override",
                EventType: request.EventType,
                GuestCount: request.GuestCount);
        }

        var priceRule = await dbContext.PriceRules
            .Where(x => x.IsActive)
            .Where(x => x.EventType == request.EventType)
            .Where(x => x.PackageType == request.PackageType)
            .Where(x => x.MinGuests <= request.GuestCount && x.MaxGuests >= request.GuestCount)
            .OrderBy(x => x.MinGuests)
            .FirstOrDefaultAsync(ct);

        if (priceRule is null)
        {
            throw new InvalidOperationException(
                $"Geen actieve prijsregel gevonden voor EventType '{request.EventType}', PackageType '{request.PackageType}' en GuestCount {request.GuestCount}.");
        }

        var lineTotal = priceRule.Price * request.Quantity;

        return new PriceCalculationResult(
            UnitPrice: priceRule.Price,
            LineTotal: lineTotal,
            PriceSource: "price-rule",
            EventType: request.EventType,
            GuestCount: request.GuestCount);
    }
}
