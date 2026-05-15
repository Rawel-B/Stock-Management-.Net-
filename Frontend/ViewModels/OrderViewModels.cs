using StockManagement.Models;

namespace StockManagement.ViewModels;

public class OrderPageViewModel
{
    public List<SelectOptionViewModel> Clients { get; set; } = [];
    public List<OrderProductOptionViewModel> Products { get; set; } = [];
    public List<OrderViewModel> Orders { get; set; } = [];
}

public class OrderProductOptionViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public decimal UnitPrice { get; set; }
}

public class OrderViewModel
{
    public int Id { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public OrderStatus Status { get; set; }
    public decimal TotalAmount { get; set; }
    public DateTime Date { get; set; }
    public List<OrderLineViewModel> Lines { get; set; } = [];
}

public class OrderLineViewModel
{
    public string ProductName { get; set; } = string.Empty;
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
