using Duende.IdentityServer.Services;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Identity;

[Authorize]
public sealed class ConsentViewModel(
    IIdentityServerInteractionService _interactionService) :
    PageModel
{
    [BindProperty]
    public required string ReturnUrl { get; init; }

    public required string ClientName { get; set; }
    public required string ClientUrl { get; set; }
    public required string ClientLogoUrl { get; set; }
    public required bool AllowRememberConsent { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        var request = await _interactionService.GetAuthorizationContextAsync(ReturnUrl);

        if (request is null)
        {
            return RedirectToPage("/error/index");
        }

        ClientName = request.Client.ClientName ?? request.Client.ClientId;
        ClientUrl = request.Client.ClientUri ?? string.Empty;
        ClientLogoUrl = request.Client.LogoUri ?? string.Empty;
        AllowRememberConsent = request.Client.AllowRememberConsent;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        var request = await _interactionService.GetAuthorizationContextAsync(ReturnUrl);

        if (request is null)
        {
            return RedirectToPage("/error/index");
        }


        return Page();
    }
}