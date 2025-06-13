using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace FlightStorageService.Services;

[ExcludeFromCodeCoverage]
public static class DependencyInjection
{
    public static IServiceCollection AddDependecyForServices(this IServiceCollection services)
    {

        services.AddScoped<IFlightValidator, FlightValidator>();
        services.AddScoped<IFlightService, FlightService>();
        return services;
    }
}
