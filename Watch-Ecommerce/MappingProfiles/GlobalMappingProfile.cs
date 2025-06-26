using AutoMapper;
using ECommerce.Core.model;
using Watch_Ecommerce.DTOS.Category;
using Watch_Ecommerce.DTOS.Product;
using Watch_Ecommerce.DTOS.ProductBrand;

namespace Watch_Ecommerce.MappingProfiles
{
    public class GlobalMappingProfile: Profile
    {
        public GlobalMappingProfile()
        {
            #region Category
            CreateMap<Category, CategoryReadDTO>()
            .ReverseMap();
            
            CreateMap<CategoryCreateDTO,Category>()
            .ReverseMap();

            CreateMap<CategoryUpdateDTO, Category>()
            .ReverseMap();
            #endregion

            #region Product Brand
            CreateMap<ProductBrand, ProductBrandReadDTO>()
            .ReverseMap();

            CreateMap<ProductBrandCreateDTO, ProductBrand>()
            .ReverseMap();

            CreateMap<ProductBrandUpdateDTO, ProductBrand>()
            .ReverseMap();
            #endregion

            #region Product
            CreateMap<Product, ProductReadDTO>()
                .ReverseMap();
            #endregion
        }
    }
}
