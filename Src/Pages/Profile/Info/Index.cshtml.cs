using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Common;

namespace RichillCapital.Identity.Web.Pages.Profile.Info;

public sealed class ProfileInfoViewModel(
    ICurrentUser _currentUser,
    IReadOnlyRepository<User> _userRepository) : PageModel
{
    public required string Name { get; set; }
    public required string EmailAddress { get; set; }

    public async Task<IActionResult> OnGetAsync()
    {
        var maybeUser = await _userRepository.GetByIdAsync(_currentUser.Id).ThrowIfNull();

        var user = maybeUser.Value;

        Name = user.Name.Value;
        EmailAddress = user.Email.Value;
        
        return Page();
    }
}