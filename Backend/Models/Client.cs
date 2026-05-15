using System.ComponentModel.DataAnnotations;

namespace StockManagement.Models;

public class Client
{
    public int Id { get; set; }

    [Required, StringLength(120)]
    public string Name { get; set; } = string.Empty;

    [Required, EmailAddress, StringLength(180)]
    public string Email { get; set; } = string.Empty;

    [Required, StringLength(260)]
    public string Address { get; set; } = string.Empty;

    public List<Order> Orders { get; set; } = [];
}
