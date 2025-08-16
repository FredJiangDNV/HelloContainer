# HelloContainer - Local Docker Development Environment

This project is fully containerized and can easily run all services locally through Docker.

## Local Service Architecture

- **HelloContainer.Api** - .NET 8 Web API backend service (Port: 5000)
- **HelloContainer.Function** - Azure Functions message processing service (Port: 7071)  
- **Web** - React + Vite frontend application (Port: 3000)
- **CosmosDB Emulator** - Local development database (Port: 8081)
- **RabbitMQ** - Message queue service (Port: 5672, Management UI: 15672)

## Quick Start

### Prerequisites

Make sure your system has installed:
- [Docker Desktop](https://www.docker.com/products/docker-desktop/)
- [Docker Compose](https://docs.docker.com/compose/install/) (usually included with Docker Desktop)

### Start All Services

```bash
# Run in project root directory
docker-compose up --build
```

This command will:
1. Build all custom images
2. Download and start CosmosDB emulator and RabbitMQ
3. Start your API, Function, and Web applications

### Access Services

After successful startup, you can access:

- **Frontend Application**: http://localhost:3000
- **API Endpoints**: http://localhost:5000
- **API Documentation (Swagger)**: http://localhost:5000/swagger
- **Azure Function**: http://localhost:7071
- **RabbitMQ Management UI**: http://localhost:15672 (username: guest, password: guest)
- **CosmosDB Data Explorer**: https://localhost:8081/_explorer/index.html

## Development Workflow

### Method 1: Fully Containerized Development

```bash
# Start all services
docker-compose up --build

# Run in background
docker-compose up -d --build

# View logs
docker-compose logs -f

# Stop services
docker-compose down
```

### Method 2: Hybrid Development Mode

If you want to debug certain services in your local IDE, you can run only the infrastructure services:

```bash
# Start only database and message queue
docker-compose up cosmosdb rabbitmq -d

# Then run your applications locally in IDE
```

### View and Manage Services

```bash
# View running containers
docker-compose ps

# View logs for specific service
docker-compose logs api
docker-compose logs function
docker-compose logs web

# Restart specific service
docker-compose restart api

# Enter container for debugging
docker-compose exec api bash
```

### Data Persistence

Your data will be saved in the following locations:
- **CosmosDB data**: `./.containers/cosmosdb/`
- **RabbitMQ data**: `./.containers/queue/rabbitmq/`

To clean all data:
```bash
docker-compose down -v
rm -rf .containers/
```

## Troubleshooting

### Common Issues

1. **Port Conflicts**
   - Ensure ports 3000, 5000, 7071, 5672, 8081, 15672 are not used by other programs
   - You can modify port mappings in docker-compose.yml

2. **CosmosDB Emulator Slow Startup**
   - CosmosDB emulator may take several minutes to start on first run
   - Please be patient, you can check startup progress with `docker-compose logs cosmosdb`

3. **Insufficient Memory**
   - CosmosDB emulator requires significant memory, ensure Docker Desktop has enough memory allocated (recommend 8GB+)

4. **SSL Certificate Issues**
   - CosmosDB emulator uses self-signed certificates, SSL validation is disabled in configuration

### Rebuild Images

If you modify code, you need to rebuild images:

```bash
# Rebuild all images
docker-compose build --no-cache

# Rebuild specific service
docker-compose build --no-cache api
```

### Clean Docker Resources

```bash
# Stop and remove all containers and networks
docker-compose down

# Remove unused images
docker image prune -f

# Remove all unused Docker resources
docker system prune -f
```

## Monitoring and Debugging

### Application Health Checks

- **API Health Check**: http://localhost:5000/health (if configured)
- **RabbitMQ Status**: http://localhost:15672 (management UI)
- **CosmosDB Connection**: Check connection status through API logs

### Log Analysis

```bash
# View all service logs in real-time
docker-compose logs -f

# View recent error logs
docker-compose logs --tail=50 api | grep -i error

# Export logs to file
docker-compose logs > logs.txt
```

## Next Steps

After your local development environment is running properly, you can:

1. **Feature Development**: Develop and test new features in local environment
2. **Integration Testing**: Write end-to-end tests
3. **Performance Testing**: Conduct performance benchmarking in local environment
4. **Prepare Deployment**: Consider cloud configuration when ready to deploy to Azure

## Development Tips

1. **Hot Reload**: After modifying code, rebuild the corresponding service image
2. **Database Operations**: Use CosmosDB Data Explorer to view and manipulate data
3. **Message Debugging**: Monitor message queues through RabbitMQ management UI
4. **Network Debugging**: All services are in the same Docker network and can access each other by service name

Happy coding!