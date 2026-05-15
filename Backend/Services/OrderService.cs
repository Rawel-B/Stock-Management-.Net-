using Microsoft.EntityFrameworkCore;
using StockManagement.Data;
using StockManagement.ViewModels;

namespace StockManagement.Services;

public class OrderService(ApplicationDbContext dbContext)
{
    public async Task<OrderPageViewModel> GetOrderPageAsync()
    {
        return new OrderPageViewModel
        {
            Clients = await dbContext.Clients
                .OrderBy(client => client.Name)
                .Select(client => new SelectOptionViewModel { Id = client.Id, Name = client.Name })
                .ToListAsync(),
            Products = await dbContext.Products
                .OrderBy(product => product.Name)
                .Select(product => new OrderProductOptionViewModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    StockQuantity = product.StockQuantity,
                    UnitPrice = product.UnitPrice
                })
                .ToListAsync(),
            Orders = await dbContext.Orders
                .OrderByDescending(order => order.Date)
                .Select(order => new OrderViewModel
                {
                    Id = order.Id,
                    ClientName = order.Client!.Name,
                    Status = order.Status,
                    TotalAmount = order.TotalAmount,
                    Date = order.Date,
                    Lines = order.Lines.Select(line => new OrderLineViewModel
                    {
                        ProductName = line.Product!.Name,
                        Quantity = line.Quantity,
                        UnitPrice = line.UnitPrice
                    }).ToList()
                })
                .ToListAsync()
        };
    }
}
