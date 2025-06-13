using FlightStorageService.Domain;
using FlightStorageService.Domain.Exceptions;

namespace FlightStorageService.Services;

public class FlightValidator : IFlightValidator
{
    public void ValidateFlight(Flight flight)
    {
        if (flight == null)
        {
            throw new InvalidRequestException("Flight data must be not null");
        }

        if (string.IsNullOrEmpty(flight.DepartureAirportCity))
        {
            throw new InvalidRequestException("Departure city is required");
        }

        if (string.IsNullOrEmpty(flight.ArrivalAirportCity))
        {
            throw new InvalidRequestException("Arrival city is required");
        }

        if (string.Equals(flight.DepartureAirportCity, flight.ArrivalAirportCity, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidRequestException("The Departure and Arrival cant be the same");
        }

        if (flight.DurationMinutes < 1)
        {
            throw new InvalidRequestException("Flight duration can't be less than 1 minute");
        }

        if (flight.DepartureDateTime == DateTime.MinValue)
        {
            throw new InvalidRequestException("Departure date is required");

        }
    }

    public void ValidateDate(DateTime dateTime)
    {
        if (dateTime == DateTime.MinValue)
        {
            throw new InvalidRequestException("Invalid date, provide another one");
        }
    }
}
