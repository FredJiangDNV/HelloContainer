using HelloContainer.Domain.Events;
using MediatR;

namespace HelloContainer.Application.EventHandlers
{
    public class ContainerCreatedDomainEventHandler : INotificationHandler<ContainerCreatedDomainEvent>
    {
        public Task Handle(ContainerCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            Console.WriteLine($"Container created with ID: {notification.containerId}");
            return Task.CompletedTask;
        }
    }
}
