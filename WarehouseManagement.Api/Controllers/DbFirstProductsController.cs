using Microsoft.AspNetCore.Mvc;
using WarehouseManagement.Api.Services;

namespace WarehouseManagement.Api.Controllers;

[ApiController]
[Route("api/db-first/products")]
public class DbFirstProductsController : ControllerBase
{
    private readonly DbFirstProductService _productService;

    public DbFirstProductsController(
        DbFirstProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("supplier")]
    public async Task<IActionResult> GetProductsBySupplier(
        [FromQuery] string supplierName,
        [FromQuery] bool ascending = true)
    {
        var products =
            await _productService.GetProductsBySupplierAsync(
                supplierName,
                ascending
            );

        return Ok(products);
    }

    [HttpGet("group-by-expiry-year")]
    public async Task<IActionResult> GroupByExpiryYear()
    {
        var result =
            await _productService
                .GetProductsGroupedByExpiryYearAsync();

        return Ok(result);
    }

    [HttpGet("group-by-expiry-year-country")]
    public async Task<IActionResult>
        GroupByExpiryYearAndCountry()
    {
        var result =
            await _productService
                .GetProductsGroupedByExpiryYearAndCountryAsync();

        return Ok(result);
    }

    [HttpGet("count")]
    public async Task<IActionResult> GetTotalProductCount()
    {
        var count =
            await _productService.GetTotalProductCountAsync();

        return Ok(new
        {
            TotalProducts = count
        });
    }

    [HttpGet("paged")]
    public async Task<IActionResult> GetPagedProducts(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10)
    {
        if (pageNumber < 1)
        {
            return BadRequest(
                "Page number must be greater than 0."
            );
        }

        if (pageSize < 1)
        {
            return BadRequest(
                "Page size must be greater than 0."
            );
        }

        var result =
            await _productService.GetPagedProductsAsync(
                pageNumber,
                pageSize
            );

        return Ok(result);
    }
}