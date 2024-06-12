using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

using RichillCapital.Domain;
using RichillCapital.Domain.Common.Repositories;
using RichillCapital.SharedKernel;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.Pages.Users.SignIn;

[AllowAnonymous]
public sealed class SignInViewModel(
    IReadOnlyRepository<User> _userRepository,
    IIdentityServerInteractionService _interactionService,
    IOptions<IdentityServer.IdentityServerOptions> _identityServerOptions,
    IEventService _eventService) :
    PageModel
{
    [BindProperty(Name = "ReturnUrl", SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty(Name = "Email")]
    public required string Email { get; init; }

    [BindProperty(Name = "Password")]
    public required string Password { get; init; }

    [BindProperty(Name = "RememberMe")]
    public required bool RememberMe { get; init; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        var emailResult = Domain.Email.From(Email);

        if (emailResult.IsFailure)
        {
            ModelState.AddModelError(emailResult.Error.Code, emailResult.Error.Message);
            return Page();
        }

        var request = await _interactionService.GetAuthorizationContextAsync(ReturnUrl);

        var signInResult = await SignInAsync(
            emailResult.Value,
            Password,
            cancellationToken);

        if (signInResult.IsFailure)
        {
            ModelState.AddModelError(signInResult.Error.Code, signInResult.Error.Message);

            await _eventService.RaiseAsync(new UserLoginFailureEvent(
                Email,
                signInResult.Error.Message,
                clientId: request?.Client.ClientId));

            return Page();
        }

        var user = signInResult.Value;

        await _eventService.RaiseAsync(new UserLoginSuccessEvent(
            user.Name,
            user.Id.Value,
            user.Name,
            clientId: request?.Client.ClientId));

        return request is not null ?
            request.IsNativeClient() ? this.LoadingPage(ReturnUrl) : Redirect(ReturnUrl) :
            Url.IsLocalUrl(ReturnUrl) ?
            Redirect(ReturnUrl) :
            string.IsNullOrEmpty(ReturnUrl) ? Redirect("~/") : throw new Exception("invalid return URL");
    }

    private async Task<Result<User>> SignInAsync(
        Email email,
        string password,
        CancellationToken cancellationToken = default)
    {
        var maybeUsers = await _userRepository.FirstOrDefaultAsync(
            user => user.Email == email,
            cancellationToken);

        if (maybeUsers.IsNull)
        {
            var error = Error.NotFound("Users.NotFound", $"User with email {email} was not found.");

            return error.ToResult<User>();
        }

        var user = maybeUsers.Value;

        if (user.Password != password)
        {
            var error = Error.Unauthorized("Users.InvalidCredentials", "Invalid credentials.");

            return error.ToResult<User>();
        }

        var authenticationProperties = RememberMe ?
            new AuthenticationProperties
            {
                IsPersistent = RememberMe,
                ExpiresUtc = DateTimeOffset.UtcNow.Add(TimeSpan.FromDays(_identityServerOptions.Value.RememberMeDurationDays)),
            } :
            null;

        var identityUser = new IdentityServerUser(user.Id.Value)
        {
            DisplayName = user.Name,
        };

        await HttpContext.SignInAsync(identityUser, authenticationProperties);

        return user
            .ToResult();
    }
}