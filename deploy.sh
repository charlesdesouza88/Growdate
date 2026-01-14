#!/bin/bash

# GrowDate Deployment Script

set -e

echo "🌱 GrowDate Deployment Script"
echo "=============================="

# Check if Docker is installed
if ! command -v docker &> /dev/null; then
    echo "❌ Docker is not installed. Please install Docker first."
    exit 1
fi

# Check if Docker Compose is installed
if ! command -v docker-compose &> /dev/null; then
    echo "❌ Docker Compose is not installed. Please install Docker Compose first."
    exit 1
fi

echo ""
echo "1️⃣  Building Docker images..."
docker-compose build

echo ""
echo "2️⃣  Starting services..."
docker-compose up -d

echo ""
echo "3️⃣  Waiting for services to be ready..."
echo "⏳ This may take a moment while health checks run..."
sleep 15

# Check if API is running and healthy
if curl -s -f http://localhost:5100/api/regions > /dev/null; then
    echo "✅ API is running and healthy on http://localhost:5100"
    echo "   Swagger UI: http://localhost:5100/swagger"
else
    echo "⚠️  API health check failed - checking container status..."
    docker-compose logs api --tail 10
fi

# Check if Frontend is running
if curl -s -f http://localhost:5101 > /dev/null; then
    echo "✅ Frontend is running on http://localhost:5101"
else
    echo "⚠️  Frontend may still be starting or has issues..."
    docker-compose logs frontend --tail 10
fi

echo ""
echo "📊 Container Status:"
docker-compose ps

echo ""
echo "✅ Deployment complete!"
echo ""
echo "To view logs: docker-compose logs -f"
echo "To stop: docker-compose down"
echo ""
