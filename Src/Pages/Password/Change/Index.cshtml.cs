using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace RichillCapital.Identity.Web.Pages.Password.Change;

[Authorize]
public sealed class PasswordChangeViewModel() : ViewModel
{
    [BindProperty]
    [Display(Name = "Current password")]
    [Required(ErrorMessage = "Please enter your current password")]
    public required string CurrentPassword { get; init; }

    [BindProperty]
    [Display(Name = "New password")]
    [Required(ErrorMessage = "Please enter your new password")]
    public required string NewPassword { get; init; }

    [BindProperty]
    [Display(Name = "Reenter password")]
    [Required(ErrorMessage = "Please reenter your new password")]
    public required string ReenterPassword { get; init; }

    [BindProperty]
    [Display(Name = "Make me change my password every 72 days")]
    public required bool ChangePasswordEvery72Days { get; init; }

    public async Task<IActionResult> OnPostAsync(
        CancellationToken cancellationToken = default)
    {
        return ErrorPage();
    }
}