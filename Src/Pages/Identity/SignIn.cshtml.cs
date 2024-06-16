using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Identity;

[AllowAnonymous]
public sealed class SignInViewModel(
    IAuthenticationSchemeProvider _schemeProvider) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty]
    public required string Email { get; init; }

    [BindProperty]
    public required string Password { get; init; }

    [BindProperty]
    public required bool RememberMe { get; init; }

    public required IEnumerable<AuthenticationScheme> ExternalSchemes { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        await InitializeAsync(cancellationToken);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        await InitializeAsync(cancellationToken);
        return Page();
    }

    private async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        ExternalSchemes = await _schemeProvider.GetExternalSchemesAsync();
    }
}