#!/bin/bash

echo "Testing HelloContainer Local Services..."
echo ""

# Wait for services to start
echo "Waiting for services to start..."
sleep 10

# Test API service
echo "Testing API service (http://localhost:5000)..."
if curl -s -f http://localhost:5000/api/containers > /dev/null; then
    echo "[PASS] API service is running properly"
else
    echo "[FAIL] API service is not accessible"
fi

# Test frontend service
echo "Testing frontend service (http://localhost:3000)..."
if curl -s -f http://localhost:3000 > /dev/null; then
    echo "[PASS] Frontend service is running properly"
else
    echo "[FAIL] Frontend service is not accessible"
fi

# Test RabbitMQ management UI
echo "Testing RabbitMQ management UI (http://localhost:15672)..."
if curl -s -f http://localhost:15672 > /dev/null; then
    echo "[PASS] RabbitMQ management UI is running properly"
else
    echo "[FAIL] RabbitMQ management UI is not accessible"
fi

# Test CosmosDB emulator
echo "Testing CosmosDB emulator (https://localhost:8081)..."
if curl -s -k -f https://localhost:8081 > /dev/null; then
    echo "[PASS] CosmosDB emulator is running properly"
else
    echo "[FAIL] CosmosDB emulator is not accessible"
fi

echo ""
echo "Testing completed!"
echo ""
echo "If all services are working properly, you can access:"
echo "- Frontend App: http://localhost:3000"
echo "- API Documentation: http://localhost:5000/swagger"
echo "- RabbitMQ Management: http://localhost:15672 (guest/guest)"
