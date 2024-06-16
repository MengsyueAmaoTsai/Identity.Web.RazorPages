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

        var idClaim = externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
            throw new InvalidOperationException("External authentication failed.");

        var emailClaim = externalUser.FindFirst(ClaimTypes.Email) ??
            throw new InvalidOperationException("External authentication failed.");
        
        var nameClaim = externalUser.FindFirst(ClaimTypes.Name) ??
            throw new InvalidOperationException("External authentication failed.");

        var validationResult = Result<(Email, UserName, PhoneNumber)>.Combine(
            Email.From(emailClaim.Value),
            UserName.From(nameClaim.Value),
            PhoneNumber.From(""));

        var (email, name, phoneNumber) = validationResult.Value;

        var maybeUser = await _userRepository
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);

        if (maybeUser.IsNull)
        {
            var claims = externalUser.Claims.ToList();
            claims.Remove(idClaim);
        }

        //user = _users.AutoProvisionUser(provider, providerUserId, claims.ToList());
        var user = maybeUser.IsNull ? Domain.User.Create(
            UserId.NewUserId(),
            name,
            email,
            phoneNumber,
            "123",
            lockoutEnabled: true,
            twoFactorEnabled: true,
            emailConfirmed: true,
            phoneNumberConfirmed: false,
            accessFailedCount: 0,
            lockoutEnd: DateTimeOffset.UtcNow).Value :
            maybeUser.Value;

        var additionalLocalClaims = new List<Claim>();
        var localSignInProps = new AuthenticationProperties();
        //CaptureExternalLoginContext(result, additionalLocalClaims, localSignInProps);

        var provider = externalAuthenticationResult.Properties.Items["scheme"] ?? 
            throw new InvalidOperationException("Null scheme in authentiation properties");

        var isuser = new IdentityServerUser(user.Id.Value)
        {
            DisplayName = user.Name.Value,
            IdentityProvider = provider,
            AdditionalClaims = additionalLocalClaims
        };
        await HttpContext.SignInAsync(isuser, localSignInProps);
        
        // After sign in
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