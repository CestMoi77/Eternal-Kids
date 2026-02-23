using System.ComponentModel.DataAnnotations;
using EternalKids.Domain.Entities;
using EternalKids.Infrastructure.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EternalKids.Web.Pages.Admin.Pricing;

[Authorize]
public class IndexModel(EternalKidsContext dbContext) : PageModel
{
    public List<PriceRule> Rules { get; private set; } = [];

    [BindProperty]
    public CreatePriceRuleInput CreateInput { get; set; } = new();

    public async Task OnGetAsync(CancellationToken ct)
    {
        Rules = await dbContext.PriceRules.AsNoTracking().OrderBy(x => x.EventType).ThenBy(x => x.PackageType).ToListAsync(ct);
    }

    public async Task<IActionResult> OnPostCreateAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            await OnGetAsync(ct);
            return Page();
        }

        dbContext.PriceRules.Add(new PriceRule
        {
            Id = Guid.NewGuid(),
            EventType = CreateInput.EventType,
            PackageType = CreateInput.PackageType,
            MinGuests = CreateInput.MinGuests,
            MaxGuests = CreateInput.MaxGuests,
            Price = CreateInput.Price,
            IsActive = true,
            CreatedAtUtc = DateTimeOffset.UtcNow,
            UpdatedAtUtc = DateTimeOffset.UtcNow
        });

        await dbContext.SaveChangesAsync(ct);
        return RedirectToPage();
    }

    public async Task<IActionResult> OnPostDeleteAsync(Guid id, CancellationToken ct)
    {
        var rule = await dbContext.PriceRules.FirstOrDefaultAsync(x => x.Id == id, ct);
        if (rule is not null)
        {
            dbContext.PriceRules.Remove(rule);
            await dbContext.SaveChangesAsync(ct);
        }

        return RedirectToPage();
    }

    public sealed class CreatePriceRuleInput
    {
        [Required] public string EventType { get; set; } = "Kinderfeest";
        [Required] public string PackageType { get; set; } = "Basic";
        [Range(1, 100000)] public int MinGuests { get; set; } = 1;
        [Range(1, 100000)] public int MaxGuests { get; set; } = 30;
        [Range(typeof(decimal), "0.01", "9999999")] public decimal Price { get; set; } = 99m;
    }
}
