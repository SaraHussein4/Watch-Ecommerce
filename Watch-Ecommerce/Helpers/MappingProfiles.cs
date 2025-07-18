﻿using AutoMapper;
using ECommerce.Core.model;
using Microsoft.EntityFrameworkCore;
using Watch_Ecommerce.DTOs.Order;
using Watch_Ecommerce.DTOs.Product;
using Watch_Ecommerce.DTOS;
using Watch_Ecommerce.DTOS.Address;
using Watch_Ecommerce.DTOS.Category;
using Watch_Ecommerce.DTOS.Color;
using Watch_Ecommerce.DTOS.Fav;
using Watch_Ecommerce.DTOS.ImageDTO;
using Watch_Ecommerce.DTOS.Order;
using Watch_Ecommerce.DTOS.Product;
using Watch_Ecommerce.DTOS.ProductBrand;
using Watch_Ecommerce.DTOS.Size;
using Watch_Ecommerce.DTOS.User;
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

            CreateMap<Category, CategoryDTO>()
                .ReverseMap();

            CreateMap<CategoryUpdateDTO, Category>()
                .ReverseMap();
            CreateMap<Governorate, GovernorateDto>();
            CreateMap<Deliverymethod, DeliverymethodDto>();
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
            .ForMember(dest => dest.Category,
                       opt => opt.MapFrom(src => src.Category))
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

//             CreateMap<UpdateProductDTO, Product>()
//    .ForMember(dest => dest.Colors, opt => opt.MapFrom(src =>
//        src.Colors != null ? string.Join(",", new object?[] { src.Colors }) : null))
//    .ForMember(dest => dest.Sizes, opt => opt.MapFrom(src =>
//        src.Sizes != null ? string.Join(",", new object?[] { src.Sizes }) : null))
//    .ReverseMap();

   //         CreateMap<UpdateProductDTO, Product>()
   //.ForMember(dest => dest.Colors, opt => opt.MapFrom(src =>
   //    src.Colors != null ? string.Join(",", src.Colors) : null))
   //.ForMember(dest => dest.Sizes, opt => opt.MapFrom(src =>
   //    src.Sizes != null ? string.Join(",", src.Sizes) : null))
   //.ReverseMap();


            CreateMap<Image, ImageDTO>().ReverseMap();


            CreateMap<ProductCreateDTO, Product>()
                .ForMember((src) => src.Images, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ReverseMap();

           CreateMap<UpdateProductDTO, Product>()
                .IncludeBase<ProductCreateDTO, Product>()
                .ForMember(dest => dest.Images, opt => opt.Ignore());
            #endregion

            #region fav
            CreateMap<Favourite, FavDTO>().AfterMap((src, dst) =>
            {
                dst.ProductId = src.ProductId;

            }).ReverseMap();



            CreateMap<Favourite, FavReadDTO>()
                .ReverseMap();

            CreateMap<FavCreateDTO, Favourite>()
                .ReverseMap();


            CreateMap<Favourite, DisplayProductDTO>()
                .IncludeMembers(src => src.Product)
                .ReverseMap();
            #endregion

            #region order
            CreateMap<OrderAddressDto,OrderAddress>().ReverseMap();
            CreateMap<Order,OrderDetailsDto>()
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.User.Email))
                .ReverseMap();
            CreateMap<Order, OrderDto>();
            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductName, 
                    opt => opt.MapFrom(src => src.Product.Name))
                .ForMember(dest => dest.image,
                    opt => opt.MapFrom(src => src.Product.Images.FirstOrDefault(img => img.isPrimary).Url));
            CreateMap<OrderAddress, OrderAddressDto>();
            CreateMap<Deliverymethod, DeliverymethodDto>();
            #endregion

            #region image
            CreateMap<Image,AddImgDto>().ReverseMap();

            #endregion

            #region User
            CreateMap<User, UserReadDTO>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Name,
                    opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Email,
                    opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.PhoneNumber,
                    opt => opt.MapFrom(src => src.PhoneNumber))
                .ReverseMap();
            CreateMap<User, UserProfileReadDTO>();
            CreateMap<UserProfileUpdateDTO, User>()
                 .ForMember(dest => dest.Addresses, opt => opt.Ignore());
            #endregion


            #region address

            CreateMap<AddressDTO, Address>()
                .ReverseMap();
            #endregion


        }
    }
}
