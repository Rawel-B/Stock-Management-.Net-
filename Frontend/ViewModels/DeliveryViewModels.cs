using StockManagement.Models;

namespace StockManagement.ViewModels;

public class DeliveryPageViewModel
{
    public List<SelectOptionViewModel> ValidatedOrders { get; set; } = [];
    public List<SelectOptionViewModel> Transporters { get; set; } = [];
    public List<DeliveryViewModel> Deliveries { get; set; } = [];
}

public class DeliveryEditModel
{
    public int OrderId { get; set; }
    public int TransporterId { get; set; }
    public DateTime DeliveryDate { get; set; } = DateTime.UtcNow.AddDays(2);
    public decimal Cost { get; set; }
}

public class DeliveryViewModel
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string ClientName { get; set; } = string.Empty;
    public string TransporterName { get; set; } = string.Empty;
    public DateTime DeliveryDate { get; set; }
    public decimal Cost { get; set; }
    public DeliveryStatus Status { get; set; }
}
