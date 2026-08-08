using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Api.Contracts;
using WarehouseManagement.Api.Models;
using WarehouseManagement.Api.Services;

namespace WarehouseManagement.Api.Controllers;

[ApiController]
[Route("api/suppliers")]
public class SuppliersController : ControllerBase
{
    private readonly SupplierService _supplierService;

    public SuppliersController(SupplierService supplierService)
    {
        _supplierService = supplierService;
    }

    [HttpGet]
    public ActionResult<IEnumerable<Supplier>> GetAll()
    {
        var suppliers = _supplierService.GetAll();

        return Ok(suppliers);
    }

    [HttpGet("{id}")]
    public ActionResult<Supplier> GetById([FromRoute] Guid id)
    {
        var supplier = _supplierService.GetById(id);

        if (supplier == null)
        {
            return NotFound();
        }

        return Ok(supplier);
    }

    [HttpPost]
    public ActionResult<Supplier> Create(
        [FromBody] CreateSupplierRequest request)
    {
        var supplier = _supplierService.Create(request);

        return CreatedAtAction(
            nameof(GetById),
            new { id = supplier.Id },
            supplier);
    }

    [HttpDelete("{id}")]
    public ActionResult Deactivate([FromRoute] Guid id)
    {
        var success = _supplierService.Deactivate(id);

        if (!success)
        {
            return NotFound();
        }

        return NoContent();
    }
}