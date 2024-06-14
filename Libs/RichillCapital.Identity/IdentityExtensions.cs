using FluentValidation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using RichillCapital.Extensions.Options;
using RichillCapital.Identity.Web.IdentityServer;

namespace RichillCapital.Identity;

public static class IdentityExtensions
{
    public static IServiceCollection AddIdentityWebIdentity(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(
            typeof(IdentityExtensions).Assembly,
            includeInternalTypes: true);

        services.AddOptionsWithFluentValidation<IdentityOptions>(IdentityOptions.SectionKey);

        using var scope = services.BuildServiceProvider().CreateScope();
        var identityOptions = scope.ServiceProvider.GetRequiredService<IOptions<IdentityOptions>>().Value;

        services
            .AddIdentityServer(options =>
            {
                options.Authentication.CookieAuthenticationScheme = RichillCapitalAuthenticationSchemes.Cookie;

                options.IssuerUri = identityOptions.IssuerUri;
                options.UserInteraction.LoginUrl = "/users/signin";
                options.UserInteraction.LoginReturnUrlParameter = "returnUrl";
            })
            .AddInMemoryClients(InMemoryClients.Default)
            .AddInMemoryIdentityResources(InMemoryIdentityResources.Default)
            .AddDeveloperSigningCredential();

        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = RichillCapitalAuthenticationSchemes.Cookie;
            })
            .AddCookie(RichillCapitalAuthenticationSchemes.Cookie, options =>
            {
                options.LoginPath = "/users/signin";
            });

        return services;
    }
}
