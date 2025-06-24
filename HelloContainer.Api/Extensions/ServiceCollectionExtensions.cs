using HelloContainer.Application.Mappings;
using HelloContainer.Application.Services;
using HelloContainer.Infrastructure.Repositories;
using HelloContainer.Infrastructure.Services;
using HelloContainer.Infrastructure;
using Microsoft.EntityFrameworkCore;
using HelloContainer.Infrastructure.Common;

namespace HelloContainer.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddServices(this IServiceCollection services, IConfiguration configuration, IWebHostEnvironment environment)
        {
            services.AddAutoMapper(typeof(ContainerMappingProfile));
            services.AddDbContext<HelloContainerDbContext>(options =>
                options.UseCosmos(
                    configuration.GetConnectionString("CosmosDB") ?? throw new InvalidOperationException("Connection string 'CosmosDB' not found."),
                    databaseName: configuration.GetSection("DatabaseSettings:DatabaseName").Value ?? throw new InvalidOperationException("Database name not found in configuration.")
                ));
            services.AddScoped<IContainerRepository, ContainerRepository>();
            services.AddScoped<IContainerService, ContainerService>();
            services.AddScoped<DatabaseInitializer>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}
