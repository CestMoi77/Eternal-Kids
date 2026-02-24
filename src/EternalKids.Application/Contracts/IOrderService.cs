using EternalKids.Application.DTOs;

namespace EternalKids.Application.Contracts;

public interface IOrderService
{
    Task<CreateOrderFromCartResult> CreateFromCartAsync(CreateOrderFromCartRequest request, CancellationToken ct = default);
}
