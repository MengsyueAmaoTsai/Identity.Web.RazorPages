using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Services;

using IdentityModel;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Common;

namespace RichillCapital.Identity.Web.Pages.SignOut;

[Authorize]
public sealed class SignOutViewModel(
    IIdentityServerInteractionService _interactionService,
    IEventService _eventService,
    ICurrentUser _currentUser,
    IReadOnlyRepository<User> _userRepository) :
    ViewModel
{
    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        var signOutId = await _interactionService.CreateLogoutContextAsync() ?? string.Empty;

        if (!_currentUser.IsAuthenticated)
        {
            return SignedOutPage(signOutId);
        }

        var maybeUser = await _userRepository.GetByIdAsync(_currentUser.Id, cancellationToken).ThrowIfNull();

        await HttpContext.SignOutAsync();

        var scheme = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
        var user = maybeUser.Value;

        await _eventService.RaiseAsync(new UserLogoutSuccessEvent(user.Id.Value, user.Name.Value));

        if (scheme is not null &&
            scheme != IdentityServerConstants.LocalIdentityProvider)
        {
            if (await HttpContext.GetSchemeSupportsSignOutAsync(scheme))
            {
                var url = Url.Page("/signOut/signedOut", new
                {
                    SignOutId = signOutId,
                });

                var authenticationProperties = new AuthenticationProperties
                {
                    RedirectUri = url,
                };

                return SignOut(authenticationProperties, scheme);
            }
        }

        return SignedOutPage(signOutId);
    }

    private IActionResult SignedOutPage(string signOutId) => 
        RedirectToPage(
            "/signOut/signedOut/index",
            new 
            {
                SignOutId = signOutId,
            });
}

internal static class HttpContextExtensions
{
    internal static async Task<bool> GetSchemeSupportsSignOutAsync(
        this HttpContext context, 
        string scheme)
    {
        var provider = context.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
        var handler = await provider.GetHandlerAsync(context, scheme);

        return (handler is IAuthenticationSignOutHandler);
    }
}