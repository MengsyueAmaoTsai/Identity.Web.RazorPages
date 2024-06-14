using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Domain;
using RichillCapital.Domain.Common.Repositories;

namespace RichillCapital.Identity.Web.Pages.Users.ForgotPassword;

public sealed class ForgotPasswordViewModel(
    IReadOnlyRepository<User> _userRepository) : PageModel
{
    [BindProperty]
    public required string Email { get; init; }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken)
    {
        var emailResult = Domain.Email.From(Email);

        if (emailResult.IsFailure)
        {
            ModelState.AddModelError(emailResult.Error.Code, emailResult.Error.Message);

            return Page();
        }

        var email = emailResult.Value;

        var maybeUser = await _userRepository.FirstOrDefaultAsync(user => user.Email == email, cancellationToken);

        if (maybeUser.IsNull)
        {
            ModelState.AddModelError("UserNotFound", "User with this email does not exist");
            return Page();
        }

        var passwordResetToken = string.Empty;
        var resetUrl = string.Empty;

        return Page();
    }
}