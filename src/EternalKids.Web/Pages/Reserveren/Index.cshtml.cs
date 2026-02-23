using System.ComponentModel.DataAnnotations;
using EternalKids.Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace EternalKids.Web.Pages.Reserveren;

public class IndexModel : PageModel
{
    [BindProperty]
    public ReserveringInput Input { get; set; } = new();

    public bool Submitted { get; private set; }

    public void OnGet() { }

    public void OnPost()
    {
        if (!ModelState.IsValid)
        {
            return;
        }

        Submitted = true;
    }

    public sealed class ReserveringInput
    {
        [Required] public string EventType { get; set; } = "Kinderfeest";
        [Range(1, 10000)] public int GuestCount { get; set; } = 30;
        public DateOnly EventDate { get; set; } = DateOnly.FromDateTime(DateTime.Today.AddDays(14));
        public string? ChildrenAge { get; set; }
        public string? CompanyName { get; set; }
        public string PackageType { get; set; } = "Elegant";
        public decimal Budget { get; set; }
        public string? Notes { get; set; }
        [Required] public string Name { get; set; } = string.Empty;
        [Required, EmailAddress] public string Email { get; set; } = string.Empty;
        [Required] public string Phone { get; set; } = string.Empty;
        public PaymentMode PaymentMode { get; set; } = PaymentMode.Deposit50;
    }
}
