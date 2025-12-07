# GrowDate - Development Notes

## Quick Start Commands

### Build Everything
```bash
dotnet build
```

### Run API (Terminal 1)
```bash
cd src/GrowDate.Api
dotnet run
# API runs at https://localhost:7000
# Swagger at https://localhost:7000/swagger
```

### Run Frontend (Terminal 2)
```bash
cd src/GrowDate.Frontend
dotnet run
# Frontend runs at https://localhost:7001 (or similar)
```

### Run Tests
```bash
cd src/GrowDate.Tests
dotnet test --verbosity normal
```

## Project Structure

```
GrowDate/
├── src/
│   ├── GrowDate.Core/              # Business logic layer
│   │   ├── Entities/               # Domain models
│   │   ├── Interfaces/             # Repository contracts
│   │   ├── Services/               # Business services
│   │   └── DTOs/                   # Data transfer objects
│   │
│   ├── GrowDate.Infrastructure/    # Data access layer
│   │   ├── Data/                   # DbContext & seeder
│   │   └── Repositories/           # Repository implementations
│   │
│   ├── GrowDate.Api/               # Web API layer
│   │   ├── Controllers/            # API endpoints
│   │   ├── Program.cs              # API startup
│   │   └── appsettings.json        # Configuration
│   │
│   ├── GrowDate.Frontend/          # Blazor WebAssembly
│   │   ├── Pages/                  # Razor pages
│   │   ├── Shared/                 # Shared components
│   │   ├── Services/               # Frontend services
│   │   └── wwwroot/                # Static files
│   │
│   └── GrowDate.Tests/             # Unit tests
│       ├── Services/               # Service tests
│       └── Repositories/           # Repository tests
│
├── GrowDate.sln
└── README.md
```

## Database

- **Type**: SQLite
- **Location**: `src/GrowDate.Api/growdate.db`
- **Auto-created**: Yes, on first run
- **Seeded**: Yes, automatically with sample data

### Reset Database
```bash
# Delete database file
rm src/GrowDate.Api/growdate.db

# Restart API - database will be recreated
cd src/GrowDate.Api
dotnet run
```

### View Database
```bash
sqlite3 src/GrowDate.Api/growdate.db
.schema
SELECT * FROM Regions;
SELECT * FROM Crops;
.quit
```

## API Endpoints

### Base URL
- Development: `https://localhost:7000`

### Regions
- `GET /api/regions` - All regions
- `GET /api/regions/{id}` - Single region
- `GET /api/regions/by-country/{country}` - Filter by country
- `GET /api/regions/by-zone/{zone}` - Filter by climate zone

### Crops
- `GET /api/crops` - All crops
- `GET /api/crops/{id}` - Single crop
- `GET /api/crops/by-zone/{zone}` - Filter by zone
- `GET /api/crops/by-category/{category}` - Filter by category

### Recommendations
- `GET /api/recommendations?regionId={id}&selectedDate={date}` - Get recommendations
- `GET /api/recommendations/crops?regionId={id}` - Crops for region
- `GET /api/recommendations/detailed?cropId={id}&regionId={id}&selectedDate={date}` - Detailed info

## Testing

```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --verbosity detailed

# Run specific test class
dotnet test --filter "FullyQualifiedName~RecommendationServiceTests"

# Run with coverage (requires coverlet)
dotnet test /p:CollectCoverage=true
```

## Common Development Tasks

### Add New Crop
1. Edit `src/GrowDate.Infrastructure/Data/DatabaseSeeder.cs`
2. Add crop to the crops list
3. Delete `growdate.db`
4. Restart API

### Add New Region
1. Edit `src/GrowDate.Infrastructure/Data/DatabaseSeeder.cs`
2. Add region to the regions list
3. Delete `growdate.db`
4. Restart API

### Create New Page
1. Add `.razor` file in `src/GrowDate.Frontend/Pages/`
2. Use `@page "/route"` directive
3. Add navigation link in `NavMenu.razor`

### Create New API Endpoint
1. Add method in appropriate controller
2. Use `[HttpGet]`, `[HttpPost]`, etc. attributes
3. Test with Swagger UI

## Troubleshooting

### Frontend can't reach API
- Check API is running on https://localhost:7000
- Update `src/GrowDate.Frontend/wwwroot/appsettings.json`
- Check CORS configuration in `Program.cs`

### Database errors
- Delete `growdate.db` and restart
- Check EF Core packages are installed
- Verify connection string in `appsettings.json`

### Build errors
- Run `dotnet restore`
- Clean solution: `dotnet clean`
- Rebuild: `dotnet build`

### Port conflicts
- Change ports in `launchSettings.json` (if present)
- Or use `dotnet run --urls "https://localhost:PORT"`

## Code Style

- Use C# naming conventions
- Follow Clean Architecture principles
- Keep controllers thin, services thick
- Write tests for business logic
- Document public APIs

## Performance Tips

- Database is in-memory on startup for fast prototyping
- Use async/await for all I/O operations
- Consider caching for production
- Optimize queries with proper indexes

## Security Notes

- CORS is wide open for development
- No authentication in prototype
- SQLite is not production-ready
- Validate all inputs in production

## Deployment Checklist

- [ ] Switch to PostgreSQL/SQL Server
- [ ] Add authentication/authorization
- [ ] Configure proper CORS
- [ ] Add logging and monitoring
- [ ] Set up CI/CD pipeline
- [ ] Add rate limiting
- [ ] Enable HTTPS only
- [ ] Add health checks
- [ ] Configure caching
- [ ] Add API versioning

## Useful Commands

```bash
# Format code
dotnet format

# List outdated packages
dotnet list package --outdated

# Update packages
dotnet add package <PackageName>

# Create migration (if using migrations)
dotnet ef migrations add MigrationName

# Apply migrations
dotnet ef database update

# Watch for changes and auto-rebuild
dotnet watch run
```

## Resources

- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [Blazor Documentation](https://docs.microsoft.com/aspnet/core/blazor/)
- [EF Core Documentation](https://docs.microsoft.com/ef/core/)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
