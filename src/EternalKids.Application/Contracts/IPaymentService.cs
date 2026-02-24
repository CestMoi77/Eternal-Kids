using EternalKids.Application.DTOs;

namespace EternalKids.Application.Contracts;

public interface IPaymentService
{
    Task<StartPaymentResult> StartPaymentAsync(StartPaymentRequest request, CancellationToken ct = default);
}
