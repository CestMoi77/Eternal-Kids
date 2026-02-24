using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EternalKids.Web.Pages.Diensten;

public class IndexModel : PageModel
{
    public IReadOnlyList<ServiceVm> Services { get; } =
    [
        new("party-design", "Party Designs", "Van conceptboard tot complete installatie.", "Wij ontwerpen en realiseren complete event settings op maat met premium materialen en styling."),
        new("drukwerk", "Drukwerk", "Uitnodigingen en signage in luxe stijl.", "Exclusieve uitnodigingen, backdrop prints, menukaarten en welkomstsigns."),
        new("traktaties", "Traktaties", "Sweet tables en premium traktatiepakketten.", "Gepersonaliseerde traktaties met aandacht voor dieetwensen zoals halal/allergieën."),
        new("decoratie", "Decoratie", "Balloninstallaties en tafelstyling.", "Van subtiele tafelaccenten tot opvallende showpieces."),
        new("ai", "AI Planner", "Slimme planner met realtime prijzen.", "Onze AI helpt met pakketadvies, prijsinschatting en directe boeking."),
        new("digitale", "Digitale Producten", "Snapchat filters en e-invites.", "Digitale uitnodigingen en social assets voor jouw event.")
    ];

    public sealed record ServiceVm(string Key, string Title, string Description, string LongDescription);
}
