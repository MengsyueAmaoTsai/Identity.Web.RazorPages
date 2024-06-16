using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Identity;

[Authorize]
public sealed class IdentityManageViewModel : PageModel
{
    public async Task<IActionResult> OnGetAsync(
        CancellationToken cancellationToken = default)
    {
        return Page();
    }
}