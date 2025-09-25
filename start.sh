#!/bin/bash
set -e

echo "🚀 Building and starting VaultLink with Docker Compose..."

# Cleanup function for error handling
cleanup() {
    echo "❌ Error occurred. Cleaning up..."
    docker compose down 2>/dev/null || true
    exit 1
}

# Set trap for error handling
trap cleanup ERR

# Generate SSH keys if not exist
if [ ! -f "./ops/ssh/keys/demo_ed25519" ]; then
    echo "📋 Generating SSH keys..."
    cd ops/scripts
    ./gen-ssh-key.sh
    cd ../..
fi

# Generate GPG keys if not exist
if [ ! -f "./samples/keys/client_pub.asc" ]; then
    echo "🔐 Generating GPG keys..."
    cd ops/scripts
    ./gen-gpg-key.sh
    cd ../..
fi

# Build and start all services
echo "🔨 Building Docker images..."
docker compose build

echo "🚀 Starting all services..."
docker compose up -d

echo "⏳ Waiting for services to be ready..."

# Wait for backend health check
echo "🔍 Waiting for backend to be ready..."
for i in {1..30}; do
    if curl -s http://localhost:5080/health >/dev/null 2>&1; then
        echo "✅ Backend is ready!"
        break
    fi
    if [ $i -eq 30 ]; then
        echo "❌ Backend failed to start within 60 seconds"
        docker compose logs backend
        exit 1
    fi
    sleep 2
done

# Wait for frontend to be ready
echo "🔍 Waiting for frontend to be ready..."
for i in {1..15}; do
    if curl -s http://localhost:3000 >/dev/null 2>&1; then
        echo "✅ Frontend is ready!"
        break
    fi
    if [ $i -eq 15 ]; then
        echo "❌ Frontend failed to start within 30 seconds"
        docker compose logs frontend
        exit 1
    fi
    sleep 2
done

# Check service health
echo "🔍 Checking service status..."
docker compose ps

echo ""
echo "✅ VaultLink is ready!"
echo ""
echo "📱 Frontend:     http://localhost:3000"
echo "🔧 Backend API:  http://localhost:5080"
echo "📡 SFTP Server:  localhost:2222"
echo ""
echo "🔧 To stop all services:"
echo "   docker compose down"
echo ""
echo "📋 To view logs:"
echo "   docker compose logs -f [service_name]"