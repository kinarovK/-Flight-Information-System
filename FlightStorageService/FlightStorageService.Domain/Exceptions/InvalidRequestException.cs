namespace FlightStorageService.Domain.Exceptions;

public class InvalidRequestException : Exception
{
    public InvalidRequestException(string message)
        : base(message)
    {
    }
}
