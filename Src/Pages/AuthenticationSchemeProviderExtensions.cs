using Microsoft.AspNetCore.Authentication;

namespace RichillCapital.Identity.Web.Pages;

internal static class AuthenticationSchemeProviderExtensions
{
    internal static async Task<IEnumerable<AuthenticationScheme>> GetExternalSchemesAsync(
        this IAuthenticationSchemeProvider provider) =>
        (await provider.GetAllSchemesAsync())
            .Where(scheme => !string.IsNullOrEmpty(scheme.DisplayName));
}