using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;
using RichillCapital.SharedKernel.Monads;


namespace RichillCapital.Identity.Web.Pages.Identity;

[AllowAnonymous]
public sealed class SignInViewModel(
    IAuthenticationSchemeProvider _schemeProvider,
    ISignInManager _signInManager,
    IReadOnlyRepository<User> _userRepository,
    IIdentityServerInteractionService _interactionService,
    IEventService _eventService) :
    PageModel
{

    [BindProperty(SupportsGet = true)]
    public string ReturnUrl { get; init; } = string.Empty;

    [BindProperty]
    public string Email { get; init; } = string.Empty;

    [BindProperty]
    public string Password { get; init; } = string.Empty;

    [BindProperty]
    public bool RememberMe { get; init; }

    public IEnumerable<AuthenticationScheme> ExternalSchemes { get; set; } = [];
    public bool AllowRememberMe { get; init; }

    public async Task<IActionResult> OnGetAsync(
        CancellationToken cancellationToken = default)
    {
        await InitializeAsync(cancellationToken);
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        var validationResult = Domain.Users.Email.From(Email);

        if (validationResult.IsFailure)
        {
            await InitializeAsync(cancellationToken);
            return Page();
        }

        var email = validationResult.Value;

        var signInResult = await _signInManager.PasswordSignInAsync(
            email,
            Password,
            isPersistent: AllowRememberMe && RememberMe,
            lockoutOnFailure: true);

        if (signInResult.IsFailure)
        {
            await InitializeAsync(cancellationToken);
            return Page();
        }

        var maybeUser = await _userRepository
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken)
            .ThrowIfNull();

        var user = maybeUser.Value;

        var context = await _interactionService.GetAuthorizationContextAsync(ReturnUrl);

        await _eventService.RaiseAsync(new UserLoginSuccessEvent(
            user.Email.Value,
            user.Id.Value,
            user.Name.Value,
            clientId: context is null || context.Client is null ?
                string.Empty :
                context.Client.ClientId));

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

    private async Task InitializeAsync(CancellationToken _ = default)
    {
        ExternalSchemes = await _schemeProvider.GetExternalSchemesAsync();
    }

    public async Task<IActionResult> OnPostCancelAsync(CancellationToken cancellationToken = default)
    {
        var request = await _interactionService.GetAuthorizationContextAsync(ReturnUrl);

        if (request is null)
        {
            return Redirect("~/");
        }

        // if the user cancels, send a result back into IdentityServer as if they 
        // denied the consent (even if this client does not require consent).
        // this will send back an access denied OIDC error response to the client.
        await _interactionService.DenyAuthorizationAsync(request, AuthorizationError.AccessDenied);

        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
        if (request.IsNativeClient())
        {
            // The client is native, so this change in how to
            // return the response is for better UX for the end user.
            return this.LoadingPage(ReturnUrl);
        }

        return Redirect(ReturnUrl);
    }
}
