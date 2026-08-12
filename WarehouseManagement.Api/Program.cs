using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Api.Data;
using WarehouseManagement.Api.Mapping;
using WarehouseManagement.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

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