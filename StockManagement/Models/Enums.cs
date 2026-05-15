namespace StockManagement.Models;

public enum OrderStatus
{
    Draft,
    Validated,
    Shipped,
    Delivered,
    Cancelled
}

public enum DeliveryStatus
{
    Planned,
    InTransit,
    Delivered,
    Cancelled
}

public enum PaymentStatus
{
    Pending,
    Paid,
    Failed,
    Refunded
}

public enum PaymentMode
{
    Card,
    BankTransfer,
    Cash,
    MobileMoney,
    OnlineGateway
}
