using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using RichillCapital.Domain.Abstractions;
using RichillCapital.Extensions.Options;

namespace RichillCapital.Infrastructure.Identity;

public static class IdentityExtensions
{
    public static IServiceCollection AddCustomIdentity(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(
            typeof(IdentityExtensions).Assembly,
            includeInternalTypes: true);

        services.AddOptionsWithFluentValidation<IdentityOptions>(IdentityOptions.SectionKey);

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
