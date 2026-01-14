using GrowDate.Infrastructure.Data;
using GrowDate.Infrastructure.Repositories;
using GrowDate.Core.Interfaces;
using GrowDate.Core.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Configure CORS for frontend
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:5101",
                "https://localhost:5101",
                "http://frontend",
                "http://frontend:80",
                "https://ideal-winner-5w66vv46wp534jw7-5101.app.github.dev"
            )
            .AllowAnyMethod()
            .AllowAnyHeader();
        });
});

// Configure Database
builder.Services.AddDbContext<GrowDateDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<ICropRepository, CropRepository>();
builder.Services.AddScoped<IRegionRepository, RegionRepository>();

// Register services
builder.Services.AddScoped<IRecommendationService, RecommendationService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowFrontend");

// Only redirect to HTTPS in production when HTTPS is properly configured
if (app.Environment.IsDevelopment() || !string.IsNullOrEmpty(Environment.GetEnvironmentVariable("ASPNETCORE_HTTPS_PORT")))
{
    app.UseHttpsRedirection();
}

app.UseAuthorization();

app.MapControllers();

// Seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<GrowDateDbContext>();
    // Apply migrations when available; fall back to EnsureCreated for first-time setups
    var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
    if (pendingMigrations.Any())
    {
        await context.Database.MigrateAsync();
    }
    else
    {
        context.Database.EnsureCreated();
    }

    await DatabaseSeeder.SeedAsync(context);
}

app.Run();
