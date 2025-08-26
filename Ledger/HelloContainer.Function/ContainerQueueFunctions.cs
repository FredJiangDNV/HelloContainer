using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using System.Text.Json;
using HelloContainer.SharedKernel.IntegrationEvents;
using HelloContainer.Function.Data;
using HelloContainer.Function.Data.Entities;
using HelloContainer.SharedKernel;

namespace HelloContainer.Function
{
    public class ContainerQueueFunctions
    {
        private readonly LedgerDbContext _dbContext;

        public ContainerQueueFunctions(LedgerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        [Function("ContainerQueueProcessor")]
        public async Task ProcessMessage(
            [ServiceBusTrigger("container-events", Connection = "ServiceBus")] ServiceBusReceivedMessage message,
            FunctionContext context)
        {
            var cancellationToken = context.CancellationToken;

            var messageBody = message.Body.ToString();
            var eventType = GetEventType(message, messageBody);
                
            switch (eventType)
            {
                case "ContainerCreated":
                    await HandleEvent<ContainerCreatedIntegrationEvent>(messageBody, cancellationToken);
                    break;
                case "ContainerDeleted":
                    await HandleEvent<ContainerDeletedIntegrationEvent>(messageBody, cancellationToken);
                    break;
            }
        }

        private async Task HandleEvent<T>(string messageBody, CancellationToken cancellationToken)
            where T : IIntegrationEvent
        {
            T? @event = default(T);
                
            using var document = JsonDocument.Parse(messageBody);
            if (document.RootElement.TryGetProperty("message", out var messageElement))
            {
                var messageJson = messageElement.GetRawText();
                @event = JsonSerializer.Deserialize<T>(messageJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            var ledger = EventLedger.Create(
                eventType: @event!.EventType,
                eventData: @event
            );

            _dbContext.EventLedgers.Add(ledger);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private string GetEventType(ServiceBusReceivedMessage message, string messageBody)
        {
            using var document = JsonDocument.Parse(messageBody);
            if (document.RootElement.TryGetProperty("message", out var messageElement))
            {
                if (messageElement.TryGetProperty("eventType", out var eventTypeElement))
                {
                    return eventTypeElement.GetString() ?? string.Empty;
                }
            }

            return string.Empty;
        }
    }
}


