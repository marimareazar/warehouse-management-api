namespace WarehouseManagement.Api.ViewModels;

public class ProductViewModel
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string SKU { get; set; } = string.Empty;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int QuantityInStock { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public bool IsArchived { get; set; }

    public string? SupplierName { get; set; }

    public string? SupplierCountry { get; set; }
}