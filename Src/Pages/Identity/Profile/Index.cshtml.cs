using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.UseCases.Common;

namespace RichillCapital.Identity.Web.Pages.Identity.Profile;

[Authorize]
public sealed class ProfileViewModel(
    ICurrentUser _currentUser) :
    PageModel
{
    [TempData]
    public required string StatusMessage { get; init; }

    public required bool HasPassword { get; set; }

    public required string PhoneNumber { get; set; }

    public required bool EnabledTwoFactorAuthentication { get; set; }

    public required bool BrowserRemembered { get; set; }

    public required string AuthenticatorKey { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        // var userResult = await _userService.GetByIdAsync(_currentUser.Id, cancellationToken);

        // if (userResult.IsFailure)
        // {
        //     return NotFound(userResult.Error.Message);
        // }

        // var user = userResult.Value;

        // HasPassword = !string.IsNullOrEmpty(user.Password);
        // PhoneNumber = user.PhoneNumber.Value;
        // EnabledTwoFactorAuthentication = user.TwoFactorEnabled;

        return Page();
    }
}