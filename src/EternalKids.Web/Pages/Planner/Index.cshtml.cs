using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EternalKids.Web.Pages.Planner;

public class IndexModel : PageModel
{
    public Guid SessionId { get; private set; }

    public void OnGet()
    {
        SessionId = Guid.NewGuid();
    }
}
