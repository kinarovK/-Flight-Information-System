using AutoMapper;
using FlightStorageService.Domain;

namespace FlightStorageService.Dtos.MappingProfile
{
    public class FlightProfile : Profile
    {
        public FlightProfile()
        {
            CreateMap<FlightDto, Flight>();
            CreateMap<Flight, FlightDto>();
            CreateMap<UpdateFlightDto, Flight>().ForMember(f => f.FlightNumber, opt => opt.Ignore());
        }
    }
}
