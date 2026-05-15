using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using StockManagement.Data;
using StockManagement.Models;

namespace StockManagement.Services;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        await dbContext.Database.MigrateAsync();

        if (!await userManager.Users.AnyAsync())
        {
            var admin = new ApplicationUser
            {
                UserName = "admin@stock.local",
                Email = "admin@stock.local",
                EmailConfirmed = true
            };

            await userManager.CreateAsync(admin, "Admin123!");
        }

        if (await dbContext.Clients.AnyAsync())
        {
            return;
        }

        var supplier = new Supplier { Name = "Northline Supplies", Phone = "+1 555 0100", Email = "orders@northline.example" };
        var products = new List<Product>
        {
            new() { Name = "Wireless Scanner", Sku = "SCAN-100", StockQuantity = 42, UnitPrice = 180, Supplier = supplier },
            new() { Name = "Thermal Labels", Sku = "LBL-220", StockQuantity = 700, UnitPrice = 8.50m, Supplier = supplier },
            new() { Name = "Inventory Tablet", Sku = "TAB-410", StockQuantity = 18, UnitPrice = 320, Supplier = supplier }
        };

        var clients = new List<Client>
        {
            new() { Name = "Acme Retail", Email = "ops@acme.example", Address = "12 Market Street" },
            new() { Name = "Bright Foods", Email = "logistics@bright.example", Address = "44 Warehouse Avenue" }
        };

        dbContext.Suppliers.Add(supplier);
        dbContext.Products.AddRange(products);
        dbContext.Clients.AddRange(clients);
        dbContext.Transporters.AddRange(
            new Transporter { Name = "FastWay Logistics", Phone = "+1 555 0190", Rating = 5 },
            new Transporter { Name = "City Courier", Phone = "+1 555 0191", Rating = 4 });

        await dbContext.SaveChangesAsync();
    }
}
