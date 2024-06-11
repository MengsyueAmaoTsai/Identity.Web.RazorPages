namespace RichillCapital.Identity.Web.Services;

internal static class ServiceExtensions
{
    internal static IServiceCollection AddApiService(this IServiceCollection services)
    {
        services
            .AddHttpClient<IApiService, ApiService>(client =>
            {
                client.BaseAddress = new Uri("https://localhost:10000");
                client.Timeout = TimeSpan.FromSeconds(10);
            });

        return services;
    }
}