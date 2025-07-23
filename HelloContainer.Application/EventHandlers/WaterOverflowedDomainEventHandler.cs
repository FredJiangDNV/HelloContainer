using HelloContainer.Domain.Abstractions;
using HelloContainer.Domain.AlertAggregate;
using HelloContainer.Domain.ContainerAggregate.Events;
using MediatR;

namespace HelloContainer.Application.EventHandlers
{
    public class WaterOverflowedDomainEventHandler : INotificationHandler<WaterOverflowedDomainEvent>
    {
        private readonly IAlertRepository _alertRepository;
        private readonly IUnitOfWork _unitOfWork;

        public WaterOverflowedDomainEventHandler(IAlertRepository alertRepository, IUnitOfWork unitOfWork)
        {
            _alertRepository = alertRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task Handle(WaterOverflowedDomainEvent @event, CancellationToken cancellationToken)
        {
            var alert = Alert.Create(@event.containerId, $"Container {@event.name} has overflowed with water.");
            _alertRepository.Add(alert);

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
