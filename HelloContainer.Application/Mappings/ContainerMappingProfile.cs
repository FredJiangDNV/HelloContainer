using AutoMapper;
using HelloContainer.Application.DTOs;
using HelloContainer.Domain;

namespace HelloContainer.Application.Mappings
{
    public class ContainerMappingProfile : Profile
    {
        public ContainerMappingProfile()
        {
            CreateMap<Container, ContainerReadDto>();
        }
    }
} 