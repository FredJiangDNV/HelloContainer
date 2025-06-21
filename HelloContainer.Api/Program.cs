using HelloContainer.Application.Services;
using HelloContainer.Infrastructure;
using HelloContainer.Infrastructure.Repositories;
using HelloContainer.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add AutoMapper
builder.Services.AddAutoMapper(typeof(HelloContainer.Application.Mappings.ContainerMappingProfile));

// Add DbContext
builder.Services.AddDbContext<HelloContainerDbContext>(options =>
    options.UseCosmos(
        builder.Configuration.GetConnectionString("CosmosDB") ?? throw new InvalidOperationException("Connection string 'CosmosDB' not found."),
        databaseName: builder.Configuration.GetSection("DatabaseSettings:DatabaseName").Value ?? throw new InvalidOperationException("Database name not found in configuration.")
    ));

// Add Repositories
builder.Services.AddScoped<IContainerRepository, ContainerRepository>();

// Add Application Services
builder.Services.AddScoped<IContainerService, ContainerService>();

// Add Infrastructure Services
builder.Services.AddScoped<DatabaseInitializer>();

var app = builder.Build();

// Initialize database
using (var scope = app.Services.CreateScope())
{
    var databaseInitializer = scope.ServiceProvider.GetRequiredService<DatabaseInitializer>();
    await databaseInitializer.InitializeAsync();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
