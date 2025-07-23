using AutoMapper;
using HelloContainer.DTOs;
using HelloContainer.Domain.ContainerAggregate;

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