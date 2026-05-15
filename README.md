# Stock Management

Stock Management is an ASP.NET Core Blazor Web App for managing customer orders, deliveries, inventory, transporters, suppliers, and payments.

## Tech Stack

- .NET 8
- ASP.NET Core Blazor Web App
- ASP.NET Core Identity
- JWT bearer authentication for API access
- Entity Framework Core
- SQL Server / SQL Server LocalDB
- MudBlazor
- Docker Compose with SQL Server 2022

## Project Structure

```text
Frontend/
  Components/      Blazor pages, layouts, and account UI
  ViewModels/      UI-facing models used by Blazor components

Backend/
  Data/            EF Core DbContext, Identity user, migrations
  Endpoints/       Minimal API route mapping
  Models/          Database entities and enums
  Services/        Business logic, data access, JWT generation, workflows

wwwroot/           Static web assets served by ASP.NET Core
Properties/        Launch profiles for local development
Program.cs         Application startup, DI registration, middleware, route mapping
appsettings*.json  Runtime configuration and connection strings
Dockerfile         Container build definition
docker-compose.yml App + SQL Server development environment
```

`Program.cs`, `wwwroot`, `Properties`, configuration files, Docker files, `.sln`, and `.csproj` intentionally remain at the root because this is a single ASP.NET Core hosted Blazor Web App.

## Features

- Authenticated dashboard
- Client CRUD
- Supplier CRUD
- Product and stock management
- Order creation and validation
- Automatic stock deduction when orders are validated
- Delivery scheduling and status tracking
- Transporter CRUD
- Payment tracking
- JWT login for protected API routes
- Database seeding for development

## Development Login

The application seeds an admin user when the database is empty.

```text
Email:    admin@stock.local
Password: Admin123!
```

## Run Locally

Requirements:

- Visual Studio 2022 or .NET 8 SDK
- SQL Server LocalDB or SQL Server

```powershell
dotnet restore
dotnet build
dotnet run --project .\StockManagement.csproj
```

Open:

```text
http://localhost:5127
```

The default connection string uses SQL Server LocalDB:

```json
"Server=(localdb)\\mssqllocaldb;Database=StockManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
```

## Run With Docker

Requirements:

- Docker Desktop running with the Linux engine

```powershell
docker compose up --build
```

Open:

```text
http://localhost:8080
```

SQL Server is exposed on host port `14333`.

## JWT API Login

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "admin@stock.local",
  "password": "Admin123!"
}
```

Use the returned token as a bearer token for protected API routes:

```text
GET /api/clients
GET /api/orders
GET /api/deliveries
GET /api/payments
```

## Verification

Useful checks before committing:

```powershell
dotnet build StockManagement.sln
docker compose config
```

`docker compose build` requires Docker Desktop to be running.
