using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Deltas;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Api.Contracts;
using WarehouseManagement.Api.Data;
using WarehouseManagement.Api.Models;

namespace WarehouseManagement.Api.Controllers;

public class ODataProductsController : ODataController
{
    private readonly WarehouseDbContext _context;

    public ODataProductsController(
        WarehouseDbContext context)
    {
        _context = context;
    }

    [EnableQuery]
    public IQueryable<Product> Get()
    {
        return _context.Products;
    }

    [EnableQuery]
    public async Task<ActionResult<Product>> Get(Guid key)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == key);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    public async Task<IActionResult> Post(
        [FromBody] Product? product)
    {
        if (product == null)
        {
            return BadRequest(
                "Product data is required.");
        }

        if (string.IsNullOrWhiteSpace(product.Name))
        {
            return BadRequest(
                "Product name is required.");
        }

        if (string.IsNullOrWhiteSpace(product.SKU))
        {
            return BadRequest(
                "SKU is required.");
        }

        if (product.Price <= 0)
        {
            return BadRequest(
                "Price must be greater than 0.");
        }

        if (product.QuantityInStock < 0)
        {
            return BadRequest(
                "Quantity cannot be negative.");
        }

        var duplicateSku = await _context.Products
            .AnyAsync(p => p.SKU == product.SKU);

        if (duplicateSku)
        {
            return BadRequest(
                "A product with this SKU already exists.");
        }

        product.Id = Guid.NewGuid();
        product.CreatedAt = DateTime.UtcNow;
        product.LastUpdatedAt = null;
        product.IsArchived = false;
        product.SupplierId = null;
        product.Supplier = null;

        if (product.ExpiryDate.HasValue)
        {
            product.ExpiryDate =
                product.ExpiryDate.Value.ToUniversalTime();
        }

        _context.Products.Add(product);

        await _context.SaveChangesAsync();

        return Created(product);
    }

    public async Task<IActionResult> Patch(
        Guid key,
        [FromBody] Delta<Product> delta)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == key);

        if (product == null)
        {
            return NotFound();
        }

        delta.Patch(product);

        if (product.Price <= 0)
        {
            return BadRequest(
                "Price must be greater than 0.");
        }

        if (product.QuantityInStock < 0)
        {
            return BadRequest(
                "Quantity cannot be negative.");
        }

        if (product.ExpiryDate.HasValue)
        {
            product.ExpiryDate =
                product.ExpiryDate.Value.ToUniversalTime();
        }

        product.LastUpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Updated(product);
    }

    public async Task<IActionResult> Delete(Guid key)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == key);

        if (product == null)
        {
            return NotFound();
        }

        product.IsArchived = true;
        product.LastUpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpPost(
        "odata/ODataProducts({key})/assign-supplier/{supplierId}")]
    public async Task<IActionResult> AssignSupplier(
        [FromRoute] Guid key,
        [FromRoute] Guid supplierId)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == key);

        if (product == null)
        {
            return NotFound(
                "Product not found.");
        }

        if (product.IsArchived)
        {
            return BadRequest(
                "Archived products cannot be assigned to a supplier.");
        }

        var supplier = await _context.Suppliers
            .FirstOrDefaultAsync(
                s => s.SupplierId == supplierId);

        if (supplier == null)
        {
            return NotFound(
                "Supplier not found.");
        }

        product.SupplierId = supplier.SupplierId;
        product.Supplier = supplier;
        product.LastUpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(new
        {
            product.Id,
            product.Name,
            product.SupplierId,
            SupplierName = supplier.Name
        });
    }

    [HttpPost(
        "odata/ODataProducts({key})/image")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadImage(
        [FromRoute] Guid key,
        [FromForm] UploadProductImageRequest request)
    {
        var product = await _context.Products
            .FirstOrDefaultAsync(p => p.Id == key);

        if (product == null)
        {
            return NotFound();
        }

        var file = request.File;

        if (file == null || file.Length == 0)
        {
            return BadRequest(
                "No file was uploaded.");
        }

        if (file.Length > 2 * 1024 * 1024)
        {
            return BadRequest(
                "File size cannot exceed 2 MB.");
        }

        var extension = Path
            .GetExtension(file.FileName)
            .ToLowerInvariant();

        if (extension != ".jpg" &&
            extension != ".jpeg" &&
            extension != ".png")
        {
            return BadRequest(
                "Only JPG and PNG files are allowed.");
        }

        var uploadsFolder = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            "uploads");

        Directory.CreateDirectory(
            uploadsFolder);

        var fileName =
            $"{Guid.NewGuid()}{extension}";

        var filePath = Path.Combine(
            uploadsFolder,
            fileName);

        using (var stream =
            new FileStream(
                filePath,
                FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var image = new ProductImage
        {
            ProductImageId = Guid.NewGuid(),
            ProductId = product.Id,
            FileName = fileName,
            FilePath = $"/uploads/{fileName}",
            Product = product
        };

        _context.ProductImages.Add(image);

        await _context.SaveChangesAsync();

        return Ok(new
        {
            image.ProductImageId,
            image.ProductId,
            image.FileName,
            image.FilePath
        });
    }
}