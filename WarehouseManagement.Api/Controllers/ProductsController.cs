using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Api.Data;
using WarehouseManagement.Api.Models;
using WarehouseManagement.Api.Contracts;
using System.Globalization;

namespace WarehouseManagement.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{

    private readonly ILogger<ProductsController> _logger;

    public ProductsController(ILogger<ProductsController> logger)
    {
        _logger = logger;
    }


    [HttpGet]
    public ActionResult<IEnumerable<Product>> GetAll([FromQuery] bool onlyAvailable = false)
    {
        var products = FakeWarehouseStore.Products.AsEnumerable();

        if (onlyAvailable)
        {
            products = products.Where(p => p.QuantityInStock > 0);
        }

        products = products.OrderByDescending(p => p.CreatedAt);

        return Ok(products);
    }

    [HttpGet("{id}")]
    public ActionResult<Product> GetById([FromRoute] Guid id)
    {
        var product = FakeWarehouseStore.Products
            .FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(product);
    }

    [HttpGet("search")]
    public ActionResult<IEnumerable<Product>> Search(
    [FromQuery] string? name,
    [FromQuery] string? supplier)
    {
        if (string.IsNullOrWhiteSpace(name) &&
            string.IsNullOrWhiteSpace(supplier))
        {
            return BadRequest("Please provide a name or supplier.");
        }

        var products = FakeWarehouseStore.Products.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(name))
        {
            products = products.Where(p =>
                p.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(supplier))
        {
            products = products.Where(p =>
                p.SupplierName.Contains(
                    supplier,
                    StringComparison.OrdinalIgnoreCase));
        }

        return Ok(products);
    }

    [HttpPost]
    public ActionResult<Product> Create([FromBody] CreateProductRequest request)
    {
        var duplicateSku = FakeWarehouseStore.Products
            .Any(p => p.SKU.Equals(
                request.SKU,
                StringComparison.OrdinalIgnoreCase));

        if (duplicateSku)
        {
            return BadRequest("A product with this SKU already exists.");
        }

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            SKU = request.SKU,
            Description = request.Description,
            Price = request.Price,
            QuantityInStock = request.QuantityInStock,
            SupplierName = request.SupplierName,
            ExpiryDate = request.ExpiryDate,
            IsArchived = false,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = null
        };

        FakeWarehouseStore.Products.Add(product);

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            product);
    }

    [HttpPost("{id}/quantity")]
    public ActionResult<Product> UpdateQuantity(
    [FromRoute] Guid id,
    [FromBody] UpdateProductQuantityRequest request)
    {
        if (request.QuantityInStock < 0)
        {
            return BadRequest("Quantity cannot be negative.");
        }

        var product = FakeWarehouseStore.Products
            .FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        product.QuantityInStock = request.QuantityInStock;
        product.LastUpdatedAt = DateTime.UtcNow;

        return Ok(product);
    }

    [HttpPost("{id}/price")]
    public ActionResult<Product> UpdatePrice(
    [FromRoute] Guid id,
    [FromBody] UpdateProductPriceRequest request)
    {
        if (request.Price <= 0)
        {
            return BadRequest("Price must be greater than 0.");
        }

        var product = FakeWarehouseStore.Products
            .FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        var oldPrice = product.Price;

        product.Price = request.Price;
        product.LastUpdatedAt = DateTime.UtcNow;

        _logger.LogInformation(
            "Product {ProductId} price changed from {OldPrice} to {NewPrice}",
            product.Id,
            oldPrice,
            product.Price);

        return Ok(product);
    }

    [HttpPost("{id}/image")]
    [Consumes("multipart/form-data")]
    public async Task<ActionResult<ProductImage>> UploadImage(
    [FromRoute] Guid id,
    [FromForm] UploadProductImageRequest request)
    {
        var file = request.File;

        var product = FakeWarehouseStore.Products
            .FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        if (file == null || file.Length == 0)
        {
            return BadRequest("No file was uploaded.");
        }

        if (file.Length > 2 * 1024 * 1024)
        {
            return BadRequest("File size cannot exceed 2 MB.");
        }

        var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

        if (extension != ".jpg" &&
            extension != ".jpeg" &&
            extension != ".png")
        {
            return BadRequest("Only JPG and PNG files are allowed.");
        }

        var uploadsFolder = Path.Combine(
            Directory.GetCurrentDirectory(),
            "wwwroot",
            "uploads");

        Directory.CreateDirectory(uploadsFolder);

        var fileName = $"{Guid.NewGuid()}{extension}";

        var filePath = Path.Combine(
            uploadsFolder,
            fileName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var productImage = new ProductImage
        {
            ProductId = product.Id,
            FileName = fileName,
            FilePath = $"/uploads/{fileName}"
        };

        return Ok(productImage);
    }

    [HttpDelete("{id}")]
    public ActionResult Delete([FromRoute] Guid id)
    {
        var product = FakeWarehouseStore.Products
            .FirstOrDefault(p => p.Id == id);

        if (product == null)
        {
            return NotFound();
        }

        product.IsArchived = true;
        product.LastUpdatedAt = DateTime.UtcNow;

        return NoContent();
    }


    [HttpGet("server-time")]
    public ActionResult GetServerTime(
    [FromHeader(Name = "Accept-Language")] string? language)
    {
        var supportedLanguages = new[] { "en-US", "fr-FR", "ar-LB" };

        if (string.IsNullOrWhiteSpace(language) ||
            !supportedLanguages.Contains(language))
        {
            language = "en-US";
        }

        var culture = new CultureInfo(language);

        var formattedDate = DateTime.Now.ToString(
            "F",
            culture);

        return Ok(new
        {
            language,
            serverTime = formattedDate
        });
    }

}