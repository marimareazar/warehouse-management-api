using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Api.Models.DbFirst;

namespace WarehouseManagement.Api.Data.DbFirst;

public partial class WarehouseDbFirstContext : DbContext
{
    public WarehouseDbFirstContext()
    {
    }

    public WarehouseDbFirstContext(DbContextOptions<WarehouseDbFirstContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductImage> ProductImages { get; set; }

    public virtual DbSet<Supplier> Suppliers { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("Products_pkey");

            entity.HasIndex(e => e.Sku, "Products_SKU_key").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.ExpiryDate).HasColumnType("timestamp without time zone");
            entity.Property(e => e.IsArchived).HasDefaultValue(false);
            entity.Property(e => e.LastUpdatedAt).HasColumnType("timestamp without time zone");
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.Price).HasPrecision(18, 2);
            entity.Property(e => e.Sku)
                .HasMaxLength(100)
                .HasColumnName("SKU");

            entity.HasOne(d => d.Supplier).WithMany(p => p.Products)
                .HasForeignKey(d => d.SupplierId)
                .HasConstraintName("FK_Products_Suppliers");
        });

        modelBuilder.Entity<ProductImage>(entity =>
        {
            entity.HasKey(e => e.ProductImageId).HasName("ProductImages_pkey");

            entity.Property(e => e.ProductImageId).ValueGeneratedNever();
            entity.Property(e => e.FileName).HasMaxLength(255);
            entity.Property(e => e.FilePath).HasMaxLength(500);

            entity.HasOne(d => d.Product).WithMany(p => p.ProductImages)
                .HasForeignKey(d => d.ProductId)
                .HasConstraintName("FK_ProductImages_Products");
        });

        modelBuilder.Entity<Supplier>(entity =>
        {
            entity.HasKey(e => e.SupplierId).HasName("Suppliers_pkey");

            entity.Property(e => e.SupplierId).ValueGeneratedNever();
            entity.Property(e => e.ContactEmail).HasMaxLength(200);
            entity.Property(e => e.Country).HasMaxLength(100);
            entity.Property(e => e.IsActive).HasDefaultValue(true);
            entity.Property(e => e.Name).HasMaxLength(150);
            entity.Property(e => e.PhoneNumber).HasMaxLength(50);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
