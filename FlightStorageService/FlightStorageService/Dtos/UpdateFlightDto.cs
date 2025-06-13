using System.ComponentModel.DataAnnotations;

namespace FlightStorageService.Dtos
{
    public class UpdateFlightDto
    {
        [Required]
        public DateTime DepartureDateTime { get; set; }
        [Required, StringLength(100)]
        public string DepartureAirportCity { get; set; }
        [Required, StringLength(100)]
        public string ArrivalAirportCity { get; set; }
        [Required, Range(1, int.MaxValue)]
        public int DurationMinutes { get; set; }
    }
}
