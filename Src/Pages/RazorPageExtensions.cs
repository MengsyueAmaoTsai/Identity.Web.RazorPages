namespace RichillCapital.Identity.Web.Pages;

internal static class RazorPageExtensions
{
    internal static IServiceCollection AddPages(this IServiceCollection services)
    {
        services
            .AddRazorPages(options =>
            {
            })
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

