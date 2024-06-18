using Duende.IdentityServer;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Domain;
using RichillCapital.Domain.Common.Identity;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.Pages.Identity;

[AllowAnonymous]
public sealed class SignInViewModel(
    ILogger<SignInViewModel> _logger,
    IAuthenticationSchemeProvider _schemeProvider,
    IUserService _userService,
    ISignInManager _signInManager,
    IIdentityServerInteractionService _interactionService) : PageModel
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

    public async Task<IActionResult> OnPostAsync(
        string action,
        CancellationToken cancellationToken = default)
    {
        var context = await _interactionService.GetAuthorizationContextAsync(ReturnUrl);

        if (action == "Cancel")
        {
            return await HandleCancelAsync(context, cancellationToken);
        }

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

        var maybeUser = await _userService
            .GetByIdAsync(userId, cancellationToken)
            .ThrowIfFailure();

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

    private async Task<IActionResult> HandleCancelAsync(
        AuthorizationRequest? request,
        CancellationToken cancellationToken = default)
    {
        var context = await _interactionService.GetAuthorizationContextAsync(ReturnUrl);

        if (context is null)
        {
            return Redirect("~/");
        }

        ArgumentNullException.ThrowIfNull(ReturnUrl, nameof(ReturnUrl));

        // if the user cancels, send a result back into IdentityServer as if they 
        // denied the consent (even if this client does not require consent).
        // this will send back an access denied OIDC error response to the client.
        await _interactionService.DenyAuthorizationAsync(
            context,
            AuthorizationError.AccessDenied);

        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
        if (context.IsNativeClient())
        {
            // The client is native, so this change in how to
            // return the response is for better UX for the end user.
            return this.LoadingPage(ReturnUrl);
        }

        return Redirect(ReturnUrl ?? "~/");
    }
}
