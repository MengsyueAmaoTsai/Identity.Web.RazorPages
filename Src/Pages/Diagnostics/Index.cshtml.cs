using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Diagnostics;

[Authorize]
public sealed class DiagnosticsViewModel :
    PageModel
{
    public async Task<IActionResult> OnGetAsync()
    {
        var result = await HttpContext.AuthenticateAsync();

        return Page();
    }
}
