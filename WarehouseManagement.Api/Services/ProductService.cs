using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Api.Data;
using WarehouseManagement.Api.Models;

namespace WarehouseManagement.Api.Services;

public class ProductService
{
    private readonly WarehouseDbContext _context;

    public ProductService(WarehouseDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetAllAsync()
    {
        return await _context.Products
            .Include(p => p.Supplier)
            .Include(p => p.Images)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();
    }

    public async Task<Product?> GetByIdAsync(Guid id)
    {
        return await _context.Products
            .Include(p => p.Supplier)
            .Include(p => p.Images)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<bool> SkuExistsAsync(string sku)
    {
        return await _context.Products
            .AnyAsync(p => p.SKU == sku);
    }

    public async Task<Product> AddAsync(Product product)
    {
        _context.Products.Add(product);

        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<Product?> UpdatePriceAsync(
        Guid id,
        decimal price)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return null;
        }

        product.Price = price;
        product.LastUpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<Product?> UpdateQuantityAsync(
        Guid id,
        int quantity)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return null;
        }

        product.QuantityInStock = quantity;
        product.LastUpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<bool> ArchiveAsync(Guid id)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == id);

        if (product == null)
        {
            return false;
        }

        product.IsArchived = true;
        product.LastUpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<Product?> AssignSupplierAsync(
    Guid productId,
    Guid supplierId)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == productId);

        if (product == null || product.IsArchived)
        {
            return null;
        }

        var supplier = await _context.Suppliers
            .FirstOrDefaultAsync(
                s => s.SupplierId == supplierId);

        if (supplier == null)
        {
            return null;
        }

        product.SupplierId = supplier.SupplierId;
        product.Supplier = supplier;
        product.LastUpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return product;
    }

    public async Task<ProductImage?> AddImageAsync(
        Guid productId,
        ProductImage productImage)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == productId);

        if (product == null)
        {
            return null;
        }

        productImage.ProductId = productId;
        productImage.Product = product;

        _context.ProductImages.Add(productImage);

        await _context.SaveChangesAsync();

        return productImage;
    }
}