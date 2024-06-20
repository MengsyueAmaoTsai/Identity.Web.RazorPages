using System.Security.Claims;

using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;

using IdentityModel;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Domain;
using RichillCapital.Domain.Users;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.Pages.Identity;

public sealed class CallbackViewModel(
    IUserService _userService,
    IIdentityServerInteractionService _interactionService,
    IEventService _eventService) :
    PageModel
{
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        // var externalAuthenticationResult = await HttpContext.AuthenticateAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

        // if (!externalAuthenticationResult.Succeeded)
        // {
        //     throw new InvalidOperationException("External authentication failed.");
        // }

        // var externalUser = externalAuthenticationResult.Principal ??
        //     throw new InvalidOperationException("External authentication failed.");

        // var idClaim = externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
        //     throw new InvalidOperationException("External authentication failed.");

        // var emailClaim = externalUser.FindFirst(ClaimTypes.Email) ??
        //     throw new InvalidOperationException("External authentication failed.");

        // var nameClaim = externalUser.FindFirst(ClaimTypes.Name) ??
        //     throw new InvalidOperationException("External authentication failed.");

        // var validationResult = Result<(Email, UserName)>.Combine(
        //     Email.From(emailClaim.Value),
        //     UserName.From(nameClaim.Value));

        // var (email, name) = validationResult.Value;

        // var userResult = await _userService.GetByEmailAsync(email, cancellationToken);

        // if (userResult.IsFailure)
        // {
        //     var claims = externalUser.Claims.ToList();
        //     claims.Remove(idClaim);
        // }

        // var user = userResult.IsFailure ?
        //     Domain.Users.User.Create(
        //         UserId.NewUserId(),
        //         name,
        //         email,
        //         PhoneNumber.From("23").Value,
        //         "123",
        //         lockoutEnabled: true,
        //         twoFactorEnabled: true,
        //         emailConfirmed: true,
        //         phoneNumberConfirmed: false,
        //         accessFailedCount: 0,
        //         lockoutEnd: DateTimeOffset.UtcNow,
        //         createdAt: DateTimeOffset.UtcNow).Value :


        //     userResult.Value;

        // var additionalClaims = new List<Claim>();
        // var properties = new AuthenticationProperties();

        // CaptureExternalLoginContext(
        //     externalAuthenticationResult,
        //     additionalClaims,
        //     properties);

        // var provider = externalAuthenticationResult.Properties.Items["scheme"] ??
        //     throw new InvalidOperationException("Null scheme in authentication properties");

        // var identityServerUser = new IdentityServerUser(user.Id.Value)
        // {
        //     DisplayName = user.Name.Value,
        //     IdentityProvider = provider,
        //     AdditionalClaims = additionalClaims,
        // };

        // await HttpContext.SignInAsync(identityServerUser, properties);

        // // After sign in
        // await HttpContext.SignOutAsync(IdentityServerConstants.ExternalCookieAuthenticationScheme);

        // var returnUrl = externalAuthenticationResult.Properties.Items["returnUrl"] ?? "~/";

        // var request = await _interactionService.GetAuthorizationContextAsync(returnUrl);

        // await _eventService.RaiseAsync(
        //     new UserLoginSuccessEvent(
        //         provider,
        //         idClaim.Value,
        //         user.Id.Value,
        //         user.Name.Value,
        //         true,
        //         request?.Client.ClientId));

        // if (request is not null)
        // {
        //     if (request.IsNativeClient())
        //     {
        //         return this.LoadingPage(returnUrl);
        //     }
        // }

        // return Redirect(returnUrl);
        return Page();
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
                "unknown"));

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