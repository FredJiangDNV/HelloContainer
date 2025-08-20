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
            var scope = _serviceScopeFactory.CreateScope();
            var outboxRepository = scope.ServiceProvider.GetRequiredService<IOutboxRepository>();
            var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
            var outboxIntegrationEvents = await outboxRepository.GetUnprocessedEventsAsync(stoppingToken);
            var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

            foreach (var e in outboxIntegrationEvents)
            {
                // TODO: Hard code namespace need fix
                var eventType = Type.GetType($"HelloContainer.SharedKernel.IntegrationEvents.{e.EventName}, HelloContainer.SharedKernel");
                var integrationEvent = JsonSerializer.Deserialize(e.EventContent, eventType);
                if (integrationEvent != null)
                    await publishEndpoint.Publish(integrationEvent, stoppingToken);
            }

            if (outboxIntegrationEvents.Any())
            {
                foreach (var e in outboxIntegrationEvents)
                    await outboxRepository.MarkAsProcessedAsync(e.Id, stoppingToken);

                await unitOfWork.SaveChangesAsync(stoppingToken);
            }
        }
    }
}
