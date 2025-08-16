# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["Container/HelloContainer.Api/HelloContainer.Api.csproj", "Container/HelloContainer.Api/"]
COPY ["Container/HelloContainer.Application/HelloContainer.Application.csproj", "Container/HelloContainer.Application/"]
COPY ["Container/HelloContainer.Domain/HelloContainer.Domain.csproj", "Container/HelloContainer.Domain/"]
COPY ["Container/HelloContainer.DTOs/HelloContainer.DTOs.csproj", "Container/HelloContainer.DTOs/"]
COPY ["Container/HelloContainer.Infrastructure/HelloContainer.Infrastructure.csproj", "Container/HelloContainer.Infrastructure/"]
COPY ["SharedKernel/HelloContainer.SharedKernel.csproj", "SharedKernel/"]

RUN dotnet restore "Container/HelloContainer.Api/HelloContainer.Api.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR "/src/Container/HelloContainer.Api"
RUN dotnet build "HelloContainer.Api.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "HelloContainer.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the official .NET 8 runtime image for running
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create a non-root user for security
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /app
USER appuser

COPY --from=publish /app/publish .

# Expose port 80
EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "HelloContainer.Api.dll"]
