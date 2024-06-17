using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Identity;

public sealed class SignedOutViewModel : PageModel
{
    public required string ClientName { get; set; }
    public required string PostLogoutRedirectUri { get; set; }
    public required string SignOutIframeUrl { get; set; }
    public required bool AutomaticRedirectAfterSignOut { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        return Page();
    }
}