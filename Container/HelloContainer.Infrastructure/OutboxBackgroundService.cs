using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using HelloContainer.Domain.Abstractions;
using System.Text.Json;

namespace HelloContainer.Infrastructure
{
    public class OutboxBackgroundService : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<OutboxBackgroundService> _logger;
        private PeriodicTimer? _timer = null!;

        public OutboxBackgroundService(
            IServiceScopeFactory serviceScopeFactory,
            ILogger<OutboxBackgroundService> logger)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _timer = new PeriodicTimer(TimeSpan.FromSeconds(5));
            while (await _timer.WaitForNextTickAsync(stoppingToken))
            {
                await PublishIntegrationEvents(stoppingToken);
            }
        }

        private async Task PublishIntegrationEvents(CancellationToken stoppingToken)
        {
            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var outboxIntegrationEvents = await outboxRepository.GetUnprocessedEventsAsync(stoppingToken);
                var sender = scope.ServiceProvider.GetRequiredService<ISendEndpointProvider>();

                if (!outboxIntegrationEvents.Any())
                {
                    return;
                }

                _logger.LogInformation("Processing {Count} outbox events", outboxIntegrationEvents.Count());

                var processedEvents = new List<Guid>();
                foreach (var e in outboxIntegrationEvents)
                {
                    try
                    {
                        // TODO: Hard code namespace need fix - consider using a registry pattern
                        var eventType = Type.GetType($"HelloContainer.SharedKernel.IntegrationEvents.{e.EventName}, HelloContainer.SharedKernel");
                        if (eventType == null)
                        {
                            _logger.LogError("Could not resolve event type for {EventName}", e.EventName);
                            continue;
                        }

                        var integrationEvent = JsonSerializer.Deserialize(e.EventContent, eventType);
                        if (integrationEvent != null)
                        {
                            var endpoint = await sender.GetSendEndpoint(new Uri("queue:container-events"));
                            await endpoint.Send(integrationEvent, stoppingToken);
                            processedEvents.Add(e.Id);
                            _logger.LogDebug("Successfully published event {EventId} of type {EventType}", e.Id, e.EventName);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to publish event {EventId} of type {EventType}", e.Id, e.EventName);
                        // Continue processing other events, don't mark this one as processed
                    }
                }

                // Only mark successfully processed events as processed
                if (processedEvents.Any())
                {
                    foreach (var eventId in processedEvents)
                        await outboxRepository.MarkAsProcessedAsync(eventId, stoppingToken);

                    await unitOfWork.SaveChangesAsync(stoppingToken);
                    _logger.LogInformation("Successfully processed {Count} outbox events", processedEvents.Count);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing outbox events");
            }
        }
    }
}
