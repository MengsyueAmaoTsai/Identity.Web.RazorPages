using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Profile.Security;

public sealed class ChangePasswordViewModel : PageModel
{
    [BindProperty]
    [Display(Name = "Current Password")]
    public required string CurrentPassword { get; set; }

    [BindProperty]
    [Display(Name = "New Password")]
    public required string NewPassword { get; set; }

    [BindProperty]
    [Display(Name = "Confirm Password")]
    public required string ConfirmPassword { get; set; }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        return RedirectToPage("./index");
    }
}