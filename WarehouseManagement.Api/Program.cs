using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Api.Data.DbFirst;
using WarehouseManagement.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<SupplierService>();

builder.Services.AddDbContext<WarehouseDbFirstContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("WarehouseDbFirst")
    )
);

builder.Services.AddOpenApi();

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

app.UseHttpsRedirection();

app.MapControllers();

app.Run();