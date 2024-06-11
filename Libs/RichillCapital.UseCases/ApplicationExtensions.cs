using Microsoft.Extensions.DependencyInjection;

namespace RichillCapital.UseCases;

public static class ApplicationExtensions
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services
            .AddMediatR(configuration => configuration
                .RegisterServicesFromAssembly(typeof(ApplicationExtensions).Assembly));

        return services;
    }
}