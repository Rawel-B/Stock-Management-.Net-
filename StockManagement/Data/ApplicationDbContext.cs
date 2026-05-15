using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StockManagement.Models;

namespace StockManagement.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : IdentityDbContext<ApplicationUser>(options)
{
    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Supplier> Suppliers => Set<Supplier>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderLine> OrderLines => Set<OrderLine>();
    public DbSet<Transporter> Transporters => Set<Transporter>();
    public DbSet<Delivery> Deliveries => Set<Delivery>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Client>()
            .HasIndex(client => client.Email)
            .IsUnique();

        builder.Entity<Product>()
            .HasIndex(product => product.Sku)
            .IsUnique();

        builder.Entity<Order>()
            .HasOne(order => order.Client)
            .WithMany(client => client.Orders)
            .HasForeignKey(order => order.ClientId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<OrderLine>()
            .HasOne(line => line.Order)
            .WithMany(order => order.Lines)
            .HasForeignKey(line => line.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<OrderLine>()
            .HasOne(line => line.Product)
            .WithMany(product => product.OrderLines)
            .HasForeignKey(line => line.ProductId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Delivery>()
            .HasOne(delivery => delivery.Order)
            .WithOne(order => order.Delivery)
            .HasForeignKey<Delivery>(delivery => delivery.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Delivery>()
            .HasOne(delivery => delivery.Transporter)
            .WithMany(transporter => transporter.Deliveries)
            .HasForeignKey(delivery => delivery.TransporterId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Payment>()
            .HasOne(payment => payment.Order)
            .WithOne(order => order.Payment)
            .HasForeignKey<Payment>(payment => payment.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
