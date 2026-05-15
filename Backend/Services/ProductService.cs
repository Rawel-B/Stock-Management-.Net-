using Microsoft.EntityFrameworkCore;
using StockManagement.Data;
using StockManagement.Models;
using StockManagement.ViewModels;

namespace StockManagement.Services;

public class ProductService(ApplicationDbContext dbContext)
{
    public async Task<List<ProductViewModel>> GetProductsAsync()
    {
        return await dbContext.Products
            .OrderBy(product => product.Name)
            .Select(product => new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Sku = product.Sku,
                StockQuantity = product.StockQuantity,
                UnitPrice = product.UnitPrice,
                SupplierId = product.SupplierId,
                SupplierName = product.Supplier == null ? null : product.Supplier.Name
            })
            .ToListAsync();
    }

    public async Task<List<SelectOptionViewModel>> GetSupplierOptionsAsync()
    {
        return await dbContext.Suppliers
            .OrderBy(supplier => supplier.Name)
            .Select(supplier => new SelectOptionViewModel { Id = supplier.Id, Name = supplier.Name })
            .ToListAsync();
    }

    public async Task SaveProductAsync(ProductEditModel model)
    {
        if (model.Id == 0)
        {
            dbContext.Products.Add(new Product
            {
                Name = model.Name,
                Sku = model.Sku,
                StockQuantity = model.StockQuantity,
                UnitPrice = model.UnitPrice,
                SupplierId = model.SupplierId
            });
        }
        else
        {
            var product = await dbContext.Products.FindAsync(model.Id)
                ?? throw new InvalidOperationException("Product not found.");

            product.Name = model.Name;
            product.Sku = model.Sku;
            product.StockQuantity = model.StockQuantity;
            product.UnitPrice = model.UnitPrice;
            product.SupplierId = model.SupplierId;
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task DeleteProductAsync(int productId)
    {
        var product = await dbContext.Products.FindAsync(productId)
            ?? throw new InvalidOperationException("Product not found.");

        dbContext.Products.Remove(product);
        await dbContext.SaveChangesAsync();
    }
}
