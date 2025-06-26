using AutoMapper;
using HelloContainer.Application.DTOs;
using HelloContainer.Domain;
using HelloContainer.Domain.ValueObjects;

namespace HelloContainer.Application.Mappings
{
    public class ContainerMappingProfile : Profile
    {
        public ContainerMappingProfile()
        {
            CreateMap<Container, ContainerReadDto>()
                .ForMember(dest => dest.Amount, opt => opt.MapFrom(src => src.Amount.Value))
                .ForMember(dest => dest.Capacity, opt => opt.MapFrom(src => src.Capacity.Value));
        }
    }
} 