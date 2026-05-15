# Stock Management

ASP.NET Core Blazor Web App for managing orders, deliveries, stock, clients, suppliers, transporters, and payments.

## Stack

- ASP.NET Core Blazor Web App (.NET 8)
- Entity Framework Core
- SQL Server / LocalDB
- ASP.NET Core Identity plus JWT bearer tokens for API access
- MudBlazor
- Docker Compose with SQL Server 2022

## Development Login

The app seeds a development admin user when the database is empty:

- Email: `admin@stock.local`
- Password: `Admin123!`

## Run Locally

```powershell
dotnet restore
dotnet run --project .\StockManagement.csproj
```

The default connection string uses SQL Server LocalDB:

```json
"Server=(localdb)\\mssqllocaldb;Database=StockManagementDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True"
```

## Run With Docker

```powershell
docker compose up --build
```

Open `http://localhost:8080`.

## JWT API Login

```http
POST /api/auth/login
Content-Type: application/json

{
  "email": "admin@stock.local",
  "password": "Admin123!"
}
```

Use the returned bearer token for `/api/clients`, `/api/orders`, `/api/deliveries`, and `/api/payments`.
