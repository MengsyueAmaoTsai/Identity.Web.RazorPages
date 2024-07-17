using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;
using RichillCapital.UseCases.Common;

namespace RichillCapital.Identity.Web.Pages.Profile;

[Authorize]
public class ProfileViewModel(
    ICurrentUser _currentUser,
    IReadOnlyRepository<User> _userRepository) :
    ViewModel
{
    public required User CurrentUser { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        var maybeUser = await _userRepository
            .GetByIdAsync(_currentUser.Id, cancellationToken);

        if (maybeUser.IsNull)
        {
            return ErrorPage();
        }

        CurrentUser = maybeUser.Value;

        return Page();
    }
}
