using EternalKids.Application.Contracts;
using EternalKids.Application.DTOs;
using EternalKids.Domain.Entities;
using EternalKids.Domain.Enums;
using EternalKids.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EternalKids.Application.Services;

public class OrderService(EternalKidsContext dbContext) : IOrderService
{
    public async Task<CreateOrderFromCartResult> CreateFromCartAsync(CreateOrderFromCartRequest request, CancellationToken ct = default)
    {
        var cart = await dbContext.Carts
            .Include(x => x.Items)
            .FirstOrDefaultAsync(x => x.Id == request.CartId, ct)
            ?? throw new InvalidOperationException($"Cart '{request.CartId}' niet gevonden.");

        if (cart.Items.Count == 0)
        {
            throw new InvalidOperationException("De winkelwagen is leeg.");
        }

        var subtotal = cart.Items.Sum(x => x.LineTotal);
        var grandTotal = Math.Max(0m, subtotal - request.DiscountTotal);
        var depositAmount = Math.Round(grandTotal * 0.5m, 2, MidpointRounding.AwayFromZero);

        var order = new Order
        {
            Id = Guid.NewGuid(),
            OrderNumber = $"EK-{DateTime.UtcNow:yyyyMMdd}-{Random.Shared.Next(1000, 9999)}",
            Status = OrderStatus.Draft,
            Channel = ParseChannel(request.Channel),
            EventDate = request.EventDate,
            EventType = request.EventType,
            GuestCount = request.GuestCount,
            Subtotal = subtotal,
            DiscountTotal = request.DiscountTotal,
            GrandTotal = grandTotal,
            DepositRequired = request.PaymentMode == PaymentMode.Deposit50,
            DepositRate = request.PaymentMode == PaymentMode.Deposit50 ? 0.5m : 1m,
            DepositAmount = request.PaymentMode == PaymentMode.Deposit50 ? depositAmount : grandTotal,
            CouponCodeApplied = request.CouponCode,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            UpdatedAtUtc = DateTimeOffset.UtcNow
        };

        foreach (var cartItem in cart.Items)
        {
            order.Items.Add(new OrderItem
            {
                Id = Guid.NewGuid(),
                OrderId = order.Id,
                ProductType = cartItem.ProductType,
                ProductId = cartItem.ProductId,
                DescriptionSnapshot = $"{cartItem.ProductType} {cartItem.ProductId}",
                Quantity = cartItem.Quantity,
                UnitPrice = cartItem.UnitPrice,
                LineTotal = cartItem.LineTotal,
                OptionsJson = cartItem.OptionsJson,
                FulfillmentStatus = "Pending"
            });
        }

        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync(ct);

        return new CreateOrderFromCartResult(
            OrderId: order.Id,
            OrderNumber: order.OrderNumber,
            GrandTotal: order.GrandTotal,
            DepositAmount: order.DepositAmount,
            Status: order.Status);
    }

    private static OrderChannel ParseChannel(string channel) => channel.ToLowerInvariant() switch
    {
        "web" => OrderChannel.Web,
        "aiplanner" or "ai" => OrderChannel.AiPlanner,
        "admin" => OrderChannel.Admin,
        _ => OrderChannel.Web
    };
}
