namespace WarehouseManagement.Api.Models;

public class ProductImage
{
    public Guid ProductImageId { get; set; }

    public Guid ProductId { get; set; }

    public string FileName { get; set; } = string.Empty;

    public string FilePath { get; set; } = string.Empty;

    public Product Product { get; set; } = null!;
}