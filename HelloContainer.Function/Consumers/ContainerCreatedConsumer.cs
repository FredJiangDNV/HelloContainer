using HelloContainer.Common.IntegrationEvents;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace HelloContainer.Function.Consumers
{
    public class ContainerCreatedConsumer(ILogger<ContainerCreatedConsumer> logger) :
        IConsumer<ContainerCreatedIntegrationEvent>
    {
        public Task Consume(ConsumeContext<ContainerCreatedIntegrationEvent> context)
        {
            logger.LogInformation("Create container {name}", context.Message.name);
            return Task.CompletedTask;
        }
    }
}
