using Microsoft.EntityFrameworkCore;
using StockManagement.Data;
using StockManagement.Models;
using StockManagement.ViewModels;

namespace StockManagement.Services;

public class TransporterService(ApplicationDbContext dbContext)
{
    public async Task<List<TransporterViewModel>> GetTransportersAsync()
    {
        return await dbContext.Transporters
            .OrderBy(transporter => transporter.Name)
            .Select(transporter => new TransporterViewModel
            {
                Id = transporter.Id,
                Name = transporter.Name,
                Phone = transporter.Phone,
                Rating = transporter.Rating,
                DeliveryCount = transporter.Deliveries.Count
            })
            .ToListAsync();
    }

    public async Task SaveTransporterAsync(TransporterEditModel model)
    {
        if (model.Id == 0)
        {
            dbContext.Transporters.Add(new Transporter
            {
                Name = model.Name,
                Phone = model.Phone,
                Rating = model.Rating
            });
        }
        else
        {
            var transporter = await dbContext.Transporters.FindAsync(model.Id)
                ?? throw new InvalidOperationException("Transporter not found.");

            transporter.Name = model.Name;
            transporter.Phone = model.Phone;
            transporter.Rating = model.Rating;
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteTransporterAsync(int transporterId)
    {
        var transporter = await dbContext.Transporters.FindAsync(transporterId)
            ?? throw new InvalidOperationException("Transporter not found.");

        dbContext.Transporters.Remove(transporter);
        await dbContext.SaveChangesAsync();
    }
}
