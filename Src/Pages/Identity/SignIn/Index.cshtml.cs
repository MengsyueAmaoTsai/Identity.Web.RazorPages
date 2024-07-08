using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;


namespace RichillCapital.Identity.Web.Pages.Identity.SignIn;

[AllowAnonymous]
public sealed class SignInViewModel(
    IAuthenticationSchemeProvider _schemeProvider,
    IReadOnlyRepository<User> _userRepository,
    IIdentityServerInteractionService _interactionService) :
    PageModel
{

    [BindProperty(SupportsGet = true)]
    public required string ReturnUrl { get; init; } 

    [BindProperty]
    public required string EmailAddress { get; init; } 

    [BindProperty]
    public required string Password { get; init; }

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
        var validationResult = Email.From(EmailAddress);

        if (validationResult.IsFailure)
        {
            await InitializeAsync(cancellationToken);
            return Page();
        }

        var email = validationResult.Value;

        var maybeUser = await _userRepository
            .FirstOrDefaultAsync(user => user.Email == email, cancellationToken);

        if (maybeUser.IsNull)
        {
            await InitializeAsync(cancellationToken);
            return Page();
        }

        var user = maybeUser.Value;

        return RedirectToPage("../enterPassword/index", new
        {
            ReturnUrl,
            EmailAddress = user.Email,
        });
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
