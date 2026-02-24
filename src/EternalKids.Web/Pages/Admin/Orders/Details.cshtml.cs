using EternalKids.Domain.Entities;
using EternalKids.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EternalKids.Web.Pages.Admin.Orders;

[Authorize]
public class DetailsModel(EternalKidsContext dbContext) : PageModel
{
    public Order? Order { get; private set; }

    public async Task OnGetAsync(Guid id, CancellationToken ct)
    {
        Order = await dbContext.Orders
            .Include(x => x.Items)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, ct);
    }
}
