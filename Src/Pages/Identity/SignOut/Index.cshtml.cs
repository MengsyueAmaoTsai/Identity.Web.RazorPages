using Duende.IdentityServer;
using Duende.IdentityServer.Services;

using IdentityModel;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.UseCases.Common;

namespace RichillCapital.Identity.Web.Pages.Identity.SignOut;

[Authorize]
public sealed class SignOutViewModel(
    IIdentityServerInteractionService _interactionService,
    ICurrentUser _currentUser,
    ISignInManager _signInManager) :
    PageModel
{
    public async Task<IActionResult> OnPostAsync(string returnUrl, CancellationToken cancellationToken = default)
    {
        var logoutId = await _interactionService.CreateLogoutContextAsync();

        if (!_currentUser.IsAuthenticated)
        {
            return RedirectToPage("/identity/singedOut", new { LogoutId = logoutId });
        }

        var result = await _signInManager.SignOutAsync(cancellationToken);

        if (result.IsFailure)
        {
            return Page();
        }

        var provider = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

        // if it's a local login we can ignore this workflow
        if (provider is not null && provider != IdentityServerConstants.LocalIdentityProvider)
        {
            if (await HttpContext.IsSchemeSupportsSignOutAsync(provider))
            {
                // build a return URL so the upstream provider will redirect back
                // to us after the user has logged out. this allows us to then
                // complete our single sign-out processing.
                // this triggers a redirect to the external provider for sign-out
                return SignOut(
                    new AuthenticationProperties
                    {
                        RedirectUri = Url.Page(
                            "/identity/signedOut",
                            new
                            {
                                LogOutId = logoutId
                            }) ?? "/identity/signed-out",
                    },
                    provider);
            }
        }

        if (!string.IsNullOrEmpty(returnUrl))
        {
            return LocalRedirect(returnUrl);
        }
        else
        {
            // This needs to be a redirect so that the browser performs a new
            // request and the identity for the user gets updated.
            return RedirectToPage();
        }
    }
}