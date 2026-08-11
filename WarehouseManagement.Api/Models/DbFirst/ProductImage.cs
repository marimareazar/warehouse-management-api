using System;
using System.Collections.Generic;

namespace WarehouseManagement.Api.Models.DbFirst;

public partial class ProductImage
{
    public Guid ProductImageId { get; set; }

    public Guid ProductId { get; set; }

    public string FileName { get; set; } = null!;

    public string FilePath { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
