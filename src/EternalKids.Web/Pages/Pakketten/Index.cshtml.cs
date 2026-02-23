using System.Text.Json;
using EternalKids.Domain.Entities;
using EternalKids.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace EternalKids.Web.Pages.Pakketten;

public class IndexModel(EternalKidsContext dbContext) : PageModel
{
    public List<PackageVm> Packages { get; private set; } = [];
    public IReadOnlyList<ComparisonVm> ComparisonRows { get; } =
    [
        new("5-10 personen", 145, 189, 249, 305),
        new("11-20 personen", 185, 229, 299, 365),
        new("21-35 personen", 245, 299, 399, 495)
    ];

    public async Task OnGetAsync(CancellationToken ct)
    {
        var fromDb = await dbContext.PackageDefinitions.AsNoTracking().Where(x => x.IsActive).ToListAsync(ct);
        if (fromDb.Count == 0)
        {
            Packages =
            [
                new("Basic", "Digitale uitnodiging met basisaccessoires.", ["Digitale uitnodiging", "Chipszakjes", "Rietjes"]),
                new("Elegant", "Uitgebreider pakket met premium afwerking.", ["Snapchat filter", "Traktaties", "Backdrop elementen"]),
                new("Prestige", "Luxe setting met extra styling.", ["Grote backdrop", "Welkomstbord", "Candy corner"]),
                new("Deluxe", "All-in premium event concept.", ["Complete styling", "Premium drukwerk", "On-site team"])
            ];
            return;
        }

        Packages = fromDb.Select(x => new PackageVm(
            x.Title,
            x.Description,
            JsonSerializer.Deserialize<List<string>>(x.FeaturesJson) ?? [])).ToList();
    }

    public sealed record PackageVm(string Title, string Description, IReadOnlyList<string> Features);
    public sealed record ComparisonVm(string RangeLabel, decimal Basic, decimal Elegant, decimal Prestige, decimal Deluxe);
}
