using HelloContainer.Domain.ContainerAggregate.Events;
using MediatR;

namespace HelloContainer.Application.EventHandlers
{
    public class WaterOverflowedDomainEventEmailHandler : INotificationHandler<WaterOverflowedDomainEvent>
    {
        public async Task Handle(WaterOverflowedDomainEvent @event, CancellationToken cancellationToken)
        {
            Console.WriteLine("Send email");
        }
    }
}
