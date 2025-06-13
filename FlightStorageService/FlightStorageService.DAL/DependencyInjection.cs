using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace FlightStorageService.DAL;

public static class DependencyInjection
{
    public static IServiceCollection AddDependecyForDatabase(this IServiceCollection services, IConfiguration config)
    {
        string connectionString = config.GetConnectionString("DefaultCOnnection");
        services.AddScoped<IFlightRepository, FlightRepository>(provider => new FlightRepository(connectionString));
        return services;
    }
}
