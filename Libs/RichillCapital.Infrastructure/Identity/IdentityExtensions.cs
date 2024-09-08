using Duende.IdentityServer.Models;
using Microsoft.Extensions.DependencyInjection;
using RichillCapital.Domain.Abstractions;

namespace RichillCapital.Infrastructure.Identity;

public static class IdentityExtensions
{
    public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
    {
        var authenticationBuilder = services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = CustomAuthenticationSchemes.CookieDefault;
        });

        authenticationBuilder
            .AddCookie(
                CustomAuthenticationSchemes.CookieDefault,
                options =>
                {
                    options.Cookie.Name = CustomAuthenticationSchemes.CookieDefault;
                    options.LoginPath = "/sign-in";
                    options.ReturnUrlParameter = "returnUrl";
                });

        services.AddHttpContextAccessor();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddScoped<IUserManager, UserManager>();
        services.AddScoped<ISignInManager, SignInManager>();

        return services;
    }
}

internal static class InMemoryClients
{
    public static IEnumerable<Client> GetClients()
    {
        return new List<Client>
        {
            new() {
                ClientId = "client",
                ClientSecrets = { new Secret("secret".Sha256()) },
                AllowedGrantTypes = GrantTypes.ClientCredentials,
                AllowedScopes = { "api1" }
            }
        };
    }
}