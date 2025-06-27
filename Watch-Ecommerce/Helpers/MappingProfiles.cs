using AutoMapper;
using ECommerce.Core.model;
using Watch_Ecommerce.DTOs.Product;
using Watch_Ecommerce.DTOS.Category;
using Watch_Ecommerce.DTOS.Product;
using Watch_Ecommerce.DTOS.ProductBrand;

namespace Watch_Ecommerce.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            #region Category
            CreateMap<CategoryCreateDTO, Category>()
                .ReverseMap();
    
            CreateMap<Category, CategoryReadDTO>()
                .ReverseMap();

            CreateMap<CategoryUpdateDTO, Category>()
                .ReverseMap();
            #endregion
            #region ProductBrand
            CreateMap<ProductBrandCreateDTO, ProductBrand>()
                .ReverseMap();

            CreateMap<ProductBrand, ProductBrandReadDTO>()
                .ReverseMap();

            CreateMap<ProductBrandUpdateDTO, ProductBrand>()
                .ReverseMap();
            #endregion

            #region Product
            CreateMap<Product, ProductReadDTO>()
               .AfterMap((src, dest) => {
                   dest.CategoryName = src.Category?.Name;
                   dest.ProductBrandName = src.ProductBrand?.Name;
               });

            CreateMap<ProductReadDTO, Product>()
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.ProductBrand, opt => opt.Ignore());

            CreateMap<AddProductDTO, Product>()
              .ForMember(dest => dest.CategoryId, opt => opt.Ignore())
              .ForMember(dest => dest.ProductBrandId, opt => opt.Ignore())
              .ForMember(dest => dest.Category, opt => opt.Ignore())
              .ForMember(dest => dest.ProductBrand, opt => opt.Ignore());

            CreateMap<Product, UpdateProductDTO>()
              .ReverseMap();

            #endregion

        }
    }
}
