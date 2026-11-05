
# SmartShop API

[![.NET](https://github.com/supunsarachitha/SmartShop-API/actions/workflows/dotnet.yml/badge.svg)](https://github.com/supunsarachitha/SmartShop-API/actions/workflows/dotnet.yml)
[![Dependency Review](https://github.com/supunsarachitha/SmartShop-API/actions/workflows/dependency-review.yml/badge.svg)](https://github.com/supunsarachitha/SmartShop-API/actions/workflows/dependency-review.yml)
![License](https://img.shields.io/github/license/supunsarachitha/SmartShop-API)
![Last Commit](https://img.shields.io/github/last-commit/supunsarachitha/SmartShop-API)
![Top Language](https://img.shields.io/github/languages/top/supunsarachitha/SmartShop-API)

SmartShop API is a **.NET-based backend service** designed to power e-commerce applications. It provides endpoints for managing products, customers, users, invoices, payments and settings, enabling developers to build scalable and maintainable online store solutions.

---

## Features

- **Product Management** – Create, update, delete, and list products (name, price, stock) via secure endpoints.
- **Customer Management** – Full CRUD for customer profiles.
- **User Management** – Full CRUD operations for user accounts with role support (`SysAdmin`, `StoreAdmin`).
- **Invoice Processing** – Create invoices with line items and payments, with auto-generated invoice numbers.
- **Payment Method Management** – Add, update, view, and delete payment methods.
- **Settings** – Store key/value application settings through the API.
- **Authentication** – Secure JWT-based login endpoint (tokens are valid for 30 minutes); every endpoint except login and status requires a token.
- **Health Check Endpoint** – `GET /api/Status` for API status and health monitoring.
- **RESTful API** – Clean, predictable endpoints using standard HTTP conventions.
- **Entity Models** – Strongly typed C# classes for application data.
- **Extensible Architecture** – Easily integrates with payment gateways, inventory, and analytics systems.
- **Multi-platform Docker support** (`amd64` and `arm64`)

## Tech Stack

- **Language:** C#
- **Framework:** .NET 9 (ASP.NET Core)
- **Database:** PostgreSQL
- **ORM:** Entity Framework Core (Npgsql provider)
- **Authentication:** JWT-based authentication
- **API Style:** RESTful endpoints, documented with Swagger UI (Development only)
- **Hosting:** Compatible with Azure, AWS, or on-premise deployment

---

## Project Structure

```
SmartShop-API/
│
├── SmartShop.API/           # Main API project
│   ├── Controllers/         # API endpoints
│   ├── Models/              # Entity, request, response and DTO classes
│   ├── Services/            # Business logic
│   ├── Interfaces/          # Service contracts
│   ├── Helpers/             # Shared helpers
│   ├── Enums/               # Enumerations (e.g. invoice status)
│   ├── Data/                # Database context (EF Core)
│   ├── Migrations/          # EF Core migrations
│   └── Program.cs           # Application entry point
│
├── SmartShop.UnitTests/         # Unit test project
├── SmartShop.IntegrationTests/  # Integration test project
│
├── .github/workflows/       # CI/CD pipelines
├── Dockerfile               # Container image
├── compose.yaml             # Docker Compose file
├── SmartShop.API.sln        # Solution file
├── LICENSE.txt              # Apache License 2.0
└── README.md                # Project documentation
```

---

## Getting Started

### 1️ Prerequisites
- [.NET 9 SDK](https://dotnet.microsoft.com/download)
- A running PostgreSQL instance
- Git

### 2️ Clone the Repository
```bash
git clone https://github.com/supunsarachitha/SmartShop-API.git
cd SmartShop-API
```

### 3️ Configure Environment
Create an `appsettings.json` file in `SmartShop.API/` (it is git-ignored) with your PostgreSQL connection string and JWT settings. The app refuses to start without `Jwt:Key`:
```json
{
  "ConnectionStrings":
  {
    "DefaultConnection": "Host=localhost;Database=SmartShop;Username=youruser;Password=yourpassword"
  },
  "Jwt":
  {
    "Key": "your-256-bit-secret-key-should-be-at-least-32-characters-long",
    "Issuer": "SmartShopIssuer",
    "Audience": "SmartShopAudience"
  }
}
```

### Initial Database Setup and Seeding

The migrations are included in the repository. Applying them creates the schema and seeds the user roles and an admin user. The seed data is defined in `SmartShopDbContext.OnModelCreating`.
To use a different default admin, change it in `SmartShopDbContext.cs` and add a new migration before updating the database.

**Sample demo seeded Data:**
- Two roles: `SysAdmin` and `StoreAdmin`
- One admin user:
  - Username: `admin`
  - Email: `admin@smartshop.local`
  - Password: `admin123` (hashed in the database)

**How to apply the migrations and seed:**
1. Ensure your connection string is set up in `appsettings.json`.
2. Install the EF Core CLI once (`dotnet tool install --global dotnet-ef`), then apply the existing migrations:
```
dotnet ef database update --project SmartShop.API
```
3. The database is created and seeded.
 

### Build and Run Locally

```sh
dotnet restore
dotnet build
dotnet run --project SmartShop.API/SmartShop.API.csproj
```

The API will be available at `http://localhost:5218` by default (the `https` launch profile also serves `https://localhost:7002`).

---

#### Run Locally with Docker

```sh
docker run -p 8080:80 \
  -e "ConnectionStrings__DefaultConnection=Host=your-db-host;Database=SmartShop;Username=youruser;Password=yourpassword" \
  -e "Jwt__Key=your-256-bit-secret-key-should-be-at-least-32-characters-long" \
  -e "Jwt__Issuer=SmartShopIssuer" \
  -e "Jwt__Audience=SmartShopAudience" \
  stechbuzz/smartshop-api:latest
```

The API will be available at `http://localhost:8080`. The container is a Production build, so Swagger UI is off; its health check calls `GET /api/Status`.


## API Documentation

When running in the Development environment (`dotnet run`), you can explore the API via Swagger UI:  
```
http://localhost:5218/swagger
```

All endpoints except login and status need a JWT. Get one by posting the seeded credentials to the login endpoint, then send it as `Authorization: Bearer <token>`:

```sh
curl -X POST http://localhost:5218/api/Auth/login \
  -H "Content-Type: application/json" \
  -d '{"userName":"admin","password":"admin123"}'
```

| Route | Methods | Auth |
|---|---|---|
| `/api/Auth/login` | `POST` | No |
| `/api/Status` | `GET` | No |
| `/api/Products`, `/api/Customers`, `/api/Invoices`, `/api/PaymentMethods`, `/api/Settings`, `/api/Users` | `GET` (list), `GET /{id}`, `POST`, `PUT /{id}`, `DELETE /{id}` | JWT |

---

## Testing

Run unit tests with:
```bash
dotnet test
```
This solution includes unit and integration test projects to ensure SmartShop-API’s reliability. These test projects help maintain high code quality and a stable API.

**Unit tests** check individual components in isolation, using mocks to simulate dependencies and provide quick feedback during development.

**Integration tests** verify that different modules and services work together correctly, catching issues that unit tests may miss.

---

## License

This project is licensed under the [Apache License 2.0](LICENSE.txt).

---

## Contributing

We welcome contributions of all kinds—bug fixes, feature enhancements, documentation improvements, and more.

 See our [Contributing Guidelines](CONTRIBUTING.md) to get started.


---

## Contact

**Author:** Supun Sarachitha  
**GitHub:** [supunsarachitha](https://github.com/supunsarachitha)
