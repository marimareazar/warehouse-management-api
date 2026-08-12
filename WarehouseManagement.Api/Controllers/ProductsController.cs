using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Api.Models;
using WarehouseManagement.Api.Contracts;
using WarehouseManagement.Api.Services;
using System.Globalization;
using AutoMapper;
using WarehouseManagement.Api.ViewModels;

namespace WarehouseManagement.Api.Controllers;

[ApiController]
[Route("api/products")]
public class ProductsController : ControllerBase
{
    private readonly ProductService _productService;
    private readonly ILogger<ProductsController> _logger;
    private readonly IMapper _mapper;

    public ProductsController(
        ProductService productService,
        ILogger<ProductsController> logger,
        IMapper mapper)
    {
        _productService = productService;
        _logger = logger;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ProductViewModel>>> GetAll(
    [FromQuery] bool onlyAvailable = false)
    {
        var products = await _productService.GetAllAsync();

        if (onlyAvailable)
        {
            products = products
                .Where(p => p.QuantityInStock > 0)
                .ToList();
        }

        var result =
            _mapper.Map<List<ProductViewModel>>(products);

        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ProductViewModel>> GetById(
     [FromRoute] Guid id)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        var result =
            _mapper.Map<ProductViewModel>(product);

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProductViewModel>> Create(
    [FromBody] CreateProductRequest request)
    {
        var duplicateSku =
            await _productService.SkuExistsAsync(request.SKU);

        if (duplicateSku)
        {
            return BadRequest(
                "A product with this SKU already exists.");
        }

        var product = new Product
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            SKU = request.SKU,
            Description = request.Description,
            Price = request.Price,
            QuantityInStock = request.QuantityInStock,
            ExpiryDate = request.ExpiryDate,
            IsArchived = false,
            CreatedAt = DateTime.UtcNow,
            LastUpdatedAt = null
        };

        await _productService.AddAsync(product);

        var result =
            _mapper.Map<ProductViewModel>(product);

        return CreatedAtAction(
            nameof(GetById),
            new { id = product.Id },
            result);
    }

    [HttpPost("{id}/quantity")]
    public async Task<ActionResult<ProductViewModel>> UpdateQuantity(
    [FromRoute] Guid id,
    [FromBody] UpdateProductQuantityRequest request)
    {
        if (request.QuantityInStock < 0)
        {
            return BadRequest(
                "Quantity cannot be negative.");
        }

        var product =
            await _productService.UpdateQuantityAsync(
                id,
                request.QuantityInStock);

        if (product == null)
        {
            return NotFound();
        }

        return Ok(
            _mapper.Map<ProductViewModel>(product)
        );
    }

    [HttpPost("{id}/price")]
    public async Task<ActionResult<ProductViewModel>> UpdatePrice(
    [FromRoute] Guid id,
    [FromBody] UpdateProductPriceRequest request)
    {
        if (request.Price <= 0)
        {
            return BadRequest(
                "Price must be greater than 0.");
        }

        var existingProduct =
            await _productService.GetByIdAsync(id);

        if (existingProduct == null)
        {
            return NotFound();
        }

        var oldPrice = existingProduct.Price;

        var product =
            await _productService.UpdatePriceAsync(
                id,
                request.Price);

        _logger.LogInformation(
            "Product {ProductId} price changed from {OldPrice} to {NewPrice}",
            id,
            oldPrice,
            request.Price);

        return Ok(
            _mapper.Map<ProductViewModel>(product)
        );
    }

    [HttpPost("{id}/image")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadImage(
    [FromRoute] Guid id,
    [FromForm] UploadProductImageRequest request)
    {
        var product = await _productService.GetByIdAsync(id);

        if (product == null)
        {
            return NotFound();
        }

        var file = request.File;

        if (file == null || file.Length == 0)
        {
            return BadRequest("No file was uploaded.");
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

        Directory.CreateDirectory(uploadsFolder);

        var fileName =
            $"{Guid.NewGuid()}{extension}";

        var filePath = Path.Combine(
            uploadsFolder,
            fileName);

        using (var stream =
            new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var productImage = new ProductImage
        {
            ProductImageId = Guid.NewGuid(),
            ProductId = id,
            FileName = fileName,
            FilePath = $"/uploads/{fileName}",
            Product = product
        };

        var savedImage =
            await _productService.AddImageAsync(
                id,
                productImage);

        if (savedImage == null)
        {
            return NotFound();
        }

        return Ok(new
        {
            savedImage.ProductImageId,
            savedImage.ProductId,
            savedImage.FileName,
            savedImage.FilePath
        });
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(
        [FromRoute] Guid id)
    {
        var archived =
            await _productService.ArchiveAsync(id);

        if (!archived)
        {
            return NotFound();
        }

        return NoContent();
    }

    [HttpPost("{id}/assign-supplier/{supplierId}")]
    public async Task<ActionResult<ProductViewModel>> AssignSupplier(
    [FromRoute] Guid id,
    [FromRoute] Guid supplierId)
    {
        var product =
            await _productService.AssignSupplierAsync(
                id,
                supplierId);

        if (product == null)
        {
            return NotFound(
                "Product or supplier not found.");
        }

        return Ok(
            _mapper.Map<ProductViewModel>(product)
        );
    }

    [HttpGet("server-time")]
    public ActionResult GetServerTime(
        [FromHeader(Name = "Accept-Language")]
        string? language)
    {
        var supportedLanguages =
            new[]
            {
                "en-US",
                "fr-FR",
                "ar-LB"
            };

        if (string.IsNullOrWhiteSpace(language) ||
            !supportedLanguages.Contains(language))
        {
            language = "en-US";
        }

        var culture =
            new CultureInfo(language);

        var formattedDate =
            DateTime.Now.ToString(
                "F",
                culture);

        return Ok(new
        {
            language,
            serverTime = formattedDate
        });
    }
}