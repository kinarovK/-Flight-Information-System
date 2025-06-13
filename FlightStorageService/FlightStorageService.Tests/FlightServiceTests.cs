using FlightStorageService.DAL;
using FlightStorageService.Domain;
using FlightStorageService.Domain.Exceptions;
using FlightStorageService.Services;
using Moq;

namespace FlightStorageService.Tests;

public class FlightServiceTests
{
    private readonly Mock<IFlightRepository> _mockRepo;
    private readonly Mock<IFlightValidator> _mockValidator;
    private readonly FlightService _service;

    public FlightServiceTests()
    {
        _mockRepo = new Mock<IFlightRepository>();
        _mockValidator = new Mock<IFlightValidator>();
        _service = new FlightService(_mockRepo.Object, _mockValidator.Object);
    }

    [Fact]
    public async Task AddFlightAsync_CallsRepository()
    {
        // Arrange
        var flight = new Flight { FlightNumber = "FL123" };
        _mockValidator.Setup(v => v.ValidateFlight(flight));
        _mockRepo.Setup(r => r.AddFlightAsync(flight, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

        // Act
        await _service.AddFlightAsync(flight, CancellationToken.None);

        // Assert
        _mockValidator.Verify(v => v.ValidateFlight(flight), Times.Once);
        _mockRepo.Verify(r => r.AddFlightAsync(flight, It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task AddFlightAsync_InvalidFlight_ThrowsException()
    {
        // Arrange
        var flight = new Flight();
        _mockValidator.Setup(v => v.ValidateFlight(flight))
                      .Throws(new InvalidRequestException("Invalid flight"));

        // Act & Assert
        await Assert.ThrowsAsync<InvalidRequestException>(() =>
            _service.AddFlightAsync(flight, CancellationToken.None));
    }
    [Fact]
    public async Task GetFlightByNumberAsync_FlightExists_ReturnsFlight()
    {
        // Arrange
        var expectedFlight = new Flight { FlightNumber = "FL123" };
        _mockRepo.Setup(r => r.GetFlightByNumberAsync("FL123", It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedFlight);

        // Act
        var result = await _service.GetFlightByNumberAsync("FL123", CancellationToken.None);

        // Assert
        Assert.Equal(expectedFlight, result);
    }

    [Fact]
    public async Task GetFlightByNumberAsync_FlightNotFound_ThrowsNotFoundException()
    {
        // Arrange
        _mockRepo.Setup(r => r.GetFlightByNumberAsync("FL999", It.IsAny<CancellationToken>()))
                .ReturnsAsync((Flight)null);

        // Act & Assert
        var ex = await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.GetFlightByNumberAsync("FL999", CancellationToken.None));
        Assert.Contains("FL999", ex.Message);
    }
    [Fact]
    public async Task GetFlightsByDateAsync_ValidDate_ReturnsFlights()
    {
        // Arrange
        var date = DateTime.Now.Date;
        var flights = new List<Flight> { new(), new() };
        _mockValidator.Setup(v => v.ValidateDate(date));
        _mockRepo.Setup(r => r.GetFlightsByDateAsync(date, It.IsAny<CancellationToken>()))
                .ReturnsAsync(flights);

        // Act
        var result = await _service.GetFlightsByDateAsync(date, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count());
        _mockValidator.Verify(v => v.ValidateDate(date), Times.Once);
    }
    [Fact]
    public async Task GetFlightsByDateAsync_NoFlights_ThrowsNotFoundException()
    {
        // Arrange
        var date = DateTime.Now.Date;
        _mockValidator.Setup(v => v.ValidateDate(date));
        _mockRepo.Setup(r => r.GetFlightsByDateAsync(date, It.IsAny<CancellationToken>()))
                .ReturnsAsync([]);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.GetFlightsByDateAsync(date, CancellationToken.None));
    }
    [Fact]
    public async Task GetFlightsByDepartureCityAndDateAsync_ValidParameters_ReturnsFlights()
    {
        // Arrange
        var city = "New York";
        var date = DateTime.Now.Date;
        var flights = new List<Flight> { new() };
        _mockValidator.Setup(v => v.ValidateDate(date));
        _mockRepo.Setup(r => r.GetFlightsByDepartureCityAndDateAsync(city, date, It.IsAny<CancellationToken>()))
                .ReturnsAsync(flights);

        // Act
        var result = await _service.GetFlightsByDepartureCityAndDateAsync(city, date, CancellationToken.None);

        // Assert
        Assert.Single(result);
        _mockValidator.Verify(v => v.ValidateDate(date), Times.Once);
    }
    [Fact]
    public async Task GetFlightsByArrivalCityAndDateAsync_NoFlights_ThrowsNotFoundException()
    {
        // Arrange
        var city = "London";
        var date = DateTime.Now.Date;
        _mockValidator.Setup(v => v.ValidateDate(date));
        _mockRepo.Setup(r => r.GetFlightsByArrivalCityAndDateAsync(city, date, It.IsAny<CancellationToken>()))
                .ReturnsAsync([]);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.GetFlightsByArrivalCityAndDateAsync(city, date, CancellationToken.None));
    }
    [Fact]
    public async Task CleanupOldFlightsAsync_CallsRepository()
    {
        // Arrange
        _mockRepo.Setup(r => r.CleanupOldFlightsAsync(It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

        // Act
        await _service.CleanupOldFlightsAsync(CancellationToken.None);

        // Assert
        _mockRepo.Verify(r => r.CleanupOldFlightsAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
    [Fact]
    public async Task UpdateFlightAsync_ValidFlight_CallsRepository()
    {
        // Arrange
        var flight = new Flight { FlightNumber = "FL123" };
        _mockRepo.Setup(r => r.GetFlightByNumberAsync("FL123", It.IsAny<CancellationToken>()))
                .ReturnsAsync(flight);
        _mockValidator.Setup(v => v.ValidateFlight(flight));
        _mockRepo.Setup(r => r.UpdateFlightAsync(flight, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

        // Act
        await _service.UpdateFlightAsync(flight, CancellationToken.None);

        // Assert
        _mockValidator.Verify(v => v.ValidateFlight(flight), Times.Once);
        _mockRepo.Verify(r => r.UpdateFlightAsync(flight, It.IsAny<CancellationToken>()), Times.Once);
    }
    [Fact]
    public async Task UpdateFlightAsync_FlightNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var flight = new Flight { FlightNumber = "FL999" };
        _mockRepo.Setup(r => r.GetFlightByNumberAsync("FL999", It.IsAny<CancellationToken>()))
                .ReturnsAsync((Flight)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.UpdateFlightAsync(flight, CancellationToken.None));
    }
    [Fact]
    public async Task DeleteFlightAsync_FlightExists_CallsRepository()
    {
        // Arrange
        var flightNumber = "FL123";
        var flight = new Flight { FlightNumber = flightNumber };
        _mockRepo.Setup(r => r.GetFlightByNumberAsync(flightNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync(flight);
        _mockRepo.Setup(r => r.DeleteFlightAsync(flightNumber, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

        // Act
        await _service.DeleteFlightAsync(flightNumber, CancellationToken.None);

        // Assert
        _mockRepo.Verify(r => r.DeleteFlightAsync(flightNumber, It.IsAny<CancellationToken>()), Times.Once);
    }
    [Fact]
    public async Task DeleteFlightAsync_FlightNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var flightNumber = "FL999";
        _mockRepo.Setup(r => r.GetFlightByNumberAsync(flightNumber, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Flight)null);

        // Act & Assert
        await Assert.ThrowsAsync<NotFoundException>(() =>
            _service.DeleteFlightAsync(flightNumber, CancellationToken.None));
    }
}
