using HelloContainer.Domain.Abstractions;
using HelloContainer.DTOs;

namespace HelloContainer.Application
{
    public class AlertService
    {
        private readonly IAlertRepository _alertRepository;

        public AlertService(IAlertRepository alertRepository)
        {
            _alertRepository = alertRepository;
        }

        public async Task<IEnumerable<AlertReadDto>> GetAlertsByContainerId(Guid containerId)
        {
            var alerts = await _alertRepository.FindByContainerIdAsync(containerId);
            return alerts.Select(a => new AlertReadDto(a.Id, a.ContainerId, a.Message, a.CreatedAt));
        }
    }
} 