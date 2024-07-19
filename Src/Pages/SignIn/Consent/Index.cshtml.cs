using Duende.IdentityServer.Services;

using Microsoft.AspNetCore.Mvc;

namespace RichillCapital.Identity.Web.Pages.SignIn.Consent;

public sealed class SignInConsentViewModel(
    IIdentityServerInteractionService _interactionService) :
    IdentityViewModel
{
    [BindProperty]
    public required string Description { get; init; }

    [BindProperty]
    public required bool RememberMyDecision { get; init; }

    public required string ClientName { get; set; }
    public required string ClientUrl { get; set; }
    public required string ClientLogoUri { get; set; }
    public required bool AllowRememberConsent { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(ReturnUrl))
        {
            return ErrorPage();
        }

        var context = await _interactionService.GetAuthorizationContextAsync(ReturnUrl);

        if (context is null)
        {
            return ErrorPage();
        }

        ClientName = context.Client.ClientName ?? context.Client.ClientId;
        ClientUrl = context.Client.ClientUri ?? string.Empty;
        ClientLogoUri = context.Client.LogoUri ?? string.Empty;
        AllowRememberConsent = context.Client.AllowRememberConsent;

        return Page();
    }
}