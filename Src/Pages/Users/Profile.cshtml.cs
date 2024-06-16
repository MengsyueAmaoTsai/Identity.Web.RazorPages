using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Domain;
using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Identity.Web.Models.Users;
using RichillCapital.UseCases.Common;

namespace RichillCapital.Identity.Web.Pages.Users;

public sealed class UserProfileViewModel(
    ICurrentUser _currentUser,
    IReadOnlyRepository<User> _userRepository) :
    PageModel
{
    public new required UserModel User { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        var maybeUser = await _userRepository.GetByIdAsync(_currentUser.Id, cancellationToken);

        if (maybeUser.IsNull)
        {
            return NotFound();
        }

        var user = maybeUser.Value;

        User = new UserModel
        {
            Id = user.Id.Value,
            Email = user.Email.Value,
            Name = user.Name.Value,
            AvatarUrl = string.Empty,
        };

        return Page();
    }
}