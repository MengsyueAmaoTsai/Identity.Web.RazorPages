using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.UseCases.Common;

namespace RichillCapital.Identity.Web.Pages.Profile.Security;

public sealed class ChangePasswordViewModel(
    IUserManager _userManager,
    ICurrentUser _currentUser) :
    PageModel
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
        var userResult = await _userManager.GetByIdAsync(_currentUser.Id, cancellationToken);

        if (userResult.IsFailure)
        {
            return NotFound();
        }

        var user = userResult.Value;

        var changePasswordResult = await _userManager.ChangePasswordAsync(
            user,
            CurrentPassword,
            NewPassword,
            cancellationToken);

        if (changePasswordResult.IsFailure)
        {
            return Page();
        }

        var result = await _signInManager.SignInAsync(user, isPersistent: true);

        if (result.IsFailure)
        {
            return Page();
        }

        return RedirectToPage("./index");
    }
}