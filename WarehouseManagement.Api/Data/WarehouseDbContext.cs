using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Api.Models;

namespace WarehouseManagement.Api.Data;

public class WarehouseDbContext : DbContext
{
    public WarehouseDbContext(
        DbContextOptions<WarehouseDbContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; }

    public DbSet<Supplier> Suppliers { get; set; }

    public DbSet<ProductImage> ProductImages { get; set; }
}