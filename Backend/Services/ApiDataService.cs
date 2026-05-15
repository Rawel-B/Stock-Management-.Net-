using Microsoft.EntityFrameworkCore;
using StockManagement.Data;
using StockManagement.Models;

namespace StockManagement.Services;

public class ApiDataService(ApplicationDbContext dbContext)
{
    public async Task<List<Client>> GetClientsAsync()
    {
        return await dbContext.Clients.OrderBy(client => client.Name).ToListAsync();
    }

    public async Task<Client> CreateClientAsync(Client client)
    {
        dbContext.Clients.Add(client);
        await dbContext.SaveChangesAsync();
        return client;
    }

    public async Task<List<Order>> GetOrdersAsync()
    {
        return await dbContext.Orders
            .Include(order => order.Client)
            .Include(order => order.Lines)
            .ThenInclude(line => line.Product)
            .OrderByDescending(order => order.Date)
            .ToListAsync();
    }

    public async Task<List<Delivery>> GetDeliveriesAsync()
    {
        return await dbContext.Deliveries
            .Include(delivery => delivery.Order)
            .ThenInclude(order => order!.Client)
            .Include(delivery => delivery.Transporter)
            .OrderByDescending(delivery => delivery.DeliveryDate)
            .ToListAsync();
    }

    public async Task<List<Payment>> GetPaymentsAsync()
    {
        return await dbContext.Payments
            .Include(payment => payment.Order)
            .ThenInclude(order => order!.Client)
            .OrderByDescending(payment => payment.Date)
            .ToListAsync();
    }
}
