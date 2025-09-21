#!/bin/bash
set -e

echo "🚀 Building and starting VaultLink with Docker Compose..."

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
docker-compose build

echo "🚀 Starting all services..."
docker-compose up -d

echo "⏳ Waiting for services to be ready..."
sleep 10

# Check service health
echo "🔍 Checking service status..."
docker-compose ps

echo ""
echo "✅ VaultLink is ready!"
echo ""
echo "📱 Frontend:     http://localhost:3000"
echo "🔧 Backend API:  http://localhost:5080"
echo "📡 SFTP Server:  localhost:2222"
echo ""
echo "🔧 To stop all services:"
echo "   docker-compose down"
echo ""
echo "📋 To view logs:"
echo "   docker-compose logs -f [service_name]"