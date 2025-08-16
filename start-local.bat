@echo off
echo Starting HelloContainer Local Development Environment...
echo.

REM Check if Docker is running
docker --version >nul 2>&1
if %ERRORLEVEL% NEQ 0 (
    echo Error: Docker is not installed or not running
    echo Please install and start Docker Desktop first
    pause
    exit /b 1
)

echo Docker detected
echo.

echo Building and starting all services...
echo This may take a few minutes, especially on first run...
echo.

docker-compose up --build

echo.
echo Services started successfully!
echo.
echo Access URLs:
echo - Frontend App: http://localhost:3000
echo - API Endpoints: http://localhost:5000
echo - API Documentation: http://localhost:5000/swagger  
echo - Azure Function: http://localhost:7071
echo - RabbitMQ Management: http://localhost:15672 (guest/guest)
echo - CosmosDB Explorer: https://localhost:8081/_explorer/index.html
echo.
pause
