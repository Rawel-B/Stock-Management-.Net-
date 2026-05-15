using Microsoft.EntityFrameworkCore;
using StockManagement.Data;
using StockManagement.Models;
using StockManagement.ViewModels;

namespace StockManagement.Services;

public class DeliveryService(ApplicationDbContext dbContext)
{
    public async Task<DeliveryPageViewModel> GetDeliveryPageAsync()
    {
        return new DeliveryPageViewModel
        {
            ValidatedOrders = await dbContext.Orders
                .Where(order => order.Status == OrderStatus.Validated && order.Delivery == null)
                .OrderByDescending(order => order.Date)
                .Select(order => new SelectOptionViewModel { Id = order.Id, Name = $"#{order.Id} - {order.Client!.Name}" })
                .ToListAsync(),
            Transporters = await dbContext.Transporters
                .OrderBy(transporter => transporter.Name)
                .Select(transporter => new SelectOptionViewModel { Id = transporter.Id, Name = transporter.Name })
                .ToListAsync(),
            Deliveries = await dbContext.Deliveries
                .OrderByDescending(delivery => delivery.DeliveryDate)
                .Select(delivery => new DeliveryViewModel
                {
                    Id = delivery.Id,
                    OrderId = delivery.OrderId,
                    ClientName = delivery.Order!.Client!.Name,
                    TransporterName = delivery.Transporter!.Name,
                    DeliveryDate = delivery.DeliveryDate,
                    Cost = delivery.Cost,
                    Status = delivery.Status
                })
                .ToListAsync()
        };
    }

    public async Task CreateDeliveryAsync(DeliveryEditModel model)
    {
        dbContext.Deliveries.Add(new Delivery
        {
            OrderId = model.OrderId,
            TransporterId = model.TransporterId,
            DeliveryDate = model.DeliveryDate,
            Cost = model.Cost,
            Status = DeliveryStatus.Planned
        });

        await dbContext.SaveChangesAsync();
    }
}
