using System.ComponentModel.DataAnnotations;

namespace StockManagement.Models;

public class Transporter
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required, Phone, StringLength(40)]
    public string Phone { get; set; } = string.Empty;

    [Range(1, 5)]
    public int Rating { get; set; } = 4;

    public List<Delivery> Deliveries { get; set; } = [];
}
