using Application.Dtos.City;
using AutoMapper;
using Domain.Entities;

namespace Application.Mappings;

public class CityProfile : Profile
{
    public CityProfile()
    {
        CreateMap<City, GetByIdCityResponse>();
        CreateMap<City, GetAllCtiyResponse>();

        CreateMap<CreateCityRequest, City>();

        CreateMap<UpdateCityRequest, City>()
            .ForAllMembers(opts =>
                opts.Condition((src, dest, srcMember) => srcMember != null));
    }
}
