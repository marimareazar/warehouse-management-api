namespace WarehouseManagement.Api.Contracts;

public class UploadProductImageRequest
{
    public IFormFile File { get; set; } = null!;
}