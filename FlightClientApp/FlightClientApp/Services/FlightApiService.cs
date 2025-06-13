using FlightClientApp.ApiClient;
using FlightClientApp.Models;

namespace FlightClientApp.Services;

public class FlightApiService : IFlightApiService
{
    private readonly IFlightApiClient _flightApiClient;
    public FlightApiService(IFlightApiClient apiClient)
    {
        _flightApiClient = apiClient;
    }

    public async Task CleanupOldFlightsAsync(CancellationToken cancellationToken)
    {
        await _flightApiClient.CleanupOldFlightsAsync(cancellationToken);
    }

    public async Task CreateFlightAsync(Flight flight, CancellationToken cancellationToken)
    {
        await _flightApiClient.CreateFlightAsync(flight, cancellationToken);
    }

    public async Task DeleteFlightAsync(string flightNumber, CancellationToken cancellationToken)
    {
        await _flightApiClient.DeleteFlightAsync(flightNumber, cancellationToken);
    }

    public async Task<Flight?> GetFlightByNumberAsync(string flightNumber, CancellationToken cancellationToken)
    {
        return await _flightApiClient.GetFlightByNumberAsync(flightNumber, cancellationToken);
    }

    public async Task<List<Flight>?> GetFlightsByArrivalCityAndDateAsync(string city, string date, CancellationToken cancellationToken)
    {
        return await _flightApiClient.GetFlightsByArrivalCityAndDateAsync(city, date, cancellationToken);
    }

    public async Task<List<Flight>?> GetFlightsByDateAsync(string date, CancellationToken cancellationToken)
    {
        return await _flightApiClient.GetFlightsByDateAsync(date, cancellationToken);
    }

    public async Task<List<Flight>?> GetFlightsByDepartureCityAndDateAsync(string city, string date, CancellationToken cancellationToken)
    {
        return await _flightApiClient.GetFlightsByDepartureCityAndDateAsync(city, date, cancellationToken);
    }

    public async Task UpdateFlightAsync(Flight flight, CancellationToken cancellationToken)
    {
        await _flightApiClient.UpdateFlightAsync(flight, cancellationToken);
    }
}
