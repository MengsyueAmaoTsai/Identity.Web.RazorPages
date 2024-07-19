using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

using IdentityModel;

using Microsoft.AspNetCore.Mvc;

using RichillCapital.Domain.Common.Repositories;
using RichillCapital.Domain.Users;
using RichillCapital.SharedKernel.Monads;
using RichillCapital.UseCases.Common;

namespace RichillCapital.Identity.Web.Pages.SignIn.Consent;

public sealed class SignInConsentViewModel(
    ICurrentUser _currentUser,
    IReadOnlyRepository<User> _userRepository,
    IIdentityServerInteractionService _interactionService,
    IEventService _eventService) :
    IdentityViewModel
{
    [BindProperty]
    public required string Description { get; init; }

    [BindProperty]
    public required bool RememberMyDecision { get; init; }

    public required bool Consent { get; init; }

    public required string ClientName { get; set; }
    public required string ClientUrl { get; set; }
    public required string ClientLogoUri { get; set; }
    public required bool AllowRememberConsent { get; set; }

    public required IEnumerable<IdentityResource> IdentityResources { get; set; }

    public required IEnumerable<ApiResource> ApiResources { get; set; }

    public required IEnumerable<ApiScope> ApiScopes { get; set; }

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(ReturnUrl))
        {
            return ErrorPage();
        }

        var context = await _interactionService.GetAuthorizationContextAsync(ReturnUrl);

        if (context is null)
        {
            return ErrorPage();
        }

        ClientName = context.Client.ClientName ?? context.Client.ClientId;
        ClientUrl = context.Client.ClientUri ?? string.Empty;
        ClientLogoUri = context.Client.LogoUri ?? string.Empty;
        AllowRememberConsent = context.Client.AllowRememberConsent;

        IdentityResources = context.ValidatedResources.Resources.IdentityResources;

        var resourceIndicators = context.Parameters.GetValues(OidcConstants.AuthorizeRequest.Resource) ?? [];
        ApiResources = context.ValidatedResources.Resources.ApiResources
            .Where(resource => resourceIndicators.Contains(resource.Name));

        var apiScopes = new List<ApiScope>();

        foreach (var parsedScope in context.ValidatedResources.ParsedScopes)
        {
            var apiScope = context.ValidatedResources.Resources.FindApiScope(parsedScope.ParsedName);

            if (apiScope is not null)
            {
                apiScopes.Add(apiScope);
            }
        }

        ApiScopes = apiScopes;

        return Page();
    }

    public async Task<IActionResult> OnPostAsync(CancellationToken cancellationToken = default)
    {
        var context = await _interactionService.GetAuthorizationContextAsync(ReturnUrl);

        if (context is null)
        {
            return ErrorPage();
        }
        
        var maybeUser = await _userRepository.GetByIdAsync(_currentUser.Id, cancellationToken).ThrowIfNull();
        var user = maybeUser.Value;
        
        throw new NotImplementedException();
    }
}