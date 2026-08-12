using Microsoft.AspNetCore.OData;
using Microsoft.EntityFrameworkCore;
using Microsoft.OData.ModelBuilder;
using WarehouseManagement.Api.Data;
using WarehouseManagement.Api.Mapping;
using WarehouseManagement.Api.Models;
using WarehouseManagement.Api.Services;

static Microsoft.OData.Edm.IEdmModel GetEdmModel()
{
    var builder = new ODataConventionModelBuilder();

    builder.EntitySet<Product>("ODataProducts");
    builder.EntitySet<Supplier>("ODataSuppliers");

    return builder.GetEdmModel();
}

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers()
    .AddOData(options =>
        options
            .Select()
            .Filter()
            .OrderBy()
            .Expand()
            .Count()
            .SetMaxTop(100)
            .AddRouteComponents(
                "odata",
                GetEdmModel()
            )
    );

builder.Services.AddOpenApi();

builder.Services.AddDbContext<WarehouseDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("WarehouseDb")
    )
);

builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<SupplierService>();

builder.Services.AddAutoMapper(
    cfg => { },
    typeof(MappingProfile)
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint(
            "/openapi/v1.json",
            "Warehouse Management API v1"
        );
    });
}

app.UseStaticFiles();

app.MapControllers();

app.Run();