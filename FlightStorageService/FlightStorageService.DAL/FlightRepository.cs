using FlightStorageService.Domain;
using Microsoft.Data.SqlClient;
using System.Data;

namespace FlightStorageService.DAL;

public class FlightRepository : IFlightRepository
{
    private readonly string _connectionString;

    public FlightRepository(string connectionString)
    {
        _connectionString = connectionString;
    }
    public async Task AddFlightAsync(Flight flight, CancellationToken cancellationToken)
    {

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync(cancellationToken);

            using (SqlCommand cmd = new SqlCommand("AddFlight", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FlightNumber", flight.FlightNumber);
                cmd.Parameters.AddWithValue("@DepartureDateTime", flight.DepartureDateTime);
                cmd.Parameters.AddWithValue("@DepartureAirportCity", flight.DepartureAirportCity);
                cmd.Parameters.AddWithValue("@ArrivalAirportCity", flight.ArrivalAirportCity);
                cmd.Parameters.AddWithValue("@DurationMinutes", flight.DurationMinutes);

                await cmd.ExecuteNonQueryAsync(cancellationToken);
            }
        }
    }

    public async Task CleanupOldFlightsAsync(CancellationToken cancellationToken)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync(cancellationToken);

            using (SqlCommand cmd = new SqlCommand("CleanupOldFlights", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                await cmd.ExecuteNonQueryAsync(cancellationToken);
            }
        }
    }



    public async Task<Flight> GetFlightByNumberAsync(string flightNumber, CancellationToken cancellationToken)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync(cancellationToken);

            using (SqlCommand cmd = new SqlCommand("GetFlightByNumber", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FlightNumber", flightNumber);
                using (SqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken))
                {
                    if (reader.Read())
                    {
                        return new Flight
                        {
                            FlightNumber = reader["FlightNumber"].ToString(),
                            DepartureDateTime = (DateTime)reader["DepartureDateTime"],
                            DepartureAirportCity = reader["DepartureAirportCity"].ToString(),
                            ArrivalAirportCity = reader["ArrivalAirportCity"].ToString(),
                            DurationMinutes = (int)reader["DurationMinutes"]
                        };
                    }
                }

                await cmd.ExecuteNonQueryAsync(cancellationToken);

            }
        }
        return null;
    }

    public async Task<IEnumerable<Flight>> GetFlightsByArrivalCityAndDateAsync(string city, DateTime date, CancellationToken cancellationToken)
    {
        List<Flight> flights = new List<Flight>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync(cancellationToken);

            using (SqlCommand cmd = new SqlCommand("GetFlightsByArrivalCityAndDate", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@City", city);
                cmd.Parameters.AddWithValue("@Date", date);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken))
                {
                    while (reader.Read())
                    {
                        flights.Add(new Flight
                        {
                            FlightNumber = reader["FlightNumber"].ToString(),
                            DepartureDateTime = (DateTime)reader["DepartureDateTime"],
                            DepartureAirportCity = reader["DepartureAirportCity"].ToString(),
                            ArrivalAirportCity = reader["ArrivalAirportCity"].ToString(),
                            DurationMinutes = (int)reader["DurationMinutes"]
                        });
                    }
                }
            }
        }

        return flights;
    }

    public async Task<IEnumerable<Flight>> GetFlightsByDateAsync(DateTime date, CancellationToken cancellationToken)
    {
        List<Flight> flights = new List<Flight>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync(cancellationToken);

            using (SqlCommand cmd = new SqlCommand("GetFlightsByDate", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Date", date);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        flights.Add(new Flight
                        {
                            FlightNumber = reader["FlightNumber"].ToString(),
                            DepartureDateTime = (DateTime)reader["DepartureDateTime"],
                            DepartureAirportCity = reader["DepartureAirportCity"].ToString(),
                            ArrivalAirportCity = reader["ArrivalAirportCity"].ToString(),
                            DurationMinutes = (int)reader["DurationMinutes"]
                        });
                    }
                }
            }
        }

        return flights;
    }

    public async Task<IEnumerable<Flight>> GetFlightsByDepartureCityAndDateAsync(string city, DateTime date, CancellationToken cancellationToken)
    {
        List<Flight> flights = new List<Flight>();

        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync(cancellationToken);

            using (SqlCommand cmd = new SqlCommand("GetFlightsByDepartureCityAndDate", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@City", city);
                cmd.Parameters.AddWithValue("@Date", date);

                using (SqlDataReader reader = await cmd.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync(cancellationToken))
                    {
                        flights.Add(new Flight
                        {
                            FlightNumber = reader["FlightNumber"].ToString(),
                            DepartureDateTime = (DateTime)reader["DepartureDateTime"],
                            DepartureAirportCity = reader["DepartureAirportCity"].ToString(),
                            ArrivalAirportCity = reader["ArrivalAirportCity"].ToString(),
                            DurationMinutes = (int)reader["DurationMinutes"]
                        });
                    }
                }
            }
        }

        return flights;
    }

    public async Task UpdateFlightAsync(Flight flight, CancellationToken cancellationToken)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync(cancellationToken);

                
            using (SqlCommand cmd = new SqlCommand("UpdateFlight", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FlightNumber", flight.FlightNumber);
                cmd.Parameters.AddWithValue("@DepartureDateTime", flight.DepartureDateTime);
                cmd.Parameters.AddWithValue("@DepartureAirportCity", flight.DepartureAirportCity);
                cmd.Parameters.AddWithValue("@ArrivalAirportCity", flight.ArrivalAirportCity);
                cmd.Parameters.AddWithValue("@DurationMinutes", flight.DurationMinutes);

                cmd.ExecuteNonQueryAsync(cancellationToken);
            }

        }

    }

    public async Task DeleteFlightAsync(string flightNumber, CancellationToken cancellationToken)
    {
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync(cancellationToken);

            using (SqlCommand cmd = new SqlCommand("DeleteFlight", conn))
            {
                cmd.CommandType= CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FlightNumber", flightNumber);
                await cmd.ExecuteNonQueryAsync(cancellationToken);
            }
        }
    }
}
