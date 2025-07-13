using HelloContainer.Common.IntegrationEvents;
using MassTransit;
using Microsoft.Extensions.Logging;
using HelloContainer.Function.Data;
using HelloContainer.Function.Data.Entities;

namespace HelloContainer.Function.Consumers
{
    public class ContainerDeletedConsumer(ILogger<ContainerDeletedConsumer> logger, LedgerDbContext dbContext) :
        IConsumer<ContainerDeletedIntegrationEvent>
    {
        public async Task Consume(ConsumeContext<ContainerDeletedIntegrationEvent> context)
        {
            logger.LogInformation("Delete container {name}", context.Message.name);
            
            var eventLedger = EventLedger.Create(
                eventType: context.Message.EventType,
                eventData: context.Message
            );
            
            dbContext.EventLedgers.Add(eventLedger);
            await dbContext.SaveChangesAsync();
        }
    }
}
