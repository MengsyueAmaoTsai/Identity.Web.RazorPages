using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;

using IdentityModel;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages.Users.Consent;

public sealed class ConsentViewModel(
    IIdentityServerInteractionService _interactionService) :
    PageModel
{
    [BindProperty(Name = "ReturnUrl", SupportsGet = true)]
    public required string ReturnUrl { get; init; }

    public required string ClientId { get; set; }
    public required string ClientName { get; set; }
    public required string ClientUrl { get; set; }
    public required string ClientLogoUrl { get; set; }
    public required bool AllowRememberConsent { get; set; }

    public required IEnumerable<IdentityResourceModel> IdentityResources { get; set; } = [];
    public required IEnumerable<ApiScopeModel> ApiScopes { get; set; } = [];

    public async Task<IActionResult> OnGetAsync(CancellationToken cancellationToken = default)
    {
        var request = await _interactionService.GetAuthorizationContextAsync(ReturnUrl);

        if (request is null)
        {
            return RedirectToPage("/Error");
        }

        // Get client details
        ClientId = request.Client.ClientId;
        ClientName = request.Client.ClientName ?? "ClientName.Empty";
        ClientUrl = request.Client.ClientUri ?? "ClientUrl.Empty";
        ClientLogoUrl = request.Client.LogoUri ?? "ClientLogoUrl.Empty";
        AllowRememberConsent = request.Client.AllowRememberConsent;

        // Get identity resources
        IdentityResources = request.ValidatedResources.Resources.IdentityResources
            .Select(resource => new IdentityResourceModel
            {
                Name = resource.Name,
                DisplayName = resource.DisplayName ?? resource.Name,
                Description = resource.Description ?? string.Empty,
                Emphasize = resource.Emphasize,
                Required = resource.Required,

                IsConsented = resource.Required,
                Value = resource.Name,
            });

        // Get api resources
        var resourceIndicators = request.Parameters.GetValues(OidcConstants.AuthorizeRequest.Resource) ??
                Enumerable.Empty<string>();

        var apiResources = request.ValidatedResources.Resources.ApiResources
            .Where(resource => resourceIndicators.Contains(resource.Name));

        // Get api scopes
        var apiScopeModels = new List<ApiScopeModel>();

        foreach (var parsedScope in request.ValidatedResources.ParsedScopes)
        {
            var apiScope = request.ValidatedResources.Resources
                .FindApiScope(parsedScope.ParsedName);

            if (apiScope is not null)
            {
                var displayName = apiScope.DisplayName ?? apiScope.Name;

                if (!string.IsNullOrWhiteSpace(parsedScope.ParsedParameter))
                {
                    displayName += ':' + parsedScope.ParsedParameter;
                }

                var apiScopeModel = new ApiScopeModel
                {
                    Name = parsedScope.ParsedName,
                    DisplayName = displayName,
                    Description = apiScope.Description ?? string.Empty,
                    Emphasize = apiScope.Emphasize,
                    Required = apiScope.Required,
                    IsConsented = apiScope.Required,
                    Value = parsedScope.RawValue,
                };

                apiScopeModel.Resources = apiResources.Where(resource => resource.Scopes.Contains(parsedScope.ParsedName))
                    .Select(resource => new ResourceModel
                    {
                        Name = resource.Name,
                        DisplayName = resource.DisplayName ?? resource.Name,
                    });

                apiScopeModels.Add(apiScopeModel);
            }
        }

        ApiScopes = apiScopeModels;

        return Page();
    }
}

public sealed record ApiScopeModel
{
    public required string Name { get; init; }
    public required string DisplayName { get; init; }
    public required string Description { get; init; }
    public required string Value { get; init; }
    public required bool Required { get; init; }
    public required bool Emphasize { get; init; }
    public required bool IsConsented { get; init; }
    public IEnumerable<ResourceModel> Resources { get; set; } = [];
}

public sealed record ResourceModel
{
    public required string Name { get; init; }
    public required string DisplayName { get; init; }
}