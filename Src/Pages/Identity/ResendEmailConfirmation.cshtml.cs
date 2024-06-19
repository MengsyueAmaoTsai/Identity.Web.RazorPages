using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Domain;
using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;

namespace RichillCapital.Identity.Web.Pages.Identity;

[AllowAnonymous]
public sealed class ResendEmailConfirmationViewModel(
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

        var user = maybeUser.Value;

        var callbackUrl = Url.Page(
            "/identity/confirmEmail",
            pageHandler: null,
            values: new { userId = user.Id.Value, code = string.Empty },
            protocol: Request.Scheme)!;

        // await _emailSender.SendConfirmationLinkAsync(user, Input.Email, HtmlEncoder.Default.Encode(callbackUrl));

        ModelState.AddModelError(string.Empty, "Verification email sent. Please check your email.");

        return Page();
    }
}