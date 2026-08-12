using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Api.Models;
using WarehouseManagement.Api.Services;
using AutoMapper;
using WarehouseManagement.Api.ViewModels;
using WarehouseManagement.Api.Contracts;

namespace WarehouseManagement.Api.Controllers;

[ApiController]
[Route("api/suppliers")]
public class SuppliersController : ControllerBase
{
    private readonly SupplierService _supplierService;
    private readonly IMapper _mapper;

    public SuppliersController(
        SupplierService supplierService,
        IMapper mapper)
    {
        _supplierService = supplierService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<SupplierViewModel>>> GetAll()
    {
        var suppliers =
            await _supplierService.GetAllAsync();

        var result =
            _mapper.Map<List<SupplierViewModel>>(suppliers);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<SupplierViewModel>> GetById(
    [FromRoute] Guid id)
    {
        var supplier =
            await _supplierService.GetByIdAsync(id);

        if (supplier == null)
        {
            return NotFound();
        }

        return Ok(
            _mapper.Map<SupplierViewModel>(supplier)
        );
    }

    [HttpPost]
    public async Task<ActionResult<SupplierViewModel>> Create(
    [FromBody] CreateSupplierRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest(
                "Supplier name is required.");
        }

        var exists =
            await _supplierService.ExistsByNameAsync(
                request.Name);

        if (exists)
        {
            return BadRequest(
                "A supplier with this name already exists.");
        }

        var supplier = new Supplier
        {
            SupplierId = Guid.NewGuid(),
            Name = request.Name,
            Country = request.Country,
            ContactEmail = request.ContactEmail,
            PhoneNumber = request.PhoneNumber,
            IsActive = request.IsActive
        };

        await _supplierService.AddAsync(supplier);

        var result =
            _mapper.Map<SupplierViewModel>(supplier);

        return CreatedAtAction(
            nameof(GetById),
            new { id = supplier.SupplierId },
            result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SupplierViewModel>> Update(
    [FromRoute] Guid id,
    [FromBody] Supplier request)
    {
        var supplier =
            await _supplierService.UpdateAsync(
                id,
                request);

        if (supplier == null)
        {
            return NotFound();
        }

        return Ok(
            _mapper.Map<SupplierViewModel>(supplier)
        );
    }
}