using Duende.IdentityServer.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace RichillCapital.Identity.Web.Pages.Identity;

[AllowAnonymous]
public sealed class SignInViewModel(
    ISignInManager _signInManager) :
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
        // var context = await _interactionService.GetAuthorizationContextAsync(ReturnUrl);

        // if (action == "Cancel")
        // {
        //     return await HandleCancelAsync(context, cancellationToken);
        // }

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
            isPersistent: RememberMe,
            lockoutOnFailure: true);

        if (signInResult.IsFailure)
        {
            await InitializeAsync(cancellationToken);
            return Page();
        }

        // var userId = signInResult.Value;

        // var maybeUser = await _userService
        //     .GetByIdAsync(userId, cancellationToken)
        //     .ThrowIfFailure();

        // var user = maybeUser.Value;

        // var properties = AllowRememberMe && RememberMe ?
        //     new AuthenticationProperties
        //     {
        //         IsPersistent = true,
        //         ExpiresUtc = DateTimeOffset.UtcNow.AddDays(30),
        //     } :
        //     new AuthenticationProperties();

        // var identityServerUser = new IdentityServerUser(user.Id.Value)
        // {
        //     DisplayName = user.Name.Value,
        // };

        // await HttpContext.SignInAsync(identityServerUser, properties);

        //var claims = new List<Claim>
        //{
        //    new Claim("sub", user.Id.Value),
        //    new Claim("name", user.Name.Value),
        //    new Claim("email", user.Email.Value),
        //};
        //var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "idsrv"));
        //await HttpContext.SignInAsync(principal, properties);


        // if (context is null)
        // {
        //     return Url.IsLocalUrl(ReturnUrl) ?
        //         Redirect(ReturnUrl) : string.IsNullOrEmpty(ReturnUrl) ?
        //             Redirect("~/") : throw new Exception("invalid return URL");
        // }

        // return context.IsNativeClient() ?
        //     this.LoadingPage(ReturnUrl) :
        //     Redirect(ReturnUrl);
        return Page();
    }

    private async Task InitializeAsync(CancellationToken _ = default)
    {
        // ExternalSchemes = await _schemeProvider.GetExternalSchemesAsync();
    }

    private async Task<IActionResult> HandleCancelAsync(
        AuthorizationRequest? request,
        CancellationToken cancellationToken = default)
    {
        // if (request is null)
        // {
        //     return Redirect("~/");
        // }

        // ArgumentNullException.ThrowIfNull(ReturnUrl, nameof(ReturnUrl));

        // if the user cancels, send a result back into IdentityServer as if they 
        // denied the consent (even if this client does not require consent).
        // this will send back an access denied OIDC error response to the client.
        // await _interactionService.DenyAuthorizationAsync(
        //     request,
        //     AuthorizationError.AccessDenied);

        // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
        // if (request.IsNativeClient())
        // {
        //     // The client is native, so this change in how to
        //     // return the response is for better UX for the end user.
        //     return this.LoadingPage(ReturnUrl);
        // }

        return Redirect(ReturnUrl ?? "~/");
    }
}
