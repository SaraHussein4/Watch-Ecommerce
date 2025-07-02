using AutoMapper;
using ECommerce.Core.model;
using Watch_Ecommerce.DTOs.Order;
using Watch_Ecommerce.DTOs.Product;
using Watch_Ecommerce.DTOS.Category;
using Watch_Ecommerce.DTOS.Color;
using Watch_Ecommerce.DTOS.Fav;
using Watch_Ecommerce.DTOS.ImageDTO;
using Watch_Ecommerce.DTOS.Order;
using Watch_Ecommerce.DTOS.Product;
using Watch_Ecommerce.DTOS.ProductBrand;
using Watch_Ecommerce.DTOS.Size;
using Watch_EcommerceDAL.Models;

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

            CreateMap<ProductBrand, ProductBrandWithoutProductsCollectioDTO >()
                .ReverseMap();

            CreateMap<ProductBrandUpdateDTO, ProductBrand>()
                .ReverseMap();
            #endregion

            #region Product
            CreateMap<Product, ProductReadDTO>().ReverseMap();

            CreateMap<Product, DisplayProductDTO>()
            .ForMember(dest => dest.ProductBrand,
                       opt => opt.MapFrom(src => src.ProductBrand))
            .ForMember(dest => dest.Colors, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Colors)
                    ? new List<string>()
                    : src.Colors.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()))
            .ForMember(dest => dest.Sizes, opt => opt.MapFrom(src =>
                string.IsNullOrEmpty(src.Sizes)
                    ? new List<string>()
                    : src.Sizes.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList()));

            CreateMap<AddProductDTO, Product>().ReverseMap();
            
            CreateMap<Product, UpdateProductDTO>().ReverseMap();
            
            CreateMap<Image, ImageDTO>().ReverseMap();


            CreateMap<ProductCreateDTO, Product>()
                .ForMember(src => src.Images,
                            opt => opt.Ignore())
                .ReverseMap();
            #endregion

            #region fav
            CreateMap<Favourite, FavDTO>().AfterMap((src, dst) =>
            {
                dst.ProductId = src.ProductId;

            }).ReverseMap();
            #endregion

            #region order
            CreateMap<OrderAddressDto,OrderAddress>().ReverseMap();
            CreateMap<Order,OrderDetailsDto>().ReverseMap();
            CreateMap<Order, OrderDto>();
            CreateMap<OrderItem, OrderItemDto>();
            CreateMap<OrderAddress, OrderAddressDto>();
            CreateMap<Deliverymethod, DeliverymethodDto>();
            #endregion

            //#region Color
            //CreateMap<Color, ColorReadDTO>().ReverseMap();
            
            //CreateMap<ProductColor, ColorReadDTO>()
            //    .ForMember(dest => dest.Id,
            //        opt => opt.MapFrom(src => src.ColorId))
            //    .ForMember(dest => dest.Name,
            //        opt => opt.MapFrom(src=> src.Color.Name))
            //    .ReverseMap();

            //#endregion
            //#region Size
            //CreateMap<Size, SizeReadDTO>().ReverseMap();
            //CreateMap<ProductSize, SizeReadDTO>()
            //    .ForMember(dest => dest.Id,
            //        opt => opt.MapFrom(src => src.SizeId))
            //    .ForMember(dest => dest.Name,
            //        opt => opt.MapFrom(src => src.Size.Name))
            //    .ReverseMap();

            //#endregion
            #region image
            CreateMap<Image,AddImgDto>().ReverseMap();
            #endregion
        }
    }
}
