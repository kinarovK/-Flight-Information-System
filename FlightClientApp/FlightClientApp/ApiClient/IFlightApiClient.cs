using FlightClientApp.Models;

namespace FlightClientApp.ApiClient
{
    public interface IFlightApiClient
    {
        Task CleanupOldFlightsAsync(CancellationToken cancellationToken);
        Task CreateFlightAsync(Flight flight, CancellationToken cancellationToken);
        Task DeleteFlightAsync(string flightNumber, CancellationToken cancellationToken);
        Task<Flight?> GetFlightByNumberAsync(string flightNumber, CancellationToken cancellationToken);
        Task<List<Flight>?> GetFlightsByArrivalCityAndDateAsync(string city, string date, CancellationToken cancellationToken);
        Task<List<Flight>?> GetFlightsByDateAsync(string date, CancellationToken cancellationToken);
        Task<List<Flight>?> GetFlightsByDepartureCityAndDateAsync(string city, string date, CancellationToken cancellationToken);
        Task UpdateFlightAsync(Flight flight, CancellationToken cancellationToken);
    }
}