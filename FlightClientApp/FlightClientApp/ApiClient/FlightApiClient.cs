using FlightClientApp.Models;

namespace FlightClientApp.ApiClient;

public class FlightApiClient : IFlightApiClient
{
    private readonly HttpClient _httpClient;

    public FlightApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<Flight?> GetFlightByNumberAsync(string flightNumber, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"Flights/{flightNumber}", cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<Flight>(cancellationToken);
        }

        return null;
    }
    public async Task<List<Flight>?> GetFlightsByDateAsync(string date, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"flights?date={date}", cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<Flight>>(cancellationToken);
        }

        return null;
    }
    public async Task<List<Flight>?> GetFlightsByDepartureCityAndDateAsync(string city, string date, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"flights/departure?city={city}&date={date}", cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<Flight>>(cancellationToken);
        }

        return null;
    }
    public async Task<List<Flight>?> GetFlightsByArrivalCityAndDateAsync(string city, string date, CancellationToken cancellationToken)
    {
        var response = await _httpClient.GetAsync($"flights/arrival?city={city}&date={date}", cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<List<Flight>>(cancellationToken);
        }

        return null;
    }
    public async Task UpdateFlightAsync(Flight flight, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PutAsJsonAsync($"Flights/{flight.FlightNumber}", flight, cancellationToken);
        response.EnsureSuccessStatusCode();
    }
    public async Task DeleteFlightAsync(string flightNumber, CancellationToken cancellationToken)
    {
        var response = await _httpClient.DeleteAsync($"Flights/{flightNumber}", cancellationToken);
        response.EnsureSuccessStatusCode();
    }
    public async Task CreateFlightAsync(Flight flight, CancellationToken cancellationToken)
    {
        var response = await _httpClient.PostAsJsonAsync("flights", flight, cancellationToken);
        if (!response.IsSuccessStatusCode)
        {
            var errorMessage = await response.Content.ReadAsStringAsync(cancellationToken);
            throw new Exception($"Error: {errorMessage}");
        }

    }

    public async Task CleanupOldFlightsAsync(CancellationToken cancellationToken)
    {
        var response = await _httpClient.DeleteAsync("flights", cancellationToken);
        response.EnsureSuccessStatusCode();
    }
}
