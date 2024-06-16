using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Domain;
using RichillCapital.Domain.Common.Identity;
using RichillCapital.Domain.Common.Repositories;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.Pages.Identity;

[AllowAnonymous]
public sealed class SignInViewModel(
    ILogger<SignInViewModel> _logger,
    IAuthenticationSchemeProvider _schemeProvider,
    ISignInManager _signInManager,
    IReadOnlyRepository<User> _userRepository,
    IIdentityServerInteractionService _interactionService,
    IEventService _eventService) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty]
    public required string Email { get; init; }

    [BindProperty]
    public required string Password { get; init; }

    [BindProperty]
    public required bool RememberMe { get; init; }

    public required IEnumerable<AuthenticationScheme> ExternalSchemes { get; set; } = [];


    public required bool AllowRememberMe { get; init; } = false;

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        await InitializeAsync(cancellationToken);

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        var validationResult = Domain.Email.From(Email);

        if (validationResult.IsFailure)
        {
            ModelState.AddModelError(validationResult.Error.Code, validationResult.Error.Message);
            _logger.LogWarning("Validation failed. {error}", validationResult.Error);

            await InitializeAsync(cancellationToken);
            return Page();
        }

        var email = validationResult.Value;

        var signInResult = await _signInManager.PasswordSignInAsync(email, Password, RememberMe, lockoutOnFailure: false);

        if (signInResult.IsFailure)
        {
            ModelState.AddModelError(signInResult.Error.Code, signInResult.Error.Message);
            _logger.LogWarning("Sign in failed. {error}", signInResult.Error);

            await InitializeAsync(cancellationToken);
            return Page();
        }

        var userId = signInResult.Value;

        var maybeUser = await _userRepository.GetByIdAsync(userId, cancellationToken).ThrowIfNull();

        var user = maybeUser.Value;

        var properties = AllowRememberMe && RememberMe ? 
            new AuthenticationProperties
            {
                IsPersistent = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30),
            } : 
            new AuthenticationProperties();

        var identityServerUser = new IdentityServerUser(user.Id.Value)
        {
            DisplayName = user.Name.Value,
        };
        await HttpContext.SignInAsync(identityServerUser, properties);

        //var claims = new List<Claim>
        //{
        //    new Claim("sub", user.Id.Value),
        //    new Claim("name", user.Name.Value),
        //    new Claim("email", user.Email.Value),
        //};
        //var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "idsrv"));
        //await HttpContext.SignInAsync(principal, properties);

        var context = await _interactionService.GetAuthorizationContextAsync(ReturnUrl);

        if (context is null)
        {
            return Url.IsLocalUrl(ReturnUrl) ? 
                Redirect(ReturnUrl) : string.IsNullOrEmpty(ReturnUrl) ? 
                    Redirect("~/") : throw new Exception("invalid return URL");
        }

        return context.IsNativeClient() ? 
            this.LoadingPage(ReturnUrl) : 
            Redirect(ReturnUrl);
    }

    private async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        ExternalSchemes = await _schemeProvider.GetExternalSchemesAsync();
    }
}

public static class Extensions
{
    /// <summary>
    /// Determines if the authentication scheme support signout.
    /// </summary>
    public static async Task<bool> GetSchemeSupportsSignOutAsync(this HttpContext context, string scheme)
    {
        var provider = context.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
        var handler = await provider.GetHandlerAsync(context, scheme);
        return (handler is IAuthenticationSignOutHandler);
    }

    /// <summary>
    /// Checks if the redirect URI is for a native client.
    /// </summary>
    public static bool IsNativeClient(this AuthorizationRequest context)
    {
        return !context.RedirectUri.StartsWith("https", StringComparison.Ordinal)
               && !context.RedirectUri.StartsWith("http", StringComparison.Ordinal);
    }

    /// <summary>
    /// Renders a loading page that is used to redirect back to the redirectUri.
    /// </summary>
    public static IActionResult LoadingPage(this PageModel page, string redirectUri)
    {
        page.HttpContext.Response.StatusCode = 200;
        page.HttpContext.Response.Headers["Location"] = "";

        return page.RedirectToPage("/Redirect/Index", new { RedirectUri = redirectUri });
    }
}