using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Api.Data;
using WarehouseManagement.Api.Models;

namespace WarehouseManagement.Api.Controllers;

public class ODataSuppliersController : ODataController
{
    private readonly WarehouseDbContext _context;

    public ODataSuppliersController(
        WarehouseDbContext context)
    {
        _context = context;
    }

    [EnableQuery]
    public IQueryable<Supplier> Get()
    {
        return _context.Suppliers;
    }

    [EnableQuery]
    public async Task<ActionResult<Supplier>> Get(Guid key)
    {
        var supplier = await _context.Suppliers
            .FirstOrDefaultAsync(
                s => s.SupplierId == key);

        if (supplier == null)
        {
            return NotFound();
        }

        return Ok(supplier);
    }

    public async Task<IActionResult> Post(
        [FromBody] Supplier? supplier)
    {
        if (supplier == null)
        {
            return BadRequest("Supplier data is required.");
        }

        if (string.IsNullOrWhiteSpace(supplier.Name))
        {
            return BadRequest("Supplier name is required.");
        }

        var exists = await _context.Suppliers
            .AnyAsync(s => s.Name == supplier.Name);

        if (exists)
        {
            return BadRequest(
                "A supplier with this name already exists.");
        }

        supplier.SupplierId = Guid.NewGuid();

        _context.Suppliers.Add(supplier);

        await _context.SaveChangesAsync();

        return Created(supplier);
    }

    public async Task<IActionResult> Patch(
        Guid key,
        [FromBody] Delta<Supplier> delta)
    {
        var supplier = await _context.Suppliers
            .FirstOrDefaultAsync(
                s => s.SupplierId == key);

        if (supplier == null)
        {
            return NotFound();
        }

        delta.Patch(supplier);

        await _context.SaveChangesAsync();

        return Updated(supplier);
    }
}