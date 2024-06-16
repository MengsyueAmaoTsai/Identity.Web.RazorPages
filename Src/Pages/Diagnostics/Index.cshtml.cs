using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.UseCases.Common;

namespace RichillCapital.Identity.Web.Pages.Diagnostics;

[Authorize]
public class DiagnosticsViewModel(
    IAuthenticationSchemeProvider _authenticationSchemeProvider,
    ICurrentUser _currentUser) : PageModel
{
    public required AuthenticationProperties Properties { get; set; }
    public ICurrentUser CurrentUser => _currentUser;
    public required IEnumerable<AuthenticationScheme> ExternalSchemes { get; set; } = [];

    public async Task<IActionResult> OnGetAsync()
    {
        ExternalSchemes = await _authenticationSchemeProvider.GetExternalSchemesAsync();

        var result = await HttpContext.AuthenticateAsync();

        if (!result.Succeeded)
        {
            return RedirectToPage("/users/sign-in", new { ReturnUrl = "/" });
        }

        Properties = result.Properties;

        return Page();
    }
}
