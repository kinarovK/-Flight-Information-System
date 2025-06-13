using FlightStorageService.Domain;

namespace FlightStorageService.Services;

public interface IFlightValidator
{
    public void ValidateFlight(Flight flight);

    public void ValidateDate(DateTime dateTime);
}