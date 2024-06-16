using System.Security.Claims;

using Duende.IdentityServer;
using Duende.IdentityServer.Services;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Domain;
using RichillCapital.Domain.Common.Repositories;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.Pages.Identity;

public sealed class CallbackViewModel(
    IRepository<User> _userRepository,
    IIdentityServerInteractionService _interactionService) :
    PageModel
{
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        var externalAuthenticationResult = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

        if (!externalAuthenticationResult.Succeeded)
        {
            throw new InvalidOperationException("External authentication failed.");
        }

        var externalUser = externalAuthenticationResult.Principal ??
            throw new InvalidOperationException("External authentication failed.");

        var emailClaim = externalUser.FindFirst(ClaimTypes.Email) ??
            throw new InvalidOperationException("External authentication failed.");

        var email = Email.From(emailClaim.Value).ThrowIfFailure().Value;

        var maybeUser = await _userRepository
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);

        if (maybeUser.IsNull)
        {
            // 
        }

        await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

        var returnUrl = externalAuthenticationResult.Properties.Items["returnUrl"] ?? "~/";

        var request = await _interactionService.GetAuthorizationContextAsync(returnUrl);

        if (request is not null)
        {
            if (request.IsNativeClient())
            {
                // The client is native, so this change in how to
                // return the response is for better UX for the end user.
                return this.LoadingPage(returnUrl);
            }
        }

        return Redirect(returnUrl);
    }
}