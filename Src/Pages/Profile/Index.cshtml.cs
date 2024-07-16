using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Common;

namespace RichillCapital.Identity.Web.Pages.Profile;

[Authorize]
public sealed class ProfileViewModel(
    ICurrentUser _currentUser, 
    IReadOnlyRepository<User> _userRepository): 
    PageModel
{
    public required User CurrentUser { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        var maybeUser = await _userRepository
            .GetByIdAsync(_currentUser.Id, cancellationToken)
            .ThrowIfNull();

        CurrentUser = maybeUser.Value;

        return Page();
    }
}