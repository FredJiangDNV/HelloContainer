using AutoMapper;
using HelloContainer.Domain.Abstractions;
using HelloContainer.DTOs;

namespace HelloContainer.Application
{
    public class AlertService
    {
        private readonly IAlertRepository _alertRepository;
        private readonly IMapper _mapper;

        public AlertService(IAlertRepository alertRepository, IMapper mapper)
        {
            _alertRepository = alertRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AlertReadDto>> GetAlertsByContainerId(Guid containerId)
        {
            var alerts = await _alertRepository.FindByContainerIdAsync(containerId);
            return _mapper.Map<IEnumerable<AlertReadDto>>(alerts);
        }
    }
} 