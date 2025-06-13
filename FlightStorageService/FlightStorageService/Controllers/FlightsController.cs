using AutoMapper;
using FlightStorageService.Domain;
using FlightStorageService.Dtos;
using FlightStorageService.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlightStorageService.Controllers;

[ApiController]
[Route("api/[controller]")]
[TypeFilter(typeof(CustomExceptionFilterAttribute))]
public class FlightsController : ControllerBase
{

    private readonly IFlightService _flightService;
    private readonly IMapper _mapper;

    public FlightsController(IFlightService flightService, IMapper mapper)
    {
        _flightService = flightService;
        _mapper = mapper;
    }
    /// <summary>
    /// Retrieves flight by flight number.
    /// </summary>
    /// <returns>An object of flight</returns>
    /// <response code="200">Returns specific flight.</response>
    /// <response code="404">No flight was found.</response>

    [HttpGet("{flightNumber}")]
    public async Task<IActionResult> GetFlightByFlightNumber(string flightNumber, CancellationToken cancellationToken)
    {
        var flight = await _flightService.GetFlightByNumberAsync(flightNumber, cancellationToken);
        var flightDto = _mapper.Map<FlightDto>(flight);
        return Ok(flightDto);
    }
    /// <summary>
    /// Retrieves all the flights by specific date and departure city.
    /// </summary>
    /// <returns>A list of flights.</returns>
    /// <response code="200">Returns flights by date and departure city.</response>
    /// <response code="404">No flights found.</response>
    [HttpGet("departure")]
    public async Task<IActionResult> GetFlightDepartureByCityAndDate([FromQuery] string city, [FromQuery] DateTime date, CancellationToken cancellationToken)
    {
        var flights = await _flightService.GetFlightsByDepartureCityAndDateAsync(city, date, cancellationToken);
        var flightDto = _mapper.Map<IEnumerable<FlightDto>>(flights);
        return Ok(flightDto);
    }

    /// <summary>
    /// Retrieves all the flights by specific date and arrival city.
    /// </summary>
    /// <returns>A list of flights.</returns>
    /// <response code="200">Returns flights by date and arrival city.</response>
    /// <response code="404">No flights found.</response>
    [HttpGet("arrival")]
    public async Task<IActionResult> GetFlightArrivalByCityAndDate([FromQuery] string city, [FromQuery] DateTime date, CancellationToken cancellationToken)
    {
        var flights = await _flightService.GetFlightsByArrivalCityAndDateAsync(city, date, cancellationToken);
        var flightDto = _mapper.Map<IEnumerable<FlightDto>>(flights);
        return Ok(flightDto);
    }
    /// <summary>
    /// Retrieves all the flights by specific date.
    /// </summary>
    /// <returns>A list of flights.</returns>
    /// <response code="200">Returns flights by date.</response>
    /// <response code="404">No flights found.</response>

    [HttpGet]
    public async Task<IActionResult> GetFlightsByDate([FromQuery] DateTime date, CancellationToken cancellationToken)
    {
        var flights = await _flightService.GetFlightsByDateAsync(date, cancellationToken);
        var flightDto = _mapper.Map<IEnumerable<FlightDto>>(flights);
        return Ok(flightDto);
    }

    /// <summary>
    /// Adds a new Flight.
    /// </summary>
    /// <param name="flight">Flight creation data.</param>
    /// <returns>A created Flight.</returns>
    /// <response code="201">Returns created flight object.</response>
    /// <response code="400">Incorrect request.</response>

    [HttpPost]
    public async Task<IActionResult> AddFlight([FromBody] FlightDto flight, CancellationToken cancellationToken)
    {
        if (flight == null)
        {
            return BadRequest("Flight data is requried");
        }
        var mappedFlight = _mapper.Map<Flight>(flight);
        await _flightService.AddFlightAsync(mappedFlight, cancellationToken);
        return CreatedAtAction(nameof(GetFlightByFlightNumber), new { flightNumber = flight.FlightNumber }, flight);
    }
    /// <summary>
    /// Updates an existing Flight.
    /// </summary>
    /// <param name="flight">Flight update data.</param>
    /// <returns>Success message.</returns>
    /// <response code="204">Updated successfully.</response>
    /// <response code="400">Incorrect request.</response>
    /// <response code="404">No flight found by flight number.</response>

    [HttpPut("{flightNumber}")]
    public async Task<IActionResult> UpdateFlight(string flightNumber, [FromBody] UpdateFlightDto flight, CancellationToken cancellationToken)
    {
        var mappedFlight = _mapper.Map<Flight>(flight);
        mappedFlight.FlightNumber = flightNumber;
        await _flightService.UpdateFlightAsync(mappedFlight, cancellationToken);
        return NoContent();
    }
    /// <summary>
    /// Deletes a Flight by Flight number.
    /// </summary>
    /// <param name="flightNumber">The flight to delete.</param>
    /// <returns>An HTTP NoContent response.</returns>
    /// <response code="204">Deleted successfully.</response>
    /// <response code="404">No platform found by id.</response>
    [HttpDelete("{flightNumber}")]
    public async Task<IActionResult> DeleteFlight(string flightNumber, CancellationToken cancellationToken)
    {
        await _flightService.DeleteFlightAsync(flightNumber, cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Deletes a Flight before actual date.
    /// </summary>
    /// <returns>An HTTP NoContent response.</returns>
    /// <response code="204">Deleted successfully.</response>
    [HttpDelete]
    public async Task<IActionResult> CleanupOldFlights(CancellationToken cancellationToken)
    {
        await _flightService.CleanupOldFlightsAsync(cancellationToken);
        return NoContent();
    }
}
