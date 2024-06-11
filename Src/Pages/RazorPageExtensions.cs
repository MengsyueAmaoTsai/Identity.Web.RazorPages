using Duende.IdentityServer.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace RichillCapital.Identity.Web.Pages;

internal static class RazorPageExtensions
{
    internal static IServiceCollection AddPages(this IServiceCollection services)
    {
        services
            .AddRazorPages()
            .WithRazorPagesRoot("/Src/Pages");

        return services;
    }

    internal static WebApplication MapPages(this WebApplication app)
    {
        app
            .MapRazorPages();

        return app;
    }
}


internal static class IdentityPageExtensions
{
    internal static bool IsNativeClient(this AuthorizationRequest context) =>
        !context.RedirectUri.StartsWith("https", StringComparison.Ordinal) &&
        !context.RedirectUri.StartsWith("http", StringComparison.Ordinal);

    internal static IActionResult LoadingPage(
        this PageModel viewModel,
        string redirectUri)
    {
        viewModel.HttpContext.Response.StatusCode = 200;
        viewModel.HttpContext.Response.Headers["Location"] = "";

        return viewModel.RedirectToPage("/Redirect/Index", new { RedirectUri = redirectUri });
    }

    internal static async Task<bool> GetSchemeSupportsSignOutAsync(this HttpContext context, string scheme)
    {
        var provider = context.RequestServices.GetRequiredService<IAuthenticationHandlerProvider>();
        var handler = await provider.GetHandlerAsync(context, scheme);
        return handler is IAuthenticationSignOutHandler;
    }
}