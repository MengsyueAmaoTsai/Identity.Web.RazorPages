using IdentityModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RichillCapital.Domain;
using RichillCapital.Domain.Abstractions;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace RichillCapital.Infrastructure.Identity;

internal sealed class SignInManager(
    ILogger<SignInManager> _logger,
    IHttpContextAccessor _httpContextAccessor,
    IAuthenticationSchemeProvider _schemes,
    IUserManager _userManager) :
    ISignInManager
{
    private HttpContext? _context;

    public string AuthenticationScheme { get; set; } = CustomAuthenticationSchemes.CookieDefault;

    public HttpContext Context
    {
        get
        {
            var context = _context ?? _httpContextAccessor?.HttpContext;
            return context is null ?
                throw new InvalidOperationException("HttpContext must not be null.") :
                context;
        }

        set => _context = value;
    }

    public async Task<Result> PasswordSignInAsync(
        Email email,
        string password,
        bool isPersistent,
        bool lockoutOnFailure,
        CancellationToken cancellationToken = default)
    {
        var userResult = await _userManager.FindByEmailAsync(email, cancellationToken);

        if (userResult.IsFailure)
        {
            return Result.Failure(userResult.Error);
        }

        var user = userResult.Value;

        return await PasswordSignInAsync(
            user,
            password,
            isPersistent,
            lockoutOnFailure);
    }

    public async Task<Result> PasswordSignInAsync(
        User user,
        string password,
        bool isPersistent,
        bool lockoutOnFailure,
        CancellationToken cancellationToken = default)
    {
        var signInResult = await CheckPasswordSignInAsync(user, password, lockoutOnFailure);

        if (signInResult.IsFailure)
        {
            return Result.Failure(signInResult.Error);
        }

        return await SignInOrTwoFactorAsync(user, isPersistent);
    }

    public async Task<Result> CheckPasswordSignInAsync(
        User user,
        string password,
        bool lockoutOnFailure,
        CancellationToken cancellationToken = default)
    {
        // var error = await PreSignInCheck(user);
        // if (error != null)
        // {
        //     return error;
        // }

        var checkPasswordResult = _userManager.CheckPassword(user, password);

        if (checkPasswordResult.IsFailure)
        {
            _logger.LogDebug("User failed to provide the correct password.");

            if (_userManager.SupportsUserLockout && lockoutOnFailure)
            {
                var incrementLockoutResult = await _userManager.AccessFailedAsync(user);

                if (incrementLockoutResult.IsFailure)
                {
                    return Result.Failure(incrementLockoutResult.Error);
                }

                if (await _userManager.IsLockedOutAsync(user))
                {
                    return Result.Failure(Error.Unauthorized("User is currently locked out."));
                }
            }

            return Result.Failure(checkPasswordResult.Error);
        }

        // var alwaysLockout = AppContext.TryGetSwitch(
        //     "Microsoft.AspNetCore.Identity.CheckPasswordSignInAlwaysResetLockoutOnSuccess",
        //     out var enabled) && enabled;

        // Reset lockout if not in quirks mode or if TFA is not enabled or client is remembered for TFA
        // if (alwaysLockout || !await IsTwoFactorEnabledAsync(user) || await IsTwoFactorClientRememberedAsync(user))
        // {
        //     var resetLockoutResult = await ResetLockoutWithResult(user);

        //     if (!resetLockoutResult.Succeeded)
        //     {
        //         return SignInResult.Failed;
        //     }
        // }

        return Result.Success;
    }

    public async Task<Result> SignInOrTwoFactorAsync(
        User user,
        bool isPersistent,
        string? loginProvider = null,
        bool bypassTwoFactor = false)
    {
        //if (!bypassTwoFactor && await IsTwoFactorEnabledAsync(user))
        //{
        //    if (!await IsTwoFactorClientRememberedAsync(user))
        //    {
        //        // Allow the two-factor flow to continue later within the same request with or without a TwoFactorUserIdScheme in
        //        // the event that the two-factor code or recovery code has already been provided as is the case for MapIdentityApi.
        //        _twoFactorInfo = new()
        //        {
        //            User = user,
        //            LoginProvider = loginProvider,
        //        };

        //        if (await _schemes.GetSchemeAsync(IdentityConstants.TwoFactorUserIdScheme) != null)
        //        {
        //            // Store the userId for use after two factor check
        //            var userId = await UserManager.GetUserIdAsync(user);
        //            await Context.SignInAsync(IdentityConstants.TwoFactorUserIdScheme, StoreTwoFactorInfo(userId, loginProvider));
        //        }

        //        return SignInResult.TwoFactorRequired;
        //    }
        //}

        //// Cleanup external cookie
        if (loginProvider != null)
        {
            //await Context.SignOutAsync(IdentityConstants.ExternalScheme);
        }

        if (loginProvider is null)
        {
            await SignInWithClaimsAsync(user, isPersistent, [new Claim("amr", "pwd")]);
        }
        else
        {
            await SignInAsync(user, isPersistent, loginProvider);
        }

        return Result.Success;
    }

    public Task SignInAsync(User user, bool isPersistent, string? authenticationMethod = null)
        => SignInAsync(user, new AuthenticationProperties { IsPersistent = isPersistent }, authenticationMethod);

    [SuppressMessage("ApiDesign", "RS0026:Do not add multiple public overloads with optional parameters", Justification = "Required for backwards compatibility")]
    public Task SignInAsync(
        User user,
        AuthenticationProperties authenticationProperties,
        string? authenticationMethod = null)
    {
        IList<Claim> additionalClaims = Array.Empty<Claim>();

        if (authenticationMethod != null)
        {
            additionalClaims = [
                new Claim(ClaimTypes.AuthenticationMethod, authenticationMethod)
            ];
        }

        return SignInWithClaimsAsync(user, authenticationProperties, additionalClaims);
    }

    public Task SignInWithClaimsAsync(User user, bool isPersistent, IEnumerable<Claim> additionalClaims)
        => SignInWithClaimsAsync(user, new AuthenticationProperties { IsPersistent = isPersistent }, additionalClaims);

    public async Task SignInWithClaimsAsync(
        User user,
        AuthenticationProperties? authenticationProperties,
        IEnumerable<Claim> additionalClaims)
    {
        var userPrincipal = await CreateUserPrincipalAsync(user);

        foreach (var claim in additionalClaims)
        {
            userPrincipal.Identities.First().AddClaim(claim);
        }

        await Context.SignInAsync(
            AuthenticationScheme,
            userPrincipal,
            authenticationProperties ?? new AuthenticationProperties());

        Context.User = userPrincipal;
    }

    public Task<ClaimsPrincipal> CreateUserPrincipalAsync(User user)
    {
        var claims = new List<Claim>
        {
            new(JwtClaimTypes.Subject, user.Id.Value),
            new(ClaimTypes.Name, user.Name),
            new(ClaimTypes.Email, user.Email.Value),
        };

        var userPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, AuthenticationScheme));

        return Task.FromResult(userPrincipal);
    }

    public async Task SignOutAsync()
    {
        await Context.SignOutAsync(AuthenticationScheme);

        if (await _schemes.GetSchemeAsync(IdentityConstants.ExternalScheme) is not null)
        {
            await Context.SignOutAsync(IdentityConstants.ExternalScheme);
        }

        if (await _schemes.GetSchemeAsync(IdentityConstants.TwoFactorUserIdScheme) is not null)
        {
            await Context.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);
        }
    }
}