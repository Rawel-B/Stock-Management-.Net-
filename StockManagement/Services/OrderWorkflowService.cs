using Microsoft.EntityFrameworkCore;
using StockManagement.Data;
using StockManagement.Models;

namespace StockManagement.Services;

public class OrderWorkflowService(ApplicationDbContext dbContext)
{
    public async Task<int> CreateOrderAsync(int clientId, IReadOnlyCollection<(int ProductId, int Quantity)> items)
    {
        if (items.Count == 0)
        {
            throw new InvalidOperationException("An order must contain at least one product.");
        }

        var productIds = items.Select(item => item.ProductId).ToArray();
        var products = await dbContext.Products
            .Where(product => productIds.Contains(product.Id))
            .ToDictionaryAsync(product => product.Id);

        var order = new Order
        {
            ClientId = clientId,
            Date = DateTime.UtcNow,
            Status = OrderStatus.Draft
        };

        foreach (var item in items)
        {
            if (!products.TryGetValue(item.ProductId, out var product))
            {
                throw new InvalidOperationException("One of the selected products no longer exists.");
            }

            if (item.Quantity <= 0)
            {
                throw new InvalidOperationException("Quantities must be greater than zero.");
            }

            order.Lines.Add(new OrderLine
            {
                ProductId = product.Id,
                Quantity = item.Quantity,
                UnitPrice = product.UnitPrice
            });
        }

        order.TotalAmount = order.Lines.Sum(line => line.Quantity * line.UnitPrice);
        dbContext.Orders.Add(order);
        await dbContext.SaveChangesAsync();

        return order.Id;
    }

    public async Task ValidateOrderAsync(int orderId)
    {
        var order = await dbContext.Orders
            .Include(existingOrder => existingOrder.Lines)
            .ThenInclude(line => line.Product)
            .FirstOrDefaultAsync(existingOrder => existingOrder.Id == orderId);

        if (order is null)
        {
            throw new InvalidOperationException("Order not found.");
        }

        if (order.Status != OrderStatus.Draft)
        {
            throw new InvalidOperationException("Only draft orders can be validated.");
        }

        foreach (var line in order.Lines)
        {
            if (line.Product is null || line.Product.StockQuantity < line.Quantity)
            {
                throw new InvalidOperationException($"Insufficient stock for {line.Product?.Name ?? "a selected product"}.");
            }
        }

        foreach (var line in order.Lines)
        {
            line.Product!.StockQuantity -= line.Quantity;
        }

        order.Status = OrderStatus.Validated;
        order.TotalAmount = order.Lines.Sum(line => line.Quantity * line.UnitPrice);

        if (order.Payment is null)
        {
            dbContext.Payments.Add(new Payment
            {
                OrderId = order.Id,
                Date = DateTime.UtcNow,
                Status = PaymentStatus.Pending,
                Mode = PaymentMode.OnlineGateway
            });
        }

        await dbContext.SaveChangesAsync();
    }

    public async Task MarkPaymentAsync(int orderId, PaymentStatus status, PaymentMode mode)
    {
        var payment = await dbContext.Payments.FirstOrDefaultAsync(existingPayment => existingPayment.OrderId == orderId);

        if (payment is null)
        {
            payment = new Payment { OrderId = orderId };
            dbContext.Payments.Add(payment);
        }

        payment.Date = DateTime.UtcNow;
        payment.Status = status;
        payment.Mode = mode;

        await dbContext.SaveChangesAsync();
    }

    public async Task MarkDeliveryAsync(int deliveryId, DeliveryStatus status)
    {
        var delivery = await dbContext.Deliveries
            .Include(existingDelivery => existingDelivery.Order)
            .FirstOrDefaultAsync(existingDelivery => existingDelivery.Id == deliveryId);

        if (delivery is null)
        {
            throw new InvalidOperationException("Delivery not found.");
        }

        delivery.Status = status;

        if (delivery.Order is not null)
        {
            delivery.Order.Status = status switch
            {
                DeliveryStatus.InTransit => OrderStatus.Shipped,
                DeliveryStatus.Delivered => OrderStatus.Delivered,
                DeliveryStatus.Cancelled => OrderStatus.Cancelled,
                _ => delivery.Order.Status
            };
        }

        await dbContext.SaveChangesAsync();
    }
}
