#!/bin/bash
set -e

echo "🛑 Stopping VaultLink services..."

# Stop and remove all containers
docker compose down

# Optional: Remove volumes (uncomment if needed)
# docker compose down -v

echo "✅ All VaultLink services stopped successfully!"
echo ""
echo "🔄 To restart services:"
echo "   ./start.sh"
echo ""
echo "🧹 To clean up completely (including volumes):"
echo "   docker compose down -v"
echo "   docker system prune -f"