# GrowDate Deployment Guide

## Prerequisites
- Docker and Docker Compose installed
- .NET 8.0 SDK (for local development)

## Deployment Options

### 1. Docker Compose Deployment (Recommended for Production)

Build and run both services:
```bash
docker-compose up -d --build
```

Access the application:
- Frontend: http://localhost:5101
- API: http://localhost:5100
- API Swagger: http://localhost:5100/swagger

Stop the services:
```bash
docker-compose down
```

View logs:
```bash
docker-compose logs -f
```

### 2. Individual Docker Containers

#### Build API
```bash
docker build -f src/GrowDate.Api/Dockerfile -t growdate-api .
docker run -d -p 5100:5100 --name growdate-api growdate-api
```

#### Build Frontend
```bash
docker build -f src/GrowDate.Frontend/Dockerfile -t growdate-frontend .
docker run -d -p 5101:80 --name growdate-frontend growdate-frontend
```

### 3. Cloud Deployment Options

#### Azure App Service

1. **Deploy API:**
```bash
# Login to Azure
az login

# Create resource group
az group create --name growdate-rg --location eastus

# Create App Service plan
az appservice plan create --name growdate-plan --resource-group growdate-rg --sku B1 --is-linux

# Create API web app
az webapp create --resource-group growdate-rg --plan growdate-plan --name growdate-api --runtime "DOTNETCORE:8.0"

# Deploy API
cd src/GrowDate.Api
dotnet publish -c Release
cd bin/Release/net8.0/publish
az webapp deployment source config-zip --resource-group growdate-rg --name growdate-api --src ./publish.zip
```

2. **Deploy Frontend:**
```bash
# Create frontend web app
az webapp create --resource-group growdate-rg --plan growdate-plan --name growdate-frontend --runtime "DOTNETCORE:8.0"

# Build and publish frontend
cd src/GrowDate.Frontend
dotnet publish -c Release

# Deploy to Azure Static Web Apps (alternative)
az staticwebapp create --name growdate-frontend --resource-group growdate-rg --source ./bin/Release/net8.0/publish/wwwroot --location eastus
```

#### Docker Hub + Cloud Platforms

1. **Push to Docker Hub:**
```bash
# Tag images
docker tag growdate-api:latest yourusername/growdate-api:latest
docker tag growdate-frontend:latest yourusername/growdate-frontend:latest

# Push to Docker Hub
docker push yourusername/growdate-api:latest
docker push yourusername/growdate-frontend:latest
```

2. **Deploy to AWS ECS, Google Cloud Run, or DigitalOcean:**
   - Use the pushed Docker images
   - Configure environment variables
   - Set up load balancers and networking

#### Render.com

1. Connect your GitHub repository
2. Create two Web Services:
   - **API Service:** Docker, port 5100
   - **Frontend Service:** Static Site or Docker, port 80

#### Railway.app

1. Connect GitHub repository
2. Deploy both Dockerfiles
3. Configure environment variables
4. Railway will auto-assign URLs

### 4. Environment Configuration

Before deployment, update these configuration files:

#### API CORS Configuration
Edit `src/GrowDate.Api/Program.cs`:
```csharp
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins(
            "http://localhost:5101",
            "https://your-production-frontend-url.com"
        )
        .AllowAnyMethod()
        .AllowAnyHeader();
    });
});
```

#### Frontend API URL
Edit `src/GrowDate.Frontend/wwwroot/appsettings.json`:
```json
{
  "ApiBaseUrl": "https://your-production-api-url.com"
}
```

Or use environment-based configuration in `Program.cs`.

### 5. Production Checklist

- [ ] Update CORS origins for production URLs
- [ ] Update API base URL in frontend configuration
- [ ] Configure HTTPS/SSL certificates
- [ ] Set up database (if moving from SQLite)
- [ ] Configure logging and monitoring
- [ ] Set up CI/CD pipeline
- [ ] Configure environment variables
- [ ] Enable rate limiting and security headers
- [ ] Set up backup strategy
- [ ] Configure domain names and DNS

### 6. CI/CD Pipeline (GitHub Actions)

A sample workflow file is included in `.github/workflows/deploy.yml`

### 7. Database Considerations

Currently using SQLite (file-based). For production:
- Consider PostgreSQL, MySQL, or SQL Server
- Update connection strings in `appsettings.Production.json`
- Run migrations on deployment

### 8. Monitoring and Logs

- Use Application Insights (Azure)
- Configure Serilog or NLog
- Set up health check endpoints
- Monitor API performance and errors

## Support

For issues, see the main README.md or create an issue on GitHub.
