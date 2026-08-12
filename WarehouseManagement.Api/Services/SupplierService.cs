using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Api.Data;
using WarehouseManagement.Api.Models;

namespace WarehouseManagement.Api.Services;

public class SupplierService
{
    private readonly WarehouseDbContext _context;

    public SupplierService(WarehouseDbContext context)
    {
        _context = context;
    }

    public async Task<List<Supplier>> GetAllAsync()
    {
        return await _context.Suppliers
            .Include(s => s.Products)
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task<Supplier?> GetByIdAsync(Guid id)
    {
        return await _context.Suppliers
            .Include(s => s.Products)
            .FirstOrDefaultAsync(
                s => s.SupplierId == id);
    }

    public async Task<Supplier> AddAsync(Supplier supplier)
    {
        _context.Suppliers.Add(supplier);

        await _context.SaveChangesAsync();

        return supplier;
    }

    public async Task<bool> ExistsByNameAsync(string name)
    {
        return await _context.Suppliers
            .AnyAsync(s => s.Name == name);
    }

    public async Task<Supplier?> UpdateAsync(
        Guid id,
        Supplier updatedSupplier)
    {
        var supplier = await _context.Suppliers
            .FirstOrDefaultAsync(
                s => s.SupplierId == id);

        if (supplier == null)
        {
            return null;
        }

        supplier.Name = updatedSupplier.Name;
        supplier.Country = updatedSupplier.Country;
        supplier.ContactEmail = updatedSupplier.ContactEmail;
        supplier.PhoneNumber = updatedSupplier.PhoneNumber;
        supplier.IsActive = updatedSupplier.IsActive;

        await _context.SaveChangesAsync();

        return supplier;
    }
}