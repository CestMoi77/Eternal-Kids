using System.ComponentModel.DataAnnotations;
using EternalKids.Domain.Entities;
using EternalKids.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EternalKids.Web.Pages.Admin.Content;

[Authorize]
public class IndexModel(EternalKidsContext dbContext) : PageModel
{
    public List<SiteContentBlock> Items { get; private set; } = [];

    [BindProperty]
    public ContentInput Input { get; set; } = new();

    public async Task OnGetAsync(CancellationToken ct)
    {
        Items = await dbContext.SiteContentBlocks.AsNoTracking().OrderBy(x => x.PageKey).ThenBy(x => x.SectionKey).ToListAsync(ct);
    }

    public async Task<IActionResult> OnPostSaveAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            await OnGetAsync(ct);
            return Page();
        }

        var existing = await dbContext.SiteContentBlocks.FirstOrDefaultAsync(x => x.PageKey == Input.PageKey && x.SectionKey == Input.SectionKey, ct);
        if (existing is null)
        {
            dbContext.SiteContentBlocks.Add(new SiteContentBlock
            {
                Id = Guid.NewGuid(),
                PageKey = Input.PageKey,
                SectionKey = Input.SectionKey,
                Title = Input.Title,
                Body = Input.Body,
                UpdatedAtUtc = DateTimeOffset.UtcNow
            });
        }
        else
        {
            existing.Title = Input.Title;
            existing.Body = Input.Body;
            existing.UpdatedAtUtc = DateTimeOffset.UtcNow;
        }

        await dbContext.SaveChangesAsync(ct);
        return RedirectToPage();
    }

    public sealed class ContentInput
    {
        [Required] public string PageKey { get; set; } = "Home";
        [Required] public string SectionKey { get; set; } = "Hero";
        [Required] public string Title { get; set; } = string.Empty;
        [Required] public string Body { get; set; } = string.Empty;
    }
}
