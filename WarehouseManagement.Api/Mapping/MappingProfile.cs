using AutoMapper;
using WarehouseManagement.Api.Models;
using WarehouseManagement.Api.ViewModels;

namespace WarehouseManagement.Api.Mapping;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Product, ProductViewModel>()
            .ForMember(
                destination => destination.SupplierName,
                option => option.MapFrom(
                    source => source.Supplier != null
                        ? source.Supplier.Name
                        : null))
            .ForMember(
                destination => destination.SupplierCountry,
                option => option.MapFrom(
                    source => source.Supplier != null
                        ? source.Supplier.Country
                        : null));

        CreateMap<Supplier, SupplierViewModel>();
    }
}