using WarehouseManagement.Api.Services;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Api.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddScoped<SupplierService>();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<WarehouseDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("WarehouseDb")
    )
);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Warehouse Management API v1");
    });
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();