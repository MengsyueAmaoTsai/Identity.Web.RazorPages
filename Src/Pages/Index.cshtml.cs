using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages;

[Authorize]
public class IndexViewModel() :
    PageModel
{
    public required AuthenticationProperties Properties { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken _ = default)
    {
        var authenticationResult = await HttpContext.AuthenticateAsync();

        if (!authenticationResult.Succeeded)
        {
            return RedirectToPage("/identity/signIn");
        }

        Properties = authenticationResult.Properties;

        return Page();
    }
}
