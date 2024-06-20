using Duende.IdentityServer.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Identity;

public sealed class SignedOutViewModel(
    IIdentityServerInteractionService _interactionService) : PageModel
{
    public required string ClientName { get; set; }
    public required string PostLogoutRedirectUri { get; set; }
    public required string SignOutIFrameUrl { get; set; }
    public required bool AutomaticRedirectAfterSignOut { get; set; }

    public async Task<IActionResult> OnGetAsync(
        string logoutId,
        CancellationToken _ = default)
    {
        var request = await _interactionService.GetLogoutContextAsync(logoutId);

        if (request is not null)
        {
            PostLogoutRedirectUri = request.PostLogoutRedirectUri ?? string.Empty;
            ClientName = request.ClientName ?? request.ClientId ?? string.Empty;
            SignOutIFrameUrl = request.SignOutIFrameUrl ?? string.Empty;
        }

        AutomaticRedirectAfterSignOut = true;

        return Page();
    }
}