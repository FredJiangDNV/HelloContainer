using AutoMapper;
using HelloContainer.Domain.AlertAggregate;
using HelloContainer.DTOs;

namespace HelloContainer.Application.Mappings
{
    public class AlertMappingProfile : Profile
    {
        public AlertMappingProfile()
        {
            CreateMap<Alert, AlertReadDto>();
        }
    }
}
