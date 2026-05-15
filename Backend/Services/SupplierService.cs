using Microsoft.EntityFrameworkCore;
using StockManagement.Data;
using StockManagement.Models;
using StockManagement.ViewModels;

namespace StockManagement.Services;

public class SupplierService(ApplicationDbContext dbContext)
{
    public async Task<List<SupplierViewModel>> GetSuppliersAsync()
    {
        return await dbContext.Suppliers
            .OrderBy(supplier => supplier.Name)
            .Select(supplier => new SupplierViewModel
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Phone = supplier.Phone,
                Email = supplier.Email,
                ProductCount = supplier.Products.Count
            })
            .ToListAsync();
    }

    public async Task SaveSupplierAsync(SupplierEditModel model)
    {
        if (model.Id == 0)
        {
            dbContext.Suppliers.Add(new Supplier
            {
                Name = model.Name,
                Phone = model.Phone,
                Email = model.Email
            });
        }
        else
        {
            var supplier = await dbContext.Suppliers.FindAsync(model.Id)
                ?? throw new InvalidOperationException("Supplier not found.");

            supplier.Name = model.Name;
            supplier.Phone = model.Phone;
            supplier.Email = model.Email;
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteSupplierAsync(int supplierId)
    {
        var supplier = await dbContext.Suppliers.FindAsync(supplierId)
            ?? throw new InvalidOperationException("Supplier not found.");

        dbContext.Suppliers.Remove(supplier);
        await dbContext.SaveChangesAsync();
    }
}
