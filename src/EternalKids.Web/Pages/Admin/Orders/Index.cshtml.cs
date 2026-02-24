using EternalKids.Domain.Entities;
using EternalKids.Domain.Enums;
using EternalKids.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace EternalKids.Web.Pages.Admin.Orders;

[Authorize]
public class IndexModel(EternalKidsContext dbContext) : PageModel
{
    public string StatusFilter { get; set; } = "Paid";
    public List<Order> Orders { get; private set; } = [];

    public IReadOnlyList<SelectListItem> StatusOptions { get; } =
    [
        new("Alle", "All"),
        new("Paid", nameof(OrderStatus.Paid)),
        new("DepositPaid", nameof(OrderStatus.DepositPaid))
    ];

    public async Task OnGetAsync(string? status, CancellationToken ct)
    {
        StatusFilter = string.IsNullOrWhiteSpace(status) ? nameof(OrderStatus.Paid) : status;

        var query = dbContext.Orders
            .Include(x => x.Items)
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAtUtc)
            .AsQueryable();

        if (!StatusFilter.Equals("All", StringComparison.OrdinalIgnoreCase) && Enum.TryParse<OrderStatus>(StatusFilter, out var parsed))
        {
            query = query.Where(x => x.Status == parsed);
        }

        Orders = await query.Take(200).ToListAsync(ct);
    }
}
