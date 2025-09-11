using HelloContainer.Application.Mappings;
using HelloContainer.Infrastructure.Repositories;
using HelloContainer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using HelloContainer.Application.EventHandlers;
using HelloContainer.Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using HelloContainer.Domain.Abstractions;
using HelloContainer.Domain.Services;
using HelloContainer.Application;
using MassTransit;
using HelloContainer.Api.Settings;
using Microsoft.Extensions.Options;
using HelloContainer.Api.Services;

namespace HelloContainer.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(policy =>
                {
                    policy
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            services.AddMediatR(x =>
            {
                x.Lifetime = ServiceLifetime.Scoped;
                x.RegisterServicesFromAssemblyContaining<OutboxWriterEventHandler>();
            });

            services.Configure<MessageBrokerSettings>(configuration.GetSection("MessageBroker"));
            services.AddSingleton(sp => sp.GetRequiredService<IOptions<MessageBrokerSettings>>().Value);

            services.AddMassTransit(c =>
            {
                c.SetKebabCaseEndpointNameFormatter();
                //c.UsingRabbitMq((context, cfg) =>
                //{
                //    var settings = context.GetRequiredService<IOptions<MessageBrokerSettings>>().Value;

                //    cfg.Host(settings.Host, h =>
                //    {
                //        h.Username(settings.Username);
                //        h.Password(settings.Password);
                //    });

                //    cfg.ConfigureEndpoints(context);
                //});

                c.UsingAzureServiceBus((context, cfg) =>
                {
                    var connectionString = configuration.GetConnectionString("ServiceBus");
                    cfg.Host(connectionString);
                    cfg.ConfigureEndpoints(context);
                });
            });

            services.AddHostedService<OutboxBackgroundService>();

            services.AddAutoMapper(typeof(ContainerMappingProfile).Assembly);

            // Add FluentValidation
            services.AddFluentValidationAutoValidation()
                .AddValidatorsFromAssemblyContaining<CreateContainerDtoValidator>();

            // Add DbContext
            services.AddDbContext<HelloContainerDbContext>(options =>
                options.UseCosmos(
                    configuration.GetConnectionString("CosmosDB") ?? throw new InvalidOperationException("Connection string 'CosmosDB' not found."),
                    databaseName: configuration.GetSection("DatabaseSettings:DatabaseName").Value ?? throw new InvalidOperationException("Database name not found in configuration.")
                ));

            // Add Services
            services.AddScoped<IContainerRepository, ContainerRepository>();
            services.AddScoped<IAlertRepository, AlertRepository>();
            services.AddScoped<IOutboxRepository, OutboxRepository>();
            services.AddScoped<ContainerService>();
            services.AddScoped<ContainerManager>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<ContainerFactory>();
            services.AddScoped<AlertService>();
            services.AddSingleton<IUserService, UserService>();

            return services;
        }
    }
}
