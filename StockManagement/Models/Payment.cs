namespace StockManagement.Models;

public class Payment
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order? Order { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public PaymentMode Mode { get; set; } = PaymentMode.OnlineGateway;
}
