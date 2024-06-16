using FluentValidation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using RichillCapital.Extensions.Options;
using RichillCapital.UseCases.Common;

namespace RichillCapital.Identity;

public static class IdentityExtensions
{
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
        // services
        //     .AddIdentityServer(options =>
        //     {
        //         options.Authentication.CookieAuthenticationScheme = RichillCapitalAuthenticationSchemes.Cookie;

        //         options.IssuerUri = identityOptions.IssuerUri;
        //         options.UserInteraction.LoginUrl = "/users/sign-in";
        //         options.UserInteraction.LoginReturnUrlParameter = "returnUrl";
        //     })
        //     .AddInMemoryClients(InMemoryClients.Default)
        //     .AddInMemoryIdentityResources(InMemoryIdentityResources.Default)
        //     .AddDeveloperSigningCredential();

        services
            .AddAuthentication(options =>
            {
                options.DefaultScheme = RichillCapitalAuthenticationSchemes.Cookie;
            })
            .AddCookie(RichillCapitalAuthenticationSchemes.Cookie, options =>
            {
                var defaultCookieLifetime = TimeSpan.FromHours(8);

                options.LoginPath = "/users/sign-in";
                options.Cookie.Name = RichillCapitalAuthenticationSchemes.Cookie;
                options.ExpireTimeSpan = defaultCookieLifetime;
            })
            .AddMicrosoftAccount("Microsoft", options =>
            {
                options.ClientId = "123";
                options.ClientSecret = "123";
            })
            .AddGoogle("Google", options =>
            {
                options.ClientId = "123";
                options.ClientSecret = "123";
            });

        // Current user context
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentWebUser>();

        return services;
    }
}
