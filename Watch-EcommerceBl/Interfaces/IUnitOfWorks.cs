using ECommerce.Core.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch_EcommerceBl.Interfaces
{
    public interface IUnitOfWorks : IAsyncDisposable
    {
        IGenericRepository<Category, int> CategoryRepository {  get; }
        IGenericRepository<ProductBrand, int> ProductBrandRepository {  get; }

        IGenericRepository<Product, int> ProductRepository { get; }

        public IGenericRepository<ProductBrand, int> ProductRepository
        {
            get;
        }
        public IProductRepository productrepo
        {
            get;
        }
        public IFavouriteRepository FavoriteRepo
        {
            get;
        }

        Task<int> CompleteAsync();
    }
}
