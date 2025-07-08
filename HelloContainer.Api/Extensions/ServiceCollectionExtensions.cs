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

namespace HelloContainer.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddMediatR(x =>
            {
                x.Lifetime = ServiceLifetime.Scoped;
                x.RegisterServicesFromAssemblyContaining<ContainerCreatedDomainEventHandler>();
            });

            services.AddAutoMapper(typeof(ContainerMappingProfile));

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
            services.AddScoped<ContainerService>();
            services.AddScoped<IContainerManager, ContainerManager>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IContainerFactory, ContainerFactory>();

            return services;
        }
    }
}
