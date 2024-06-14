using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Diagnostics;

[Authorize]
public class DiagnosticsViewModel : PageModel
{
    public required AuthenticationProperties Properties { get; set; }
    
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
