using FluentValidation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using RichillCapital.Extensions.Options;
using RichillCapital.Identity.Web.IdentityServer;
using RichillCapital.UseCases.Common;

namespace RichillCapital.Identity;

public static class IdentityExtensions
{
    private static class UrlParameterNames
    {
        internal const string ReturnUrl = "returnUrl";
    }

    public static IServiceCollection AddIdentityWebIdentity(this IServiceCollection services)
    {
        // Register options validator
        services.AddValidatorsFromAssembly(
            typeof(IdentityExtensions).Assembly,
            includeInternalTypes: true);

        // Register identity options
        services.AddOptionsWithFluentValidation<IdentityOptions>(IdentityOptions.SectionKey);

        // Get options and configure Identity
        using var scope = services.BuildServiceProvider().CreateScope();
        var identityOptions = scope.ServiceProvider.GetRequiredService<IOptions<IdentityOptions>>().Value;

        // Authentication
        services
            .AddIdentityServer(options =>
            {
                options.IssuerUri = identityOptions.IssuerUri;
                options.UserInteraction.LoginUrl = "/identity/sign-in";
            })
            .AddInMemoryClients(InMemoryClients.Default)
            .AddInMemoryIdentityResources(InMemoryIdentityResources.Default)
            .AddInMemoryApiResources(InMemoryApiResources.Default)
            .AddDeveloperSigningCredential();

        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = RichillCapitalAuthenticationSchemes.DefaultCookieScheme;
            })
            .AddCookie(RichillCapitalAuthenticationSchemes.DefaultCookieScheme, options =>
            {
                options.Cookie.Name = RichillCapitalAuthenticationSchemes.DefaultCookieScheme;
            })
            .AddMicrosoftAccount("Microsoft", options =>
            {
                options.ClientId = identityOptions.External.Microsoft.ClientId;
                options.ClientSecret = identityOptions.External.Microsoft.ClientSecret;
            })
            .AddGoogle("Google", options =>
            {
                options.ClientId = identityOptions.External.Google.ClientId;
                options.ClientSecret = identityOptions.External.Google.ClientSecret;
            });

        // Current user context
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentWebUser>();

        return services;
    }
}
