#!/bin/bash

# GrowDate Quick Start Script
echo "🌱 Starting GrowDate Application..."
echo ""

# Check if .NET is installed
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET SDK is not installed. Please install .NET 8.0 SDK first."
    exit 1
fi

echo "✅ .NET SDK found"
echo ""

# Build the solution
echo "🔨 Building solution..."
dotnet build --nologo --verbosity quiet
if [ $? -ne 0 ]; then
    echo "❌ Build failed"
    exit 1
fi
echo "✅ Build successful"
echo ""

# Run tests
echo "🧪 Running tests..."
dotnet test --no-build --nologo --verbosity quiet
if [ $? -ne 0 ]; then
    echo "⚠️  Some tests failed, but continuing..."
else
    echo "✅ All tests passed"
fi
echo ""

echo "🚀 Starting API and Frontend..."
echo ""
echo "API will be available at: http://localhost:5100"
echo "Swagger UI at: http://localhost:5100/swagger"
echo "Frontend will be available at: http://localhost:5101"
echo ""
echo "Press Ctrl+C to stop both services"
echo ""

# Start API in background
cd src/GrowDate.Api
dotnet run --no-build > /dev/null 2>&1 &
API_PID=$!
cd ../..

# Wait a bit for API to start
sleep 3

# Start Frontend
cd src/GrowDate.Frontend
dotnet run --no-build &
FRONTEND_PID=$!
cd ../..

# Function to cleanup on exit
cleanup() {
    echo ""
    echo "🛑 Stopping services..."
    kill $API_PID 2>/dev/null
    kill $FRONTEND_PID 2>/dev/null
    echo "✅ Services stopped"
    exit 0
}

# Trap Ctrl+C
trap cleanup SIGINT SIGTERM

# Wait for processes
wait
