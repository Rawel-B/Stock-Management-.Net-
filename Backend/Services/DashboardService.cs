using Microsoft.EntityFrameworkCore;
using StockManagement.Data;
using StockManagement.Models;
using StockManagement.ViewModels;

namespace StockManagement.Services;

public class DashboardService(ApplicationDbContext dbContext)
{
    public async Task<DashboardViewModel> GetDashboardAsync()
    {
        return new DashboardViewModel
        {
            OpenOrders = await dbContext.Orders.CountAsync(order => order.Status != OrderStatus.Delivered && order.Status != OrderStatus.Cancelled),
            LowStockProducts = await dbContext.Products.CountAsync(product => product.StockQuantity <= 10),
            PendingPayments = await dbContext.Payments.CountAsync(payment => payment.Status == PaymentStatus.Pending),
            Revenue = await dbContext.Payments
                .Where(payment => payment.Status == PaymentStatus.Paid)
                .Select(payment => payment.Order!.TotalAmount)
                .SumAsync(),
            RecentOrders = await dbContext.Orders
                .OrderByDescending(order => order.Date)
                .Take(8)
                .Select(order => new DashboardOrderViewModel
                {
                    Id = order.Id,
                    ClientName = order.Client!.Name,
                    Status = order.Status,
                    TotalAmount = order.TotalAmount,
                    Date = order.Date
                })
                .ToListAsync()
        };
    }
}
