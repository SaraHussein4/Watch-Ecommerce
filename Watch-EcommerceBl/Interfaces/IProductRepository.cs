﻿
using ECommerce.Core.model;


namespace Watch_EcommerceBl.Interfaces
{
    public interface IProductRepository :IGenericRepository<Product,int>
    {
        public  Task<Product> GetProdByName(string name);
        public Task<Product> GetProductByIdAsync(int id);
        
        Task<IEnumerable<Product>> GetAllWithPrimaryImageAsync();

        //Task<IEnumerable<Product>> GetFilteredProductsAsync(ProductFilterDTO productFilterDTO);
        Task<Product?> GetByIdWithImagesAsync(int id);

        Task<IEnumerable<Product>> GetOneProductPerBrandAsync();


    }
}
