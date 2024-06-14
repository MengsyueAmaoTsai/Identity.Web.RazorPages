using System.Security.Claims;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Users.SignIn;

[AllowAnonymous]
public sealed class SignInViewModel :
    PageModel
{
    [BindProperty(SupportsGet = true)]
    public required string ReturnUrl { get; set; }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        var principle = new ClaimsPrincipal(
            new ClaimsIdentity(
            [
                new Claim("sub", "1"),
                new Claim(ClaimTypes.Name, "John Doe"),
                new Claim(ClaimTypes.Email, "mengsyue.tsai@outlook.com"),
                new Claim(ClaimTypes.Role, "Admin"),
            ],
            RichillCapitalAuthenticationSchemes.Cookie));

        var properties = new AuthenticationProperties
        {
            IsPersistent = false,
            ExpiresUtc = DateTimeOffset.UtcNow.AddSeconds(30),
        };

        await HttpContext.SignInAsync(principle, properties);

        return Page();
    }
}
