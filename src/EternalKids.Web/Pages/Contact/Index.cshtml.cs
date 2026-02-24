using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EternalKids.Web.Pages.Contact;

public class IndexModel : PageModel
{
    [BindProperty]
    public ContactInput Input { get; set; } = new();

    public bool Sent { get; private set; }

    public void OnGet() { }

    public void OnPost()
    {
        if (!ModelState.IsValid)
        {
            return;
        }

        Sent = true;
    }

    public sealed class ContactInput
    {
        [Required] public string Name { get; set; } = string.Empty;
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required] public string Subject { get; set; } = string.Empty;
        [Required] public string Message { get; set; } = string.Empty;
    }
}
