using FlightClientApp.Models;
using FlightClientApp.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace FlightClientApp.Controllers;


public class HomeController : Controller
{

    private readonly IFlightApiService _flightApiService;

    public HomeController(IFlightApiService flightApiClient)
    {
        _flightApiService = flightApiClient;
    }

    public IActionResult Index()
    {
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }
    [HttpGet("Search")]
    public async Task<IActionResult> Search(string flightNumber, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(flightNumber))
        {
            ViewBag.ErrorMessage = "Flight number cannot be empty.";
            return View("Index");
        }

        var flight = await _flightApiService.GetFlightByNumberAsync(flightNumber, cancellationToken);

        if (flight == null)
        {
            ViewBag.ErrorMessage = "Flight not found.";
            return View("Index");
        }

        return View("FlightDetails", flight);
    }

    [HttpGet]
    public async Task<IActionResult> SearchByDate(string date, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(date))
        {
            ViewBag.ErrorMessage = "Date cannot be empty.";
            return View("Index");
        }

        var flights = await _flightApiService.GetFlightsByDateAsync(date, cancellationToken);

        if (flights == null || flights.Count == 0)
        {
            ViewBag.ErrorMessage = "No flights found for the selected date.";
            return View("Index");
        }

        return View("FlightResults", flights);
    }

    [HttpGet]
    public async Task<IActionResult> SearchByDepartureCityAndDate(string city, string date, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            ViewBag.ErrorMessage = "City cannot be empty.";
            return View("Index");
        }

        if (string.IsNullOrWhiteSpace(date))
        {
            ViewBag.ErrorMessage = "Date cannot be empty.";
            return View("Index");
        }

        var flights = await _flightApiService.GetFlightsByDepartureCityAndDateAsync(city, date, cancellationToken);

        if (flights == null || flights.Count == 0)
        {
            ViewBag.ErrorMessage = $"No flights found for the city '{city}' on {date}.";
            return View("Index");
        }

        return View("FlightResults", flights);
    }
    [HttpGet]
    public async Task<IActionResult> SearchByArrivalCityAndDate(string city, string date, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(city))
        {
            ViewBag.ErrorMessage = "City cannot be empty.";
            return View("Index");
        }

        if (string.IsNullOrWhiteSpace(date))
        {
            ViewBag.ErrorMessage = "Date cannot be empty.";
            return View("Index");
        }

        var flights = await _flightApiService.GetFlightsByArrivalCityAndDateAsync(city, date, cancellationToken);

        if (flights == null || flights.Count == 0)
        {
            ViewBag.ErrorMessage = $"No flights found for the city '{city}' on {date}.";
            return View("Index");
        }

        return View("FlightResults", flights);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(string flightNumber, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(flightNumber))
        {
            ViewBag.ErrorMessage = "Flight number cannot be empty.";
            return View("Index");
        }

        try
        {
            await _flightApiService.DeleteFlightAsync(flightNumber, cancellationToken);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = $"An error occurred while deleting the flight: {ex.Message}";
            return View("FlightDetails", await _flightApiService.GetFlightByNumberAsync(flightNumber, cancellationToken));
        }
    }
    [HttpGet]
    public async Task<IActionResult> Update(string flightNumber, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(flightNumber))
        {
            ViewBag.ErrorMessage = "Flight number cannot be empty.";
            return View("Index");
        }

        var flight = await _flightApiService.GetFlightByNumberAsync(flightNumber, cancellationToken);
        if (flight == null)
        {
            ViewBag.ErrorMessage = "Flight not found.";
            return View("Index");
        }

        return View("Update", flight);
    }
    [HttpPost]
    public async Task<IActionResult> Update(Flight flight, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View("Update", flight);
        }

        try
        {
            await _flightApiService.UpdateFlightAsync(flight, cancellationToken);
            return RedirectToAction("Search", new { flightNumber = flight.FlightNumber });
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = $"An error occurred while updating the flight: {ex.Message}";
            return View("Update", flight);
        }
    }
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(Flight flight, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            return View(flight);
        }

        try
        {
            await _flightApiService.CreateFlightAsync(flight, cancellationToken);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ViewBag.ErrorMessage = $"An error occurred while creating the flight: {ex.Message}";
            return View(flight);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Cleanup(CancellationToken cancellationToken)
    {


        await _flightApiService.CleanupOldFlightsAsync(cancellationToken);

        return RedirectToAction("Index");
    }
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}

