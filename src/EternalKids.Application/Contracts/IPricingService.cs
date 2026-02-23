using EternalKids.Application.DTOs;

namespace EternalKids.Application.Contracts;

public interface IPricingService
{
    Task<PriceCalculationResult> CalculateAsync(PriceCalculationRequest request, CancellationToken ct = default);
}
