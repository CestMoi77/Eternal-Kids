using System.Text.Json;
using EternalKids.Application.Contracts;
using EternalKids.Application.DTOs;
using EternalKids.Domain.Entities;
using EternalKids.Domain.Enums;
using EternalKids.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EternalKids.Application.Services;

public class ShoppingCartService(EternalKidsContext dbContext, IPricingService pricingService) : IShoppingCartService
{
    private const decimal DefaultDepositRate = 0.50m;

    public async Task<Cart> GetOrCreateCartAsync(string anonymousTokenHash, CancellationToken ct = default)
    {
        var cart = await dbContext.Carts
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.AnonymousTokenHash == anonymousTokenHash, ct);

        if (cart is not null)
        {
            cart.UpdatedAtUtc = DateTimeOffset.UtcNow;
            await dbContext.SaveChangesAsync(ct);
            return cart;
        }

        cart = new Cart
        {
            Id = Guid.NewGuid(),
            AnonymousTokenHash = anonymousTokenHash,
            Currency = "EUR",
            CreatedAtUtc = DateTimeOffset.UtcNow,
            UpdatedAtUtc = DateTimeOffset.UtcNow,
            ExpiresAtUtc = DateTimeOffset.UtcNow.AddDays(7)
        };

        dbContext.Carts.Add(cart);
        await dbContext.SaveChangesAsync(ct);

        return cart;
    }

    public async Task<CartItem> AddItemAsync(Guid cartId, AddCartItemRequest request, CancellationToken ct = default)
    {
        var cart = await dbContext.Carts.FirstOrDefaultAsync(x => x.Id == cartId, ct)
            ?? throw new InvalidOperationException($"Cart '{cartId}' niet gevonden.");

        var price = await pricingService.CalculateAsync(new PriceCalculationRequest(
            EventType: request.EventType,
            PackageType: request.PackageType,
            GuestCount: request.GuestCount,
            Quantity: request.Quantity), ct);

        if (!Enum.TryParse<ProductType>(request.ProductType, ignoreCase: true, out var productType))
        {
            throw new InvalidOperationException($"Onbekend ProductType '{request.ProductType}'.");
        }

        var optionsJson = JsonSerializer.Serialize(request.Options);

        var item = new CartItem
        {
            Id = Guid.NewGuid(),
            CartId = cartId,
            ProductId = request.ProductId,
            ProductType = productType,
            Quantity = request.Quantity,
            UnitPrice = price.UnitPrice,
            LineTotal = price.LineTotal,
            OptionsJson = optionsJson,
            CreatedAtUtc = DateTimeOffset.UtcNow
        };

        dbContext.CartItems.Add(item);
        cart.UpdatedAtUtc = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(ct);

        return item;
    }

    public async Task<CartTotalsResult> CalculateTotalsAsync(Guid cartId, decimal discountTotal = 0m, CancellationToken ct = default)
    {
        var subtotal = await dbContext.CartItems
            .Where(x => x.CartId == cartId)
            .SumAsync(x => x.LineTotal, ct);

        if (discountTotal < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(discountTotal), "Korting mag niet negatief zijn.");
        }

        var grandTotal = Math.Max(0m, subtotal - discountTotal);
        var depositAmount = Math.Round(grandTotal * DefaultDepositRate, 2, MidpointRounding.AwayFromZero);

        return new CartTotalsResult(
            Subtotal: subtotal,
            DiscountTotal: discountTotal,
            GrandTotal: grandTotal,
            DepositAmount: depositAmount,
            DepositRate: DefaultDepositRate);
    }
}
