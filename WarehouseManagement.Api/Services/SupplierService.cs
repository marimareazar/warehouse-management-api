using WarehouseManagement.Api.Contracts;
using WarehouseManagement.Api.Data;
using WarehouseManagement.Api.Models;

namespace WarehouseManagement.Api.Services;

public class SupplierService
{
    public IEnumerable<Supplier> GetAll()
    {
        return FakeWarehouseStore.Suppliers;
    }

    public Supplier? GetById(Guid id)
    {
        return FakeWarehouseStore.Suppliers
            .FirstOrDefault(s => s.Id == id);
    }

    public Supplier Create(CreateSupplierRequest request)
    {
        var supplier = new Supplier
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Country = request.Country,
            ContactEmail = request.ContactEmail,
            PhoneNumber = request.PhoneNumber,
            IsActive = true
        };

        FakeWarehouseStore.Suppliers.Add(supplier);

        return supplier;
    }

    public bool Deactivate(Guid id)
    {
        var supplier = FakeWarehouseStore.Suppliers
            .FirstOrDefault(s => s.Id == id);

        if (supplier == null)
        {
            return false;
        }

        supplier.IsActive = false;

        return true;
    }
}