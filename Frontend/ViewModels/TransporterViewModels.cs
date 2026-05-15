namespace StockManagement.ViewModels;

public class TransporterViewModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int Rating { get; set; }
    public int DeliveryCount { get; set; }
}

public class TransporterEditModel
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int Rating { get; set; } = 4;
}
