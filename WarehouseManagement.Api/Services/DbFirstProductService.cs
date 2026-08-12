using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Api.Data.DbFirst;

namespace WarehouseManagement.Api.Services;

public class DbFirstProductService
{
    private readonly WarehouseDbFirstContext _context;

    public DbFirstProductService(WarehouseDbFirstContext context)
    {
        _context = context;
    }

    public async Task<object> GetProductsBySupplierAsync(
        string supplierName,
        bool ascending = true)
    {
        var query = _context.Products
            .Where(p =>
                p.Supplier != null &&
                p.Supplier.Name == supplierName);

        query = ascending
            ? query.OrderBy(p => p.CreatedAt)
            : query.OrderByDescending(p => p.CreatedAt);

        return await query
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Sku,
                p.Description,
                p.Price,
                p.QuantityInStock,
                p.ExpiryDate,
                p.IsArchived,
                p.CreatedAt,
                Supplier = new
                {
                    p.Supplier!.SupplierId,
                    p.Supplier.Name,
                    p.Supplier.Country
                }
            })
            .ToListAsync();
    }

    public async Task<object> GetProductsGroupedByExpiryYearAsync()
    {
        return await _context.Products
            .Where(p => p.ExpiryDate != null)
            .GroupBy(p => p.ExpiryDate!.Value.Year)
            .Select(group => new
            {
                ExpiryYear = group.Key,
                ProductCount = group.Count(),
                Products = group.Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Sku,
                    p.ExpiryDate
                })
            })
            .OrderBy(group => group.ExpiryYear)
            .ToListAsync();
    }

    public async Task<object> GetProductsGroupedByExpiryYearAndCountryAsync()
    {
        return await _context.Products
            .Where(p =>
                p.ExpiryDate != null &&
                p.Supplier != null)
            .GroupBy(p => new
            {
                ExpiryYear = p.ExpiryDate!.Value.Year,
                Country = p.Supplier!.Country
            })
            .Select(group => new
            {
                group.Key.ExpiryYear,
                group.Key.Country,
                ProductCount = group.Count(),
                Products = group.Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Sku,
                    p.ExpiryDate
                })
            })
            .OrderBy(group => group.ExpiryYear)
            .ThenBy(group => group.Country)
            .ToListAsync();
    }

    public async Task<int> GetTotalProductCountAsync()
    {
        return await _context.Products.CountAsync();
    }

    public async Task<object> GetPagedProductsAsync(
        int pageNumber,
        int pageSize)
    {
        var totalProducts = await _context.Products.CountAsync();

        var products = await _context.Products
            .OrderBy(p => p.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .Select(p => new
            {
                p.Id,
                p.Name,
                p.Sku,
                p.Price,
                p.QuantityInStock,
                p.ExpiryDate,
                p.CreatedAt
            })
            .ToListAsync();

        return new
        {
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalProducts = totalProducts,
            TotalPages = (int)Math.Ceiling(
                totalProducts / (double)pageSize
            ),
            Products = products
        };
    }
}