# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy solution and project files
COPY *.sln ./
COPY Vaultlink.Api/Vaultlink.Api.csproj ./Vaultlink.Api/
COPY Vaultlink.Core/Vaultlink.Core.csproj ./Vaultlink.Core/
COPY Vaultlink.Tests/Vaultlink.Tests.csproj ./Vaultlink.Tests/

# Restore dependencies
RUN dotnet restore

# Copy all source code
COPY . .

# Build and publish
RUN dotnet publish Vaultlink.Api -c Release -o out --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Install curl for health check
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Create keys directory
RUN mkdir -p /app/.keys

# Copy published app
COPY --from=build /app/out .

EXPOSE 8080

ENTRYPOINT ["dotnet", "Vaultlink.Api.dll"]