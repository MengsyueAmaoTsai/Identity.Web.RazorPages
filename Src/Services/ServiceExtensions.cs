using FluentValidation;

using Microsoft.Extensions.Options;

using RichillCapital.Extensions.Options;

namespace RichillCapital.Identity.Web.Services;

internal static class ServiceExtensions
{
    internal static IServiceCollection AddApiService(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(
            typeof(ServiceExtensions).Assembly,
            includeInternalTypes: true);

        services.AddOptionsWithFluentValidation<ResourceServerOptions>(ResourceServerOptions.SectionKey);

        using var scope = services
            .BuildServiceProvider()
            .CreateScope();

        var resourceServerOptions = scope.ServiceProvider
            .GetRequiredService<IOptions<ResourceServerOptions>>()
            .Value;

        services
            .AddHttpClient<IApiService, ApiService>(client =>
            {
                client.BaseAddress = new Uri(resourceServerOptions.BaseAddress);
                client.Timeout = TimeSpan.FromSeconds(10);
            });

        return services;
    }
}