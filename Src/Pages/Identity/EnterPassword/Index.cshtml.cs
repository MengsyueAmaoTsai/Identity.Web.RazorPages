using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;
using RichillCapital.SharedKernel.Monads;

namespace RichillCapital.Identity.Web.Pages.Identity.EnterPassword;

public class EnterPasswordViewModel(
    ISignInManager _signInManager,
    IReadOnlyRepository<User> _userRepository,
    IIdentityServerInteractionService _interactionService,
    IEventService _eventService) : PageModel
{
    [BindProperty(SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    [BindProperty(SupportsGet = true)]
    public required string EmailAddress { get; init; }

    [BindProperty]
    public required string Password { get; init; }

    public IActionResult OnGet() => Page();

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        var email = Email
            .From(EmailAddress)
            .ThrowIfFailure()
            .ValueOrDefault;

        var signInResult = await _signInManager.PasswordSignInAsync(
            email,
            Password,
            isPersistent: true,
            lockoutOnFailure: true);

        if (signInResult.IsFailure)
        {
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
                    RedirectToPage("/profile/index") : throw new Exception("invalid return URL");
        }

        return context.IsNativeClient() ?
            this.LoadingPage(ReturnUrl) :
            Redirect(ReturnUrl);
    }
}
