using FlightStorageService.Domain;

namespace FlightStorageService.Services;

public interface IFlightService
{
    Task AddFlightAsync(Flight flight, CancellationToken cancellationToken);

    Task CleanupOldFlightsAsync(CancellationToken cancellationToken);

    Task<Flight> GetFlightByNumberAsync(string flightNumber, CancellationToken cancellationToken);

    Task<IEnumerable<Flight>> GetFlightsByArrivalCityAndDateAsync(string city, DateTime date, CancellationToken cancellationToken);

    Task<IEnumerable<Flight>> GetFlightsByDateAsync(DateTime date, CancellationToken cancellationToken);

    Task<IEnumerable<Flight>> GetFlightsByDepartureCityAndDateAsync(string city, DateTime date, CancellationToken cancellationToken);

    Task UpdateFlightAsync(Flight flight, CancellationToken cancellationToken);

    Task DeleteFlightAsync(string flightNumber, CancellationToken cancellationToken);
}