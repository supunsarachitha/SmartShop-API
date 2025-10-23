# -------- Build stage --------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy csproj and restore as distinct layers
COPY SmartShop.API/*.csproj ./SmartShop.API/
RUN dotnet restore ./SmartShop.API/SmartShop.API.csproj

# Copy everything else and publish
COPY SmartShop.API/. ./SmartShop.API/
RUN dotnet publish ./SmartShop.API/SmartShop.API.csproj -c Release -o /app/publish

# -------- Runtime stage --------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Install curl for healthcheck  
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*

# Set ASP.NET Core to listen on port 80
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# Create a non-root user for better security
RUN adduser --disabled-password --gecos "" appuser || true

# Copy published output from build stage
COPY --from=build /app/publish .

# Set ownership to the non-root user
RUN chown -R appuser:appuser /app
USER appuser

# Health check to verify the container is running and serving HTTP
HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 CMD curl -f http://localhost/api/Status || exit 1

# Start the application
ENTRYPOINT ["dotnet", "SmartShop.API.dll"]
