# 🌱 GrowDate - Your Intelligent Planting Companion

GrowDate is a comprehensive planting calendar application designed to help farmers and gardeners determine the optimal times to plant different crops based on their geographic region. Built with C# and .NET 8, featuring a beautiful Blazor WebAssembly frontend with an interactive calendar and region selection system.

## 🎯 Features

- **📅 Interactive Planting Calendar**: Visual calendar interface showing ideal planting dates for your region
- **🗺️ Region Selection System**: Interactive map-based region selector with multiple climate zones
- **🌾 Crop Database**: Comprehensive database of 20+ common crops with detailed planting information
- **🤖 Smart Recommendations**: Intelligent recommendation engine that matches crops to your region and selected dates
- **📊 Detailed Planting Information**: 
  - Planting windows
  - Germination timelines
  - Harvest estimates
  - Climate zone compatibility
  - Growing season status

## 🏗️ Architecture

GrowDate follows **Clean Architecture** principles with clear separation of concerns:

```
GrowDate/
├── src/
│   ├── GrowDate.Core/           # Domain entities, interfaces, services, DTOs
│   ├── GrowDate.Infrastructure/ # Data access, repositories, EF Core
│   ├── GrowDate.Api/            # REST API controllers and endpoints
│   ├── GrowDate.Frontend/       # Blazor WebAssembly UI
│   └── GrowDate.Tests/          # xUnit tests
└── GrowDate.sln
```

## 🛠️ Tech Stack

### Backend
- **.NET 8** - Latest .NET framework
- **ASP.NET Core Web API** - RESTful API endpoints
- **Entity Framework Core** - ORM and data access
- **SQLite** - Lightweight database for prototyping

### Frontend
- **Blazor WebAssembly** - Modern C# web framework
- **HTML5 Canvas** - Interactive map visualization
- **CSS3** - Responsive design with custom styling

### Testing
- **xUnit** - Unit testing framework
- **Moq** - Mocking library for isolated tests
- **EF Core InMemory** - In-memory database for testing

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- A code editor (Visual Studio 2022, VS Code, or Rider)

### Installation

1. **Clone the repository**
   ```bash
   git clone https://github.com/charlesdesouza88/Growdate.git
   cd Growdate
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

### Running the Application

#### Option 1: Run Both API and Frontend

**Terminal 1 - Start the API:**
```bash
cd src/GrowDate.Api
dotnet run
```
The API will start at `http://localhost:5100` (Swagger at `/swagger`).

**Terminal 2 - Start the Frontend:**
```bash
cd src/GrowDate.Frontend
dotnet run
```
The frontend will start at `http://localhost:5101`.

#### Option 2: Using Visual Studio

1. Right-click the solution in Solution Explorer
2. Select "Configure Startup Projects"
3. Choose "Multiple startup projects"
4. Set both `GrowDate.Api` and `GrowDate.Frontend` to "Start"
5. Press F5 to run

### Running Tests

```bash
cd src/GrowDate.Tests
dotnet test
```

Or run all tests from the solution root:
```bash
dotnet test
```

## 📖 API Documentation

Once the API is running, visit `http://localhost:5100/swagger` to see the interactive API documentation.

### Key Endpoints

#### Regions
- `GET /api/regions` - Get all regions
- `GET /api/regions/{id}` - Get region by ID
- `GET /api/regions/by-country/{country}` - Get regions by country
- `GET /api/regions/by-zone/{zone}` - Get regions by climate zone

#### Crops
- `GET /api/crops` - Get all crops
- `GET /api/crops/{id}` - Get crop by ID
- `GET /api/crops/by-zone/{zone}` - Get crops by climate zone
- `GET /api/crops/by-category/{category}` - Get crops by category

#### Recommendations
- `GET /api/recommendations?regionId={id}&selectedDate={date}` - Get planting recommendations
- `GET /api/recommendations/crops?regionId={id}` - Get crops for a region
- `GET /api/recommendations/detailed?cropId={id}&regionId={id}&selectedDate={date}` - Get detailed recommendation

## 🌍 Regions & Climate Zones

The application includes 6 pre-seeded regions:

| Region | Country | Climate Zone | Coordinates |
|--------|---------|--------------|-------------|
| California Central Valley | USA | Zone 9 | 36.78°N, 119.42°W |
| Florida | USA | Zone 10 | 27.99°N, 81.76°W |
| Texas Hill Country | USA | Zone 8 | 30.27°N, 98.75°W |
| São Paulo State | Brazil | Tropical | 23.55°S, 46.63°W |
| Minas Gerais | Brazil | Subtropical | 19.92°S, 43.93°W |
| Pacific Northwest | USA | Zone 8 | 47.61°N, 122.33°W |

## 🌾 Included Crops

