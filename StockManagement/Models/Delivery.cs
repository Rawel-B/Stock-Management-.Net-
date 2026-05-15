using System.ComponentModel.DataAnnotations.Schema;

namespace StockManagement.Models;

public class Delivery
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order? Order { get; set; }

    public int TransporterId { get; set; }
    public Transporter? Transporter { get; set; }

    public DateTime DeliveryDate { get; set; } = DateTime.UtcNow.AddDays(2);

    [Column(TypeName = "decimal(18,2)")]
    public decimal Cost { get; set; }

    public DeliveryStatus Status { get; set; } = DeliveryStatus.Planned;
}
