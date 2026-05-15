namespace StockManagement.ViewModels;

public class ProductViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int? SupplierId { get; set; }
    public string? SupplierName { get; set; }
}

public class ProductEditModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public int StockQuantity { get; set; }
    public decimal UnitPrice { get; set; }
    public int? SupplierId { get; set; }
}

public class SelectOptionViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
}
