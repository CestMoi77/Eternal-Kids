using EternalKids.Application.Contracts;
using EternalKids.Application.DTOs;
using EternalKids.Domain.Entities;
using EternalKids.Domain.Enums;
using EternalKids.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace EternalKids.Application.Services;

public class PaymentServiceMock(EternalKidsContext dbContext) : IPaymentService
{
    public async Task<StartPaymentResult> StartPaymentAsync(StartPaymentRequest request, CancellationToken ct = default)
    {
        var order = await dbContext.Orders.FirstOrDefaultAsync(x => x.Id == request.OrderId, ct)
            ?? throw new InvalidOperationException($"Order '{request.OrderId}' niet gevonden.");

        order.Status = OrderStatus.PaymentPending;

        var amount = request.PaymentMode == PaymentMode.Deposit50
            ? order.DepositAmount
            : order.GrandTotal;

        var payment = new Payment
        {
            Id = Guid.NewGuid(),
            OrderId = order.Id,
            Provider = "MockMollie",
            ProviderPaymentId = $"mock_{Guid.NewGuid():N}",
            ProviderCheckoutUrl = $"/checkout/mock-success?orderId={order.Id}",
            Amount = amount,
            Currency = "EUR",
            Status = PaymentStatus.Pending,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            UpdatedAtUtc = DateTimeOffset.UtcNow
        };

        dbContext.Payments.Add(payment);
        await dbContext.SaveChangesAsync(ct);

        // Simuleer succesvolle payment direct
        payment.Status = PaymentStatus.Paid;
        payment.UpdatedAtUtc = DateTimeOffset.UtcNow;
        order.Status = request.PaymentMode == PaymentMode.Deposit50
            ? OrderStatus.DepositPaid
            : OrderStatus.Paid;
        order.UpdatedAtUtc = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(ct);

        return new StartPaymentResult(
            PaymentId: payment.Id,
            Provider: payment.Provider,
            CheckoutUrl: payment.ProviderCheckoutUrl,
            Amount: payment.Amount,
            Status: payment.Status,
            OrderStatus: order.Status);
    }
}
