using Microsoft.EntityFrameworkCore;
using StockManagement.Data;
using StockManagement.Models;
using StockManagement.ViewModels;

namespace StockManagement.Services;

public class ClientService(ApplicationDbContext dbContext)
{
    public async Task<List<ClientViewModel>> GetClientsAsync()
    {
        return await dbContext.Clients
            .OrderBy(client => client.Name)
            .Select(client => new ClientViewModel
            {
                Id = client.Id,
                Name = client.Name,
                Email = client.Email,
                Address = client.Address
            })
            .ToListAsync();
    }

    public async Task SaveClientAsync(ClientEditModel model)
    {
        if (model.Id == 0)
        {
            dbContext.Clients.Add(new Client
            {
                Name = model.Name,
                Email = model.Email,
                Address = model.Address
            });
        }
        else
        {
            var client = await dbContext.Clients.FindAsync(model.Id)
                ?? throw new InvalidOperationException("Client not found.");

            client.Name = model.Name;
            client.Email = model.Email;
            client.Address = model.Address;
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteClientAsync(int clientId)
    {
        var client = await dbContext.Clients.FindAsync(clientId)
            ?? throw new InvalidOperationException("Client not found.");

        dbContext.Clients.Remove(client);
        await dbContext.SaveChangesAsync();
    }
}
