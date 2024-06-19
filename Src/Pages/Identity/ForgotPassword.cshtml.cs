using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;

namespace RichillCapital.Identity.Web.Pages.Identity;

[AllowAnonymous]
public sealed class ForgotPasswordViewModel(
    IReadOnlyRepository<User> _userRepository) :
    PageModel
{
    [BindProperty]
    public required string Email { get; init; }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        var validationResult = Domain.Users.Email.From(Email);

        if (validationResult.IsFailure)
        {
            ModelState.AddModelError(validationResult.Error.Code, validationResult.Error.Message);
            return Page();
        }

        var email = validationResult.Value;

        var maybeUser = await _userRepository.FirstOrDefaultAsync(user => user.Email == email, cancellationToken);

        if (maybeUser.IsNull)
        {
            ModelState.AddModelError("Email", "User with this email does not exist.");
            return Page();
        }

        // generate and send password reset token

        return Redirect("./forgotPasswordConfirmation");
    }
}