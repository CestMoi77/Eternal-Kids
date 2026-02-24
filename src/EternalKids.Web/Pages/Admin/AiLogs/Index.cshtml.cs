using EternalKids.Domain.Entities;
using EternalKids.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EternalKids.Web.Pages.Admin.AiLogs;

[Authorize]
public class IndexModel(EternalKidsContext dbContext) : PageModel
{
    public List<AiRequestLog> Logs { get; private set; } = [];

    public async Task OnGetAsync(CancellationToken ct)
    {
        Logs = await dbContext.AiRequestLogs
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAtUtc)
            .Take(300)
            .ToListAsync(ct);
    }
}
