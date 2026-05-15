using Microsoft.EntityFrameworkCore;
using StockManagement.Data;
using StockManagement.ViewModels;

namespace StockManagement.Services;

public class PaymentService(ApplicationDbContext dbContext)
{
    public async Task<List<PaymentViewModel>> GetPaymentsAsync()
    {
        return await dbContext.Payments
            .OrderByDescending(payment => payment.Date)
            .Select(payment => new PaymentViewModel
            {
                OrderId = payment.OrderId,
                ClientName = payment.Order!.Client!.Name,
                Amount = payment.Order.TotalAmount,
                Mode = payment.Mode,
                Status = payment.Status,
                Date = payment.Date
            })
            .ToListAsync();
    }
}
