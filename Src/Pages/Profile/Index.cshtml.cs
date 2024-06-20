using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;
using RichillCapital.UseCases.Common;

namespace RichillCapital.Identity.Web.Pages.Profile;

[Authorize]
public sealed class ProfileViewModel(
    IUserManager _userManager,
    ICurrentUser _currentUser) :
    PageModel
{
    [TempData]
    public required string StatusMessage { get; init; }

    public required string Email { get; set; }

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