# Product Management API

ASP.NET Core Web API for managing products, built with **Clean Architecture** and **DDD** principles.

## Solution Structure

```
ProductManagementApi/
├── API/                    # Presentation layer (startup project)
├── Application/            # Use cases, CQRS, validators
├── Domain/                 # Entities, value objects, domain events
├── Infrastructure/         # EF Core, repositories, SQL Server
└── Tests/
    └── IntegrationTests/   # API integration tests
```

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [SQL Server](https://www.microsoft.com/sql-server) (LocalDB, Express, or Developer Edition)
- Optional: Visual Studio 2022 / VS Code / Rider

## Database Setup

### Option 1: Windows Authentication (recommended for local dev)

The default connection string in `API/appsettings.json` uses Windows Authentication:

```json
"DefaultConnection": "Server=.;Database=ProductManagement;Trusted_Connection=True;TrustServerCertificate=True;"
```

On first run in **Development** mode, the API automatically creates the database and `Products` table via `EnsureCreated()`.

### Option 2: SQL Server Authentication (sa login)

Update `API/appsettings.json`:

```json
"DefaultConnection": "Server=.;Database=ProductManagement;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True;"
```

Create the database manually in SSMS if needed:

```sql
CREATE DATABASE ProductManagement;
```

### Option 3: EF Core Migrations (optional)

```bash
dotnet ef migrations add InitialCreate --project Infrastructure --startup-project API
dotnet ef database update --project Infrastructure --startup-project API
```

## Getting Started

### 1. Clone and navigate to the solution

```bash
cd ProductManagementApi
```

### 2. Restore packages

```bash
dotnet restore ProductManagementApi.slnx
```

### 3. Build the solution

```bash
dotnet build ProductManagementApi.slnx
```

### 4. Run the API

```bash
cd API
dotnet run
```

Or set **API** as the startup project in Visual Studio and press **F5**.

### 5. Open Swagger UI

The browser opens automatically at:

- **Swagger:** [http://localhost:5204/swagger](http://localhost:5204/swagger)
- **HTTPS:** [https://localhost:7121/swagger](https://localhost:7121/swagger)

## API Endpoints

Base URL: `http://localhost:5204/api/products`

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/api/products` | Get all products (paginated) |
| `GET` | `/api/products/search?search={value}` | Search by name, price, stock, or currency |
| `GET` | `/api/products/{id}` | Get product by ID |
| `POST` | `/api/products` | Create a new product |
| `PUT` | `/api/products/{id}` | Update a product |
| `DELETE` | `/api/products/{id}` | Delete a product |

### Search examples

```http
GET /api/products/search?search=pen
GET /api/products/search?search=USD
GET /api/products/search?search=29.99
GET /api/products/search?search=100
```

### Create product example

```http
POST /api/products
Content-Type: application/json

{
  "name": "Pen",
  "description": "Red pen",
  "price": 5.50,
  "stock": 100,
  "currency": "USD"
}
```

> **Note:** `currency` must be a 3-letter ISO code (e.g. `USD`, `EUR`). Product names must be unique.

## Run Tests

The `IntegrationTests` project includes **unit tests** and **integration tests**:

```bash
dotnet test ProductManagementApi.slnx
```

### Test structure

```
Tests/IntegrationTests/
├── Unit/
│   ├── Domain/              # Product entity tests
│   ├── Application/
│   │   ├── Commands/        # Create, Update, Delete handlers
│   │   ├── Queries/         # Get, Search handlers
│   │   └── Validators/      # FluentValidation rules
│   └── Infrastructure/      # ProductRepository tests
└── Integration/             # Full API HTTP tests
```

## Docker (optional)

Build and run with Docker from the solution root:

```bash
docker build -t product-management-api -f Dockerfile .
docker run -p 8080:8080 -p 8081:8081 product-management-api
```

## Configuration

| File | Purpose |
|------|---------|
| `API/appsettings.json` | Connection string, logging |
| `API/appsettings.Development.json` | Development overrides |
| `API/Properties/launchSettings.json` | URLs and launch profile |

## Architecture

```
API → Application → Domain
  ↘ Infrastructure → Application → Domain
```

- **Domain** — No external dependencies. Contains `Product` aggregate, `Money` value object, domain events.
- **Application** — MediatR (CQRS), FluentValidation, repository interfaces.
- **Infrastructure** — EF Core, SQL Server, repository implementations.
- **API** — Controllers, Swagger, exception handling middleware.

## Troubleshooting

| Issue | Solution |
|-------|----------|
| `Login failed for user 'sa'` | Use Windows Authentication or set the correct `sa` password in `appsettings.json` |
| `Cannot open database "ProductManagement"` | Create the database in SSMS or restart the API in Development mode |
| Build fails (file locked) | Stop the running API process, then rebuild |
| Validation error on create | Use 3-letter currency code (`USD` not `dollar`) |

## Tech Stack

- .NET 10
- ASP.NET Core Web API
- Entity Framework Core 10
- MediatR (CQRS)
- FluentValidation
- Swashbuckle (Swagger UI)
- SQL Server