20 crops across three categories:

### Vegetables
Tomatoes, Lettuce, Carrots, Peppers, Cucumbers, Beans, Spinach, Zucchini, Corn, Radishes, Broccoli, Pumpkin, Kale, Eggplant, Green Onions

### Fruits
Strawberries, Watermelon

### Herbs
Basil, Cilantro, Parsley

Each crop includes:
- Optimal planting window (start/end dates)
- Days to germination
- Days to harvest
- Suitable climate zones
- Category classification

## 🎨 Using the Application

### 1. Select Your Region
- Navigate to "Select Region" from the menu
- Choose your location from the interactive map or region cards
- Your region selection persists for the session

### 2. View the Calendar
- Navigate to "Calendar" to see the monthly view
- Days with planting opportunities are highlighted with 🌱
- Click any date to see what crops can be planted
- Navigate months using Previous/Next buttons

### 3. Get Recommendations
- Navigate to "Recommendations"
- Select a date to see what's plantable
- View detailed information including:
  - **Ideal**: Perfect time to plant
  - **Coming Soon**: Planting season starts within 30 days
  - **Late Season**: Within 30 days past optimal window
  - **Out of Season**: Not the right time to plant

## 🧪 Extending the Database

### Adding New Crops

Edit `src/GrowDate.Infrastructure/Data/DatabaseSeeder.cs`:

```csharp
new Crop
{
    Name = "Your Crop Name",
    Category = "Vegetable", // or "Fruit", "Herb"
    PlantingStartMonth = 3,  // March
    PlantingStartDay = 15,
    PlantingEndMonth = 6,    // June
    PlantingEndDay = 30,
    DaysToGermination = 7,
    DaysToHarvest = 70,
    SuitableZones = new List<string> { "Zone 8", "Zone 9" }
}
```

### Adding New Regions

```csharp
new Region
{
    Name = "Your Region Name",
    Country = "Country",
    ClimateZone = "Zone 9",  // or "Tropical", "Subtropical"
    Latitude = 40.7128,
    Longitude = -74.0060
}
```

Delete `growdate.v2.db` and restart the API to reseed the database.

## 🔮 Future Enhancements

- **User Authentication**: Personal accounts and saved preferences
- **3D Globe Visualization**: Three.js powered interactive globe
- **Frost Date Calculator**: Automatic frost date calculations
- **Mobile Apps**: Native iOS and Android apps using .NET MAUI
- **Weather Integration**: Real-time weather data for planting decisions
- **Garden Planning**: Multi-crop garden layout and companion planting
- **Notifications**: Reminders for planting, watering, and harvesting
- **Community Features**: Share gardens and tips with other users
- **AI Recommendations**: Machine learning for personalized suggestions
- **Soil Testing**: Integration with soil test results
- **Pest Management**: Pest identification and organic treatment recommendations

## 🧰 Development Tools

### Recommended VS Code Extensions
- C# Dev Kit
- .NET Extension Pack
- Blazor Snippet Pack

### Database Management

View the SQLite database:
```bash
# Install sqlite3 if not already installed
sqlite3 src/GrowDate.Api/growdate.v2.db

# View tables
.tables

# Query crops
SELECT * FROM Crops;

# Query regions
SELECT * FROM Regions;

# Exit
.quit
```

### Code Formatting

```bash
# Format all code
dotnet format
```

## 📊 Project Statistics

- **Lines of Code**: ~3,500+
- **Projects**: 5
- **Entities**: 3 (Crop, Region, PlantingRecommendation)
- **API Endpoints**: 13
- **Frontend Pages**: 4
- **Test Cases**: 8+
- **Seeded Crops**: 20
- **Seeded Regions**: 6

## 🤝 Contributing

Contributions are welcome! Please follow these steps:

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/AmazingFeature`)
3. Commit your changes (`git commit -m 'Add some AmazingFeature'`)
4. Push to the branch (`git push origin feature/AmazingFeature`)
5. Open a Pull Request

## 📝 License

This project is open source and available under the MIT License.

## 👨‍💻 Author

**Charles de Souza**
- GitHub: [@charlesdesouza88](https://github.com/charlesdesouza88)

## 🙏 Acknowledgments

- Inspired by the need to help farmers make data-driven planting decisions
- Built with modern .NET technologies and Clean Architecture principles
- Designed with farmers and home gardeners in mind

## 📧 Support

For questions, issues, or suggestions:
- Open an issue on GitHub
- Contact the maintainer

---

**Happy Growing! 🌱🌾🌻**
The Farmer’s Calendar is a simple yet powerful tool designed to help farmers determine the best planting times for various crops. By integrating agricultural data with a Google Calendar API and an interactive 3D map, the app shows farmers exactly when to plant different vegetables and fruits based on their specific region.
