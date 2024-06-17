using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Identity.Profile;

[Authorize]
public sealed class TwoFactorAuthenticationViewModel : PageModel
{
    [TempData]
    public required string StatusMessage { get; init; }

    public required bool HasAuthenticator { get; init; }
    public required bool Is2faEnabled { get; init; }
    public required bool IsMachineRemembered { get; init; }
    public required int RecoveryCodesLeft { get; init; }

    public IActionResult OnGet()
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        return RedirectToPage();
    }
}