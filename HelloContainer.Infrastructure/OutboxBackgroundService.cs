using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
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
            var dbContext = scope.ServiceProvider.GetRequiredService<HelloContainerDbContext>();
            var outboxIntegrationEvents = await dbContext.OutboxIntegrationEvents.ToListAsync();
            var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

            foreach (var e in outboxIntegrationEvents)
            {
                var eventType = Type.GetType($"HelloContainer.Common.IntegrationEvents.{e.EventName}, HelloContainer.Common");
                var integrationEvent = JsonSerializer.Deserialize(e.EventContent, eventType);
                if (integrationEvent != null)
                {
                    await publishEndpoint.Publish(integrationEvent, stoppingToken);
                }
            }

            if (outboxIntegrationEvents.Any())
            {
                dbContext.RemoveRange(outboxIntegrationEvents);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
