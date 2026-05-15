using System.ComponentModel.DataAnnotations;

namespace StockManagement.Models;

public class Supplier
{
    public int Id { get; set; }

    [Required, StringLength(140)]
    public string Name { get; set; } = string.Empty;

    [Phone, StringLength(40)]
    public string? Phone { get; set; }

    [EmailAddress, StringLength(180)]
    public string? Email { get; set; }

    public List<Product> Products { get; set; } = [];
}
