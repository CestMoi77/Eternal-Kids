using EternalKids.Application.DTOs;
using EternalKids.Domain.Entities;

namespace EternalKids.Application.Contracts;

public interface IShoppingCartService
{
    Task<Cart> GetOrCreateCartAsync(string anonymousTokenHash, CancellationToken ct = default);
    Task<CartItem> AddItemAsync(Guid cartId, AddCartItemRequest request, CancellationToken ct = default);
    Task<CartTotalsResult> CalculateTotalsAsync(Guid cartId, decimal discountTotal = 0m, CancellationToken ct = default);
}
