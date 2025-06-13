using FlightStorageService.Domain;
using FlightStorageService.Domain.Exceptions;
using FlightStorageService.Services;

namespace FlightStorageService.Tests;

public class FlightValidatorTests
{
    private readonly FlightValidator _validator = new();

    [Fact]
    public void ValidateFlight_Throws_WhenFlightIsNull()
    {
        // Arrange
        Flight nullFlight = null;

        // Act & Assert
        var exception = Assert.Throws<InvalidRequestException>(() => _validator.ValidateFlight(nullFlight));
        Assert.Equal("Flight data must be not null", exception.Message);
    }

    [Fact]
    public void ValidateFlight_Throws_WhenDepartureCityIsNullOrEmpty()
    {
        // Arrange
        var flight = new Flight
        {
            DepartureAirportCity = null,
            ArrivalAirportCity = "New York",
            DurationMinutes = 120,
            DepartureDateTime = DateTime.Now.AddDays(1)
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidRequestException>(() => _validator.ValidateFlight(flight));
        Assert.Equal("Departure city is required", exception.Message);
        flight.DepartureAirportCity = string.Empty;
        exception = Assert.Throws<InvalidRequestException>(() => _validator.ValidateFlight(flight));
        Assert.Equal("Departure city is required", exception.Message);
    }

    [Fact]
    public void ValidateFlight_Throws_WhenArrivalCityIsNullOrEmpty()
    {
        // Arrange
        var flight = new Flight
        {
            DepartureAirportCity = "London",
            ArrivalAirportCity = null,
            DurationMinutes = 120,
            DepartureDateTime = DateTime.Now.AddDays(1)
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidRequestException>(() => _validator.ValidateFlight(flight));
        Assert.Equal("Arrival city is required", exception.Message);
        flight.ArrivalAirportCity = string.Empty;
        exception = Assert.Throws<InvalidRequestException>(() => _validator.ValidateFlight(flight));
        Assert.Equal("Arrival city is required", exception.Message);
    }
    [Fact]
    public void ValidateFlight_Throws_WhenCitiesAreSame()
    {
        // Arrange
        var flight = new Flight
        {
            DepartureAirportCity = "Paris",
            ArrivalAirportCity = "paris",
            DurationMinutes = 120,
            DepartureDateTime = DateTime.Now.AddDays(1)
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidRequestException>(() => _validator.ValidateFlight(flight));
        Assert.Equal("The Departure and Arrival cant be the same", exception.Message);
    }
    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-100)]
    public void ValidateFlight_Throws_WhenDurationIsInvalid(int duration)
    {
        // Arrange
        var flight = new Flight
        {
            DepartureAirportCity = "Berlin",
            ArrivalAirportCity = "Rome",
            DurationMinutes = duration,
            DepartureDateTime = DateTime.Now.AddDays(1)
        };

        // Act & Assert
        var exception = Assert.Throws<InvalidRequestException>(() => _validator.ValidateFlight(flight));
        Assert.Equal("Flight duration can't be less than 1 minute", exception.Message);
    }
    [Fact]
    public void ValidateFlight_DoesNotThrow_WhenFlightIsValid()
    {
        // Arrange
        var flight = new Flight
        {
            DepartureAirportCity = "Uzhorod",
            ArrivalAirportCity = "Kharkiv",
            DurationMinutes = 60,
            DepartureDateTime = DateTime.Now.AddDays(1)
        };

        // Act & Assert
        var exception = Record.Exception(() => _validator.ValidateFlight(flight));
        Assert.Null(exception);
    }
    [Fact]
    public void ValidateDate_Throws_WhenDateIsMinValue()
    {
        // Act & Assert
        var exception = Assert.Throws<InvalidRequestException>(() => _validator.ValidateDate(DateTime.MinValue));
        Assert.Equal("Invalid date, provide another one", exception.Message);
    }

    [Theory]
    [InlineData("2030-01-01")]
    [InlineData("2023-12-31")]
    [InlineData("9999-12-31")]
    public void ValidateDate_DoesNotThrow_WhenDateIsValid(string dateString)
    {
        // Arrange
        var validDate = DateTime.Parse(dateString);

        // Act & Assert
        var exception = Record.Exception(() => _validator.ValidateDate(validDate));
        Assert.Null(exception);
    }
}
