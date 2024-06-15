using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.UseCases.Common;

namespace RichillCapital.Identity.Web.Pages.Diagnostics;

[Authorize]
public class DiagnosticsViewModel(ICurrentUser _currentUser) : PageModel
{
    public required AuthenticationProperties Properties { get; set; }
    public ICurrentUser CurrentUser => _currentUser;

    public async Task<IActionResult> OnGetAsync()
    {
        var result = await HttpContext.AuthenticateAsync();

        if (!result.Succeeded)
        {
            return RedirectToPage("/users/signin", new { ReturnUrl = "/" });
        }

        Properties = result.Properties;

        return Page();
    }
}
