using EternalKids.Domain.Enums;

namespace EternalKids.Application.DTOs;

public sealed record CreateOrderFromCartRequest(
    Guid CartId,
    string CustomerName,
    string CustomerEmail,
    DateOnly EventDate,
    string EventType,
    int GuestCount,
    PaymentMode PaymentMode,
    string Channel = "Web",
    decimal DiscountTotal = 0m,
    string? CouponCode = null,
    string? Notes = null);

public sealed record CreateOrderFromCartResult(
    Guid OrderId,
    string OrderNumber,
    decimal GrandTotal,
    decimal DepositAmount,
    OrderStatus Status);

public sealed record StartPaymentRequest(
    Guid OrderId,
    PaymentMode PaymentMode);

public sealed record StartPaymentResult(
    Guid PaymentId,
    string Provider,
    string CheckoutUrl,
    decimal Amount,
    PaymentStatus Status,
    OrderStatus OrderStatus);
