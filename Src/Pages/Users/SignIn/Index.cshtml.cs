using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

using RichillCapital.Domain;
using RichillCapital.Domain.Common.Repositories;

namespace RichillCapital.Identity.Web.Pages.Users.SignIn;

[AllowAnonymous]
public sealed class SignInViewModel(
    IReadOnlyRepository<User> _userRepository,
    IOptionsSnapshot<IdentityOptions> _identityOptions) :
    PageModel
{
    [BindProperty(SupportsGet = true)]
    public required string ReturnUrl { get; set; }

    [BindProperty]
    public required string Email { get; init; }

    [BindProperty]
    public required string Password { get; init; }

    [BindProperty]
    public bool RememberMe { get; init; }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        var emailResult = Domain.Email.From(Email);

        if (emailResult.IsFailure)
        {
            ModelState.AddModelError(emailResult.Error.Code, emailResult.Error.Message);
            return Page();
        }

        var email = emailResult.Value;

        var maybeUser = await _userRepository.FirstOrDefaultAsync(
            user => user.Email == email,
            cancellationToken);

        if (maybeUser.IsNull)
        {
            ModelState.AddModelError("UserNotFound", "User not found.");
            return Page();
        }

        var user = maybeUser.Value;

        if (!(user.Password == Password))
        {
            ModelState.AddModelError("InvalidPassword", "Invalid password.");
            return Page();
        }

        var principle = new ClaimsPrincipal(
            new ClaimsIdentity(
            [
                new Claim("sub", user.Id.Value),
                new Claim(ClaimTypes.Email, user.Email.Value),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, "Admin"),
            ],
            RichillCapitalAuthenticationSchemes.Cookie));

        var properties = RememberMe ?
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(_identityOptions.Value.RememberMeDurationDays),
            } :
            null;

        await HttpContext.SignInAsync(principle, properties);

        return Page();
    }
}
