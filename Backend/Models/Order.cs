using System.ComponentModel.DataAnnotations.Schema;

namespace StockManagement.Models;

public class Order
{
    public int Id { get; set; }

    public int ClientId { get; set; }
    public Client? Client { get; set; }

    public DateTime Date { get; set; } = DateTime.UtcNow;

    public OrderStatus Status { get; set; } = OrderStatus.Draft;

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalAmount { get; set; }

    public List<OrderLine> Lines { get; set; } = [];
    public Delivery? Delivery { get; set; }
    public Payment? Payment { get; set; }
}
