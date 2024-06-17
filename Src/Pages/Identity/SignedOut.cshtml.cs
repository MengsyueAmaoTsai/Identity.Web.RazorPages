using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Identity;

public sealed class SignedOutViewModel : PageModel
{
    public required string LogoutId { get; init; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        return Page();
    }
}