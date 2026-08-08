using WarehouseManagement.Api.Models;

namespace WarehouseManagement.Api.Data;

public static class FakeWarehouseStore
{
    public static List<Product> Products { get; } =
    [
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Laptop",
            SKU = "LAP-001",
            Description = "Business laptop",
            Price = 1200,
            QuantityInStock = 10,
            SupplierName = "Tech Supplier",
            ExpiryDate = null,
            IsArchived = false,
            CreatedAt = DateTime.UtcNow
        },

        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Mouse",
            SKU = "MOU-001",
            Description = "Wireless mouse",
            Price = 25,
            QuantityInStock = 50,
            SupplierName = "Tech Supplier",
            ExpiryDate = null,
            IsArchived = false,
            CreatedAt = DateTime.UtcNow
        },

        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Keyboard",
            SKU = "KEY-001",
            Description = "Mechanical keyboard",
            Price = 80,
            QuantityInStock = 30,
            SupplierName = "Input Devices Ltd",
            ExpiryDate = null,
            IsArchived = false,
            CreatedAt = DateTime.UtcNow
        },

        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Scanner",
            SKU = "SCA-001",
            Description = "Office scanner",
            Price = 250,
            QuantityInStock = 8,
            SupplierName = "Office Supply Co",
            ExpiryDate = null,
            IsArchived = false,
            CreatedAt = DateTime.UtcNow
        },

        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Printer",
            SKU = "PRI-001",
            Description = "Laser printer",
            Price = 400,
            QuantityInStock = 15,
            SupplierName = "Office Supply Co",
            ExpiryDate = null,
            IsArchived = false,
            CreatedAt = DateTime.UtcNow
        },

        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Monitor",
            SKU = "MON-001",
            Description = "27 inch monitor",
            Price = 300,
            QuantityInStock = 20,
            SupplierName = "Display Supplier",
            ExpiryDate = null,
            IsArchived = false,
            CreatedAt = DateTime.UtcNow
        },

        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Webcam",
            SKU = "WEB-001",
            Description = "HD webcam",
            Price = 70,
            QuantityInStock = 25,
            SupplierName = "Tech Supplier",
            ExpiryDate = null,
            IsArchived = false,
            CreatedAt = DateTime.UtcNow
        },

        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Headset",
            SKU = "HEA-001",
            Description = "USB headset",
            Price = 60,
            QuantityInStock = 40,
            SupplierName = "Audio Supplier",
            ExpiryDate = null,
            IsArchived = false,
            CreatedAt = DateTime.UtcNow
        },

        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Router",
            SKU = "ROU-001",
            Description = "Wireless router",
            Price = 110,
            QuantityInStock = 12,
            SupplierName = "Network Supplier",
            ExpiryDate = null,
            IsArchived = false,
            CreatedAt = DateTime.UtcNow
        },

        new Product
        {
            Id = Guid.NewGuid(),
            Name = "USB Cable",
            SKU = "USB-001",
            Description = "USB-C cable",
            Price = 15,
            QuantityInStock = 100,
            SupplierName = "Cable Supplier",
            ExpiryDate = null,
            IsArchived = false,
            CreatedAt = DateTime.UtcNow
        }
    ];

    public static List<Supplier> Suppliers { get; } =
[
    new Supplier
    {
        Id = Guid.NewGuid(),
        Name = "Tech Supplier",
        Country = "Lebanon",
        ContactEmail = "contact@techsupplier.com",
        PhoneNumber = "+961123456",
        IsActive = true
    },

    new Supplier
    {
        Id = Guid.NewGuid(),
        Name = "Office Supply Co",
        Country = "France",
        ContactEmail = "contact@officesupply.com",
        PhoneNumber = "+33123456789",
        IsActive = true
    },

    new Supplier
    {
        Id = Guid.NewGuid(),
        Name = "Display Supplier",
        Country = "Germany",
        ContactEmail = "contact@displaysupplier.com",
        PhoneNumber = "+49123456789",
        IsActive = true
    }
];
}