using StockManagement.Models;

namespace StockManagement.ViewModels;

public class PaymentViewModel
{
    public int OrderId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public PaymentMode Mode { get; set; }
    public PaymentStatus Status { get; set; }
    public DateTime Date { get; set; }
}
