using FlightStorageService.DAL;
using FlightStorageService.Domain;
using FlightStorageService.Domain.Exceptions;

namespace FlightStorageService.Services;

public class FlightService : IFlightService
{
    private readonly IFlightRepository _flightRepository;
    private readonly IFlightValidator _flightValidator;

    public FlightService(IFlightRepository flightRepository, IFlightValidator flightValidator)
    {
        _flightRepository = flightRepository;
        _flightValidator = flightValidator;
    }
    public async Task AddFlightAsync(Flight flight, CancellationToken cancellationToken)
    {
        _flightValidator.ValidateFlight(flight);
        await _flightRepository.AddFlightAsync(flight, cancellationToken);
    }

    public async Task<Flight> GetFlightByNumberAsync(string flightNumber, CancellationToken cancellationToken)
    {
        var flight = await _flightRepository.GetFlightByNumberAsync(flightNumber, cancellationToken);
        if (flight == null)
        {
            throw new NotFoundException($"Not found flight by number {flightNumber}");
        }
        return flight;
    }

    public async Task<IEnumerable<Flight>> GetFlightsByDateAsync(DateTime date, CancellationToken cancellationToken)
    {
        _flightValidator.ValidateDate(date);
        var flights = await _flightRepository.GetFlightsByDateAsync(date, cancellationToken);
        if (!flights.Any())
        {
            throw new NotFoundException("Not found flight by paramaters");
        }
        return flights;
    }

    public async Task<IEnumerable<Flight>> GetFlightsByDepartureCityAndDateAsync(string city, DateTime date, CancellationToken cancellationToken)
    {
        _flightValidator.ValidateDate(date);
        var flights = await _flightRepository.GetFlightsByDepartureCityAndDateAsync(city, date, cancellationToken);
        if (!flights.Any())
        {
            throw new NotFoundException("Not found flight by paramaters");
        }
        return flights;
    }

    public async Task<IEnumerable<Flight>> GetFlightsByArrivalCityAndDateAsync(string city, DateTime date, CancellationToken cancellationToken)
    {
        _flightValidator.ValidateDate(date);
        var flights = await _flightRepository.GetFlightsByArrivalCityAndDateAsync(city, date, cancellationToken);
        if (!flights.Any())
        {
            throw new NotFoundException("Not found flight by paramaters");
        }
        return flights;
    }

    public async Task CleanupOldFlightsAsync(CancellationToken cancellationToken)
    {
        await _flightRepository.CleanupOldFlightsAsync(cancellationToken);
    }

    public async Task UpdateFlightAsync(Flight flight, CancellationToken cancellationToken) 
    {
        await GetFlightByNumberAsync(flight.FlightNumber, cancellationToken);
        _flightValidator.ValidateFlight(flight);
        await _flightRepository.UpdateFlightAsync(flight, cancellationToken);
    }

    public async Task DeleteFlightAsync(string flightNumber, CancellationToken cancellationToken)
    {
        await GetFlightByNumberAsync(flightNumber, cancellationToken);
        await _flightRepository.DeleteFlightAsync(flightNumber, cancellationToken);
    }
}
