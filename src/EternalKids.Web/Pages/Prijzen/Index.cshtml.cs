using System.ComponentModel.DataAnnotations;
using EternalKids.Application.Contracts;
using EternalKids.Application.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EternalKids.Web.Pages.Prijzen;

public class IndexModel(IPricingService pricingService) : PageModel
{
    [BindProperty]
    public PricingInputModel Input { get; set; } = new();

    public List<PackageCardViewModel> PackageCards { get; private set; } = [];
    public string? ErrorMessage { get; private set; }

    public IReadOnlyList<SelectListItem> EventTypeOptions { get; } =
    [
        new("Kinderfeest", "Kinderfeest"),
        new("Verjaardag", "Verjaardag"),
        new("Babyshower", "Babyshower"),
        new("Bruiloft", "Bruiloft"),
        new("Bedrijfsevenement", "Bedrijfsevenement")
    ];

    public async Task OnGetAsync(CancellationToken ct)
    {
        await LoadPackageCardsAsync(ct);
    }

    public async Task OnPostAsync(CancellationToken ct)
    {
        if (!ModelState.IsValid)
        {
            await LoadPackageCardsAsync(ct, includePrices: false);
            return;
        }

        await LoadPackageCardsAsync(ct);
    }

    private async Task LoadPackageCardsAsync(CancellationToken ct, bool includePrices = true)
    {
        var cards = new List<PackageCardViewModel>
        {
            new("Basic", false, ["Styling basisset", "Moodboard", "Basis decoratie"]),
            new("Elegant", true, ["Premium decoratie", "Thema-afwerking", "Social media-ready setup"]),
            new("Prestige", false, ["Luxe styling", "Guest experience hoek", "Uitgebreide tafelopmaak"]),
            new("Deluxe", false, ["All-in concept", "Personalisatie", "Dedicated event styling team"])
        };

        if (!includePrices)
        {
            PackageCards = cards;
            return;
        }

        foreach (var card in cards)
        {
            try
            {
                var result = await pricingService.CalculateAsync(new PriceCalculationRequest(
                    EventType: Input.EventType,
                    PackageType: card.PackageType,
                    GuestCount: Input.GuestCount,
                    Quantity: 1), ct);

                card.Price = result.LineTotal;
            }
            catch
            {
                ErrorMessage = "Niet alle pakketprijzen konden worden geladen. Controleer of Prijzenbeheer volledig is ingericht.";
                card.Price = 0m;
            }
        }

        PackageCards = cards;
    }

    public sealed class PricingInputModel
    {
        [Required]
        public string EventType { get; set; } = "Kinderfeest";

        [Range(1, 10000)]
        public int GuestCount { get; set; } = 30;
    }

    public sealed class PackageCardViewModel(string packageType, bool isFeatured, IReadOnlyCollection<string> highlights)
    {
        public string PackageType { get; } = packageType;
        public bool IsFeatured { get; } = isFeatured;
        public IReadOnlyCollection<string> Highlights { get; } = highlights;
        public decimal Price { get; set; }
    }
}
