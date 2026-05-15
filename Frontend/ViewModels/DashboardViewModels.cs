using StockManagement.Models;

namespace StockManagement.ViewModels;

public class DashboardViewModel
{
    public int OpenOrders { get; set; }
    public int LowStockProducts { get; set; }
    public int PendingPayments { get; set; }
    public decimal Revenue { get; set; }
    public List<DashboardOrderViewModel> RecentOrders { get; set; } = [];
}

public class DashboardOrderViewModel
{
    public int Id { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime Date { get; set; }
}
