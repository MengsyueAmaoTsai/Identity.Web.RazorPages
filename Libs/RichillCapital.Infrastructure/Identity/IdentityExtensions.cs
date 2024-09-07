using Microsoft.Extensions.DependencyInjection;

namespace RichillCapital.Infrastructure.Identity;

public static class IdentityExtensions
{
    public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
    {
        var authenticationBuilder = services.AddAuthentication(options =>
        {
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

        return services;
    }
}