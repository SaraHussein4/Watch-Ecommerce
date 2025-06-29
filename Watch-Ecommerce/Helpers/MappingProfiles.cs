using AutoMapper;
using ECommerce.Core.model;
using Watch_Ecommerce.DTOs.Fav;
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
            CreateMap<Product, ProductReadDTO>().ReverseMap();

            CreateMap<Product, DisplayProductDTO>().ReverseMap();
            CreateMap<AddProductDTO, Product>().ReverseMap();
            CreateMap<Product, UpdateProductDTO>().ReverseMap();

            #endregion

            #region fav
            CreateMap<Favourite, FavDto>().AfterMap((src, dst) =>
            {
                
            }).ReverseMap();
            #endregion
        }
    }
}
