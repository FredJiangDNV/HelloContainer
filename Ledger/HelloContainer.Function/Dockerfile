# Use the official .NET 8 SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["Ledger/HelloContainer.Function/HelloContainer.Function.csproj", "Ledger/HelloContainer.Function/"]
COPY ["SharedKernel/HelloContainer.SharedKernel.csproj", "SharedKernel/"]

RUN dotnet restore "Ledger/HelloContainer.Function/HelloContainer.Function.csproj"

# Copy the rest of the source code
COPY . .

# Build the application
WORKDIR "/src/Ledger/HelloContainer.Function"
RUN dotnet build "HelloContainer.Function.csproj" -c Release -o /app/build

# Publish the application
FROM build AS publish
RUN dotnet publish "HelloContainer.Function.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the Azure Functions runtime image
FROM mcr.microsoft.com/azure-functions/dotnet-isolated:4-dotnet-isolated8.0 AS final
WORKDIR /home/site/wwwroot

# Create a non-root user for security
RUN adduser --disabled-password --gecos '' appuser && chown -R appuser /home/site/wwwroot
USER appuser

COPY --from=publish /app/publish .

# Environment variables for Azure Functions
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true
