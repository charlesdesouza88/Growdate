# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files and restore dependencies
COPY ["src/GrowDate.Api/GrowDate.Api.csproj", "GrowDate.Api/"]
COPY ["src/GrowDate.Core/GrowDate.Core.csproj", "GrowDate.Core/"]
COPY ["src/GrowDate.Infrastructure/GrowDate.Infrastructure.csproj", "GrowDate.Infrastructure/"]
RUN dotnet restore "GrowDate.Api/GrowDate.Api.csproj"

# Copy everything else and build
COPY src/GrowDate.Api/ GrowDate.Api/
COPY src/GrowDate.Core/ GrowDate.Core/
COPY src/GrowDate.Infrastructure/ GrowDate.Infrastructure/
WORKDIR /src/GrowDate.Api
RUN dotnet build "GrowDate.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "GrowDate.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
EXPOSE 5100
ENV ASPNETCORE_URLS=http://+:5100
RUN apt-get update && apt-get install -y curl && rm -rf /var/lib/apt/lists/*
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GrowDate.Api.dll"]
