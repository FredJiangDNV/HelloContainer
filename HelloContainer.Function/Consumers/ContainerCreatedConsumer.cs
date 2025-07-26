using MassTransit;
using Microsoft.Extensions.Logging;
using HelloContainer.Function.Data;
using HelloContainer.Function.Data.Entities;
using HelloContainer.SharedKernel.IntegrationEvents;

namespace HelloContainer.Function.Consumers
{
    public class ContainerCreatedConsumer(ILogger<ContainerCreatedConsumer> logger, LedgerDbContext dbContext) :
        IConsumer<ContainerCreatedIntegrationEvent>
    {
        public async Task Consume(ConsumeContext<ContainerCreatedIntegrationEvent> context)
        {
            logger.LogInformation("Create container {name}", context.Message.name);
            
            var eventLedger = EventLedger.Create(
                eventType: context.Message.EventType,
                eventData: context.Message
            );
            
            dbContext.EventLedgers.Add(eventLedger);
            await dbContext.SaveChangesAsync();
            
            logger.LogInformation("Event saved to ledger with ID: {eventId}", eventLedger.Id);
        }
    }
}
