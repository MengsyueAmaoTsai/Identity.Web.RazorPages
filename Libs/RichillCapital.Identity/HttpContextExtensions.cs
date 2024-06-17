using Duende.IdentityServer.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace RichillCapital.Identity;

public static class HttpContextExtensions
{
    public static async Task<bool> IsSchemeSupportsSignOutAsync(this HttpContext context, string scheme)
    {
        var provider = context.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
        var handler = await provider.GetHandlerAsync(context, scheme);

        return (handler is IAuthenticationSignOutHandler);
    }
}

public static class AuthorizationRequestExtensions
{
    public static bool IsNativeClient(this AuthorizationRequest context) =>
        !context.RedirectUri.StartsWith("https", StringComparison.Ordinal) && 
        !context.RedirectUri.StartsWith("http", StringComparison.Ordinal);
}