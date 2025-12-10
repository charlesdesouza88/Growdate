# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/GrowDate.Frontend/GrowDate.Frontend.csproj", "GrowDate.Frontend/"]
COPY ["src/GrowDate.Core/GrowDate.Core.csproj", "GrowDate.Core/"]
RUN dotnet restore "GrowDate.Frontend/GrowDate.Frontend.csproj"

# Copy everything else and build
COPY src/GrowDate.Frontend/ GrowDate.Frontend/
COPY src/GrowDate.Core/ GrowDate.Core/
WORKDIR /src/GrowDate.Frontend
RUN dotnet build "GrowDate.Frontend.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "GrowDate.Frontend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage - using nginx to serve the static files
FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY src/GrowDate.Frontend/nginx.conf /etc/nginx/nginx.conf
EXPOSE 80
