using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MassTransit;
using HelloContainer.Function.Consumers;
using HelloContainer.Function.Data;
using Microsoft.EntityFrameworkCore;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

builder.Services
    .AddApplicationInsightsTelemetryWorkerService()
    .ConfigureFunctionsApplicationInsights();

// Configure DbContext
builder.Services.AddDbContext<LedgerDbContext>(options =>
    options.UseCosmos(
        connectionString: Environment.GetEnvironmentVariable("CosmosDB"),
        databaseName: "HelloContainerDB"
    ));

// Configure MassTransit with RabbitMQ
builder.Services.AddMassTransit(c =>
{
    c.SetKebabCaseEndpointNameFormatter();

    c.AddConsumer<ContainerCreatedConsumer>();
    c.AddConsumer<ContainerDeletedConsumer>();

    c.UsingRabbitMq((context, cfg) =>
    {
        var rabbitMqConnection = Environment.GetEnvironmentVariable("RabbitMQ");
        cfg.Host(rabbitMqConnection);
        cfg.ConfigureEndpoints(context);
    });
});

builder.Build().Run();
