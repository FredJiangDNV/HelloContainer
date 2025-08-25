using Azure.Messaging.ServiceBus;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using HelloContainer.SharedKernel.IntegrationEvents;
using HelloContainer.Function.Data;
using HelloContainer.Function.Data.Entities;

namespace HelloContainer.Function
{
    public class ContainerQueueFunctions
    {
        private readonly ILogger<ContainerQueueFunctions> _logger;
        private readonly LedgerDbContext _dbContext;

        public ContainerQueueFunctions(ILogger<ContainerQueueFunctions> logger, LedgerDbContext dbContext)
        {
            _logger = logger;
            _dbContext = dbContext;
        }

        [Function("ContainerQueueProcessor")]
        public async Task ProcessMessage(
            [ServiceBusTrigger("container-queue", Connection = "ServiceBus")] ServiceBusReceivedMessage message,
            FunctionContext context)
        {
            var correlationId = message.CorrelationId ?? Guid.NewGuid().ToString();
            var cancellationToken = context.CancellationToken;

            var messageBody = message.Body.ToString();
            var eventType = GetEventType(message, messageBody);
                
            switch (eventType)
            {
                case "ContainerCreated":
                    await HandleContainerCreatedEvent(messageBody, correlationId, cancellationToken);
                    break;
                case "ContainerDeleted":
                    await HandleContainerDeletedEvent(messageBody, correlationId, cancellationToken);
                    break;
                default:
                    _logger.LogWarning("Unknown event type: {EventType}. CorrelationId: {CorrelationId}", eventType, correlationId);
                    break;
            }
        }

        private async Task HandleContainerCreatedEvent(string messageBody, string correlationId, CancellationToken cancellationToken)
        {
            ContainerCreatedIntegrationEvent? integrationEvent = null;
                
            using var document = JsonDocument.Parse(messageBody);
            if (document.RootElement.TryGetProperty("message", out var messageElement))
            {
                var messageJson = messageElement.GetRawText();
                integrationEvent = JsonSerializer.Deserialize<ContainerCreatedIntegrationEvent>(messageJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            var eventLedger = EventLedger.Create(
                eventType: integrationEvent!.EventType,
                eventData: integrationEvent
            );

            _dbContext.EventLedgers.Add(eventLedger);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task HandleContainerDeletedEvent(string messageBody, string correlationId, CancellationToken cancellationToken)
        {
            ContainerDeletedIntegrationEvent? integrationEvent = null;
                
            using var document = JsonDocument.Parse(messageBody);
            if (document.RootElement.TryGetProperty("message", out var messageElement))
            {
                var messageJson = messageElement.GetRawText();
                integrationEvent = JsonSerializer.Deserialize<ContainerDeletedIntegrationEvent>(messageJson, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
            }

            var eventLedger = EventLedger.Create(
                eventType: integrationEvent!.EventType,
                eventData: integrationEvent
            );

            _dbContext.EventLedgers.Add(eventLedger);
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


