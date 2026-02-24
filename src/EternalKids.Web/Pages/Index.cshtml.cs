using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EternalKids.Web.Pages;

public class IndexModel : PageModel
{
    public IReadOnlyList<ServiceVm> Services { get; } =
    [
        new("Party Designs", "Volledige event styling en thema-concepten."),
        new("Drukwerk", "Welkomstborden, uitnodigingen en gepersonaliseerde signage."),
        new("Traktaties", "Premium snack- en sweet tables met personalisatie.")
    ];

    public IReadOnlyList<ReviewVm> Reviews { get; } =
    [
        new("Echt een wow-ervaring voor ons kinderfeest.", "Nadia"),
        new("Professioneel, stijlvol en tot in detail verzorgd.", "Sanne"),
        new("De AI planner en snelle reservering werkten perfect.", "Yara")
    ];

    public sealed record ServiceVm(string Title, string Description);
    public sealed record ReviewVm(string Text, string Author);
}
