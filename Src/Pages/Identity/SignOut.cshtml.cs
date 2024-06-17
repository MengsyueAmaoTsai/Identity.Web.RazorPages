using Duende.IdentityServer;
using Duende.IdentityServer.Services;

using IdentityModel;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Common;

namespace RichillCapital.Identity.Web.Pages.Identity;

[Authorize]
public sealed class SignOutViewModel(
    ICurrentUser _currentUser,
    IIdentityServerInteractionService _interactionService) : 
    PageModel
{
    public async Task<IActionResult> OnPostAsync(string returnUrl, CancellationToken cancellationToken = default)
    {
        var logoutId = await _interactionService.CreateLogoutContextAsync();

        if (!_currentUser.IsAuthenticated)
        {
            return RedirectToPage("/identity/singedOut", new { LogoutId = logoutId });
        }
            
        var result = await SignOutAsync(cancellationToken);

        var provider = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

        // if it's a local login we can ignore this workflow
        if (provider is not null && provider != IdentityServerConstants.LocalIdentityProvider)
        {
            if (await HttpContext.IsSchemeSupportsSignOutAsync(provider))
            {
                // build a return URL so the upstream provider will redirect back
                // to us after the user has logged out. this allows us to then
                // complete our single sign-out processing.
                string url = Url.Page("/identity/signedOut", new { LogOutId = logoutId }) ?? "/identity/signed-out";

                // this triggers a redirect to the external provider for sign-out
                return SignOut(new AuthenticationProperties { RedirectUri = url }, provider);
            }
        }

        if (result.IsFailure)
        {
            return Page();
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

    private async Task<Result> SignOutAsync(
        CancellationToken cancellationToken = default)
    {
        await HttpContext.SignOutAsync();
        return Result.Success;
    }
}