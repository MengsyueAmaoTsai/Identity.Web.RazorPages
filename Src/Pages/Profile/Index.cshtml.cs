using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.UseCases.Common;

namespace RichillCapital.Identity.Web.Pages.Profile;

[Authorize]
public sealed class ProfileViewModel(
    IUserManager _userManager,
    ICurrentUser _currentUser) :
    PageModel
{
    [TempData]
    public string StatusMessage { get; init; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        var userResult = await _userManager.GetByIdAsync(_currentUser.Id, cancellationToken);

        if (userResult.IsFailure)
        {
            return NotFound(userResult.Error.Message);
        }

        var user = userResult.Value;

        Email = user.Email.Value;
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