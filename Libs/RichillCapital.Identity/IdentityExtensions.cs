using Duende.IdentityServer;

using FluentValidation;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using RichillCapital.Extensions.Options;
using RichillCapital.Identity.Web.IdentityServer;
using RichillCapital.UseCases.Common;

namespace RichillCapital.Identity;

public static class IdentityExtensions
{
    private const string ErrorPath = "/error";
    private const string SignInPath = "/sign-in";
    private const string SignUpPath = "/sign-up";
    private const string SignOutPath = "/sign-out";
    private const string ConsentPath = "/sign-in/consent";

    private static class UrlParameterNames
    {
        internal const string ErrorId = "errorId";
        internal const string ReturnUrl = "returnUrl";
        internal const string SignOutId = "signOutId";
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

                options.UserInteraction.ErrorUrl = ErrorPath;
                options.UserInteraction.ErrorIdParameter = UrlParameterNames.ErrorId;

                options.UserInteraction.CreateAccountUrl = SignUpPath;
                options.UserInteraction.CreateAccountReturnUrlParameter = UrlParameterNames.ReturnUrl;

                options.UserInteraction.LoginUrl = SignInPath;
                options.UserInteraction.LoginReturnUrlParameter = UrlParameterNames.ReturnUrl;

                options.UserInteraction.ConsentUrl = ConsentPath;
                options.UserInteraction.ConsentReturnUrlParameter = UrlParameterNames.ReturnUrl;

                options.UserInteraction.LogoutUrl = SignOutPath;
                options.UserInteraction.LogoutIdParameter = UrlParameterNames.SignOutId;
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
                options.LoginPath = SignInPath;
            })
            .AddMicrosoftAccount("Microsoft", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.ClientId = identityOptions.External.Microsoft.ClientId;
                options.ClientSecret = identityOptions.External.Microsoft.ClientSecret;
            })
            .AddGoogle("Google", options =>
            {
                options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;
                options.ClientId = identityOptions.External.Google.ClientId;
                options.ClientSecret = identityOptions.External.Google.ClientSecret;
            });

        // Current user context
        services.AddHttpContextAccessor();
        services.AddScoped<ICurrentUser, CurrentWebUser>();

        // Sign in manager
        services.AddScoped<ISignInManager, SignInManager>();

        // User Manager
        services.AddScoped<IUserManager, UserManager>();

        return services;
    }
}
