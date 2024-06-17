using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Domain;
using RichillCapital.Domain.Common.Repositories;
using RichillCapital.UseCases.Common;

namespace RichillCapital.Identity.Web.Pages.Identity.Profile;

[Authorize]
public sealed class ProfileViewModel(
    ICurrentUser _currentUser,
    IReadOnlyRepository<User> _userRepository) :
    PageModel
{
    public required string StatusMessage { get; init; }

    public required bool HasPassword { get; set; }

    public required string PhoneNumber { get; set; }

    public required bool EnabledTwoFactorAuthentication { get; set; }

    public required bool BrowserRemembered { get; set; }

    public required string AuthenticatorKey { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        // Get current user
        var maybeUser = await _userRepository.GetByIdAsync(_currentUser.Id, cancellationToken);

        if (maybeUser.IsNull)
        {
            return NotFound($"Unable to load user with ID '{_currentUser.Id}'.");
        }

        var user = maybeUser.Value;

        HasPassword = !string.IsNullOrEmpty(user.Password);
        PhoneNumber = user.PhoneNumber.Value;
        EnabledTwoFactorAuthentication = user.TwoFactorEnabled;
        
        return Page();
    }
}