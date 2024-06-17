using System.Security.Claims;

using Duende.IdentityServer;
using Duende.IdentityServer.Services;

using IdentityModel;

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

        var validationResult = Result<(Email, UserName)>.Combine(
            Email.From(emailClaim.Value),
            UserName.From(nameClaim.Value));

        var (email, name) = validationResult.Value;

        var maybeUser = await _userRepository
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);

        if (maybeUser.IsNull)
        {
            var claims = externalUser.Claims.ToList();
            claims.Remove(idClaim);
        }

        var user = maybeUser.IsNull ? Domain.User
            .Create(
                UserId.NewUserId(),
                name,
                email,
                PhoneNumber.Empty,
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

        CaptureExternalLoginContext(
            externalAuthenticationResult,
            additionalLocalClaims,
            localSignInProps);

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

    private static void CaptureExternalLoginContext(
        AuthenticateResult authenticateResult,
        List<Claim> localClaims,
        AuthenticationProperties properties)
    {
        ArgumentNullException.ThrowIfNull(
            authenticateResult.Principal, 
            nameof(authenticateResult.Principal));

        // capture the idp used to login, so the session knows where the user came from
        localClaims.Add(new Claim(
            JwtClaimTypes.IdentityProvider, 
            authenticateResult.Properties?.Items["scheme"] ?? 
                "unknown identity provider"));

        // if the external system sent a session id claim, copy it over so we can use it for single sign-out
        var sessionId = authenticateResult.Principal.Claims
            .FirstOrDefault(claim => claim.Type == JwtClaimTypes.SessionId);
        
        if (sessionId is not null)
        {
            localClaims.Add(new Claim(JwtClaimTypes.SessionId, sessionId.Value));
        }

        // if the external provider issued an id_token, we'll keep it for signout
        var idToken = authenticateResult.Properties?.GetTokenValue("id_token");

        if (idToken is not null)
        {
            properties.StoreTokens([new AuthenticationToken 
            { 
                Name = "id_token", 
                Value = idToken 
            }]);
        }
    }
}


internal sealed record ExternalSignInInfo
{
    public required string ProviderKey { get; init; }
    public required string AuthenticationProvider { get; init; }
}