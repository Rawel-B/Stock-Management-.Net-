using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StockManagement.Models;

public class Product {
    public int Id { get; set; }

    [Required, StringLength(140)]
    public string Name { get; set; } = string.Empty;

    [StringLength(50)]
    public string Sku { get; set; } = string.Empty;

    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    [Range(0, 999999999)]
    public decimal UnitPrice { get; set; }

    public int? SupplierId { get; set; }
    public Supplier? Supplier { get; set; }

    public List<OrderLine> OrderLines { get; set; } = [];
}
