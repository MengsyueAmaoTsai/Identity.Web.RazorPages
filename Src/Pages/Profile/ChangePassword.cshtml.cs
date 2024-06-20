using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Domain.Users;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.Pages.Profile;

[Authorize]
public sealed class ChangePasswordViewModel() : PageModel
{
    [BindProperty]
    public required string OldPassword { get; init; }

    [BindProperty]
    public required string NewPassword { get; init; }

    [BindProperty]
    public required string ConfirmPassword { get; init; }

    public required string StatusMessage { get; set; }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        // if (!ModelState.IsValid)
        // {
        //     return Page();
        // }

        // var userResult = await _userService.GetByIdAsync(_currentUser.Id, cancellationToken);

        // if (userResult.IsFailure)
        // {
        //     return NotFound(userResult.Error.Message);
        // }

        // var user = userResult.Value;

        // var changePasswordResult = await _userService.ChangePasswordAsync(
        //     user,
        //     OldPassword,
        //     NewPassword,
        //     cancellationToken);

        // if (changePasswordResult.IsFailure)
        // {
        //     ModelState.AddModelError(changePasswordResult.Error.Code, changePasswordResult.Error.Message);
        //     return Page();
        // }

        // await _signInManager.RefreshSignInAsync(maybeUser.Value);

        // StatusMessage = "Your password has been changed.";

        return RedirectToPage();
    }

    private async Task<Result> ChangePasswordAsync(
        User user,
        string oldPassword,
        string newPassword,
        CancellationToken cancellationToken = default)
    {
        return Result.Success;
    }
}