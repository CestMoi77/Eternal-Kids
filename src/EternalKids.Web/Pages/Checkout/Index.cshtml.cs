using System.ComponentModel.DataAnnotations;
using EternalKids.Application.Contracts;
using EternalKids.Application.DTOs;
using EternalKids.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EternalKids.Web.Pages.Checkout;

public class IndexModel(
    IShoppingCartService shoppingCartService,
    IOrderService orderService,
    IPaymentService paymentService) : PageModel
{
    private const string CartCookieName = "ek-cart-token";

    [BindProperty]
    public CheckoutInputModel Input { get; set; } = new();

    public decimal Subtotal { get; private set; }
    public decimal DiscountTotal { get; private set; }
    public decimal GrandTotal { get; private set; }
    public decimal DepositAmount { get; private set; }
    public string? ResultMessage { get; private set; }

    public async Task OnGetAsync(string? eventType, int? guestCount, string? packageType, CancellationToken ct)
    {
        if (!string.IsNullOrWhiteSpace(eventType))
        {
            Input.EventType = eventType;
        }

        if (guestCount is > 0)
        {
            Input.GuestCount = guestCount.Value;
        }

        await LoadCartSummaryAsync(ct);
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            await LoadCartSummaryAsync(ct);
            return Page();
        }

        var cart = await GetOrCreateCartAsync(ct);
        var order = await orderService.CreateFromCartAsync(new CreateOrderFromCartRequest(
            CartId: cart.Id,
            CustomerName: Input.CustomerName,
            CustomerEmail: Input.CustomerEmail,
            EventDate: Input.EventDate,
            EventType: Input.EventType,
            GuestCount: Input.GuestCount,
            PaymentMode: Input.PaymentMode,
            Channel: "Web"), ct);

        var payment = await paymentService.StartPaymentAsync(new StartPaymentRequest(
            OrderId: order.OrderId,
            PaymentMode: Input.PaymentMode), ct);

        ResultMessage = $"Bestelling {order.OrderNumber} aangemaakt. Betaalstatus: {payment.Status}. Orderstatus: {payment.OrderStatus}.";

        await LoadCartSummaryAsync(ct);
        return Page();
    }

    private async Task LoadCartSummaryAsync(CancellationToken ct)
    {
        var cart = await GetOrCreateCartAsync(ct);
        var totals = await shoppingCartService.CalculateTotalsAsync(cart.Id, 0m, ct);

        Subtotal = totals.Subtotal;
        DiscountTotal = totals.DiscountTotal;
        GrandTotal = totals.GrandTotal;
        DepositAmount = totals.DepositAmount;

        ViewData["CartItemCount"] = cart.Items.Count;
        ViewData["CartSubtotal"] = totals.Subtotal;
    }

    private async Task<EternalKids.Domain.Entities.Cart> GetOrCreateCartAsync(CancellationToken ct)
    {
        var cookieToken = Request.Cookies[CartCookieName];
        if (string.IsNullOrWhiteSpace(cookieToken))
        {
            cookieToken = Guid.NewGuid().ToString("N");
            Response.Cookies.Append(CartCookieName, cookieToken, new CookieOptions
            {
                HttpOnly = true,
                IsEssential = true,
                Secure = false,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddDays(7)
            });
        }

        return await shoppingCartService.GetOrCreateCartAsync(cookieToken, ct);
    }

    public sealed class CheckoutInputModel
    {
        [Required]
        public string CustomerName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string CustomerEmail { get; set; } = string.Empty;

        [Required]
        public DateOnly EventDate { get; set; } = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(14));

        [Required]
        public string EventType { get; set; } = "Kinderfeest";

        [Range(1, 10000)]
        public int GuestCount { get; set; } = 30;

        [Required]
        public PaymentMode PaymentMode { get; set; } = PaymentMode.Deposit50;
    }
}
