using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages;

[AllowAnonymous]
public class IndexViewModel :
    PageModel
{
    public required AuthenticationProperties Properties { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        var authenticationResult = await HttpContext.AuthenticateAsync();

        if (!authenticationResult.Succeeded)
        {
            return RedirectToPage("/error");
        }

        Properties = authenticationResult.Properties;

        return Page();
    }
}
