using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Password.Change;

public sealed class PasswordChangeViewModel : PageModel
{
    [BindProperty]
    [Display(Name = "Current password")]
    public required string CurrentPassword { get; init; }

    [BindProperty]
    [Display(Name = "New password")]
    public required string NewPassword { get; init; }

    [BindProperty]
    [Display(Name = "Reenter password")]
    public required string ReenterPassword { get; init; }

    public async Task<IActionResult> OnPostAsync()
    {
        return Page();
    }
}