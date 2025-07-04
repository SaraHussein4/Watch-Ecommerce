using ECommerce.Core.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch_EcommerceDAL.Models;

namespace Watch_EcommerceBl.Interfaces
{
    public interface IUnitOfWorks : IAsyncDisposable
    {
        IGenericRepository<Category, int> CategoryRepository {  get; }
        IGenericRepository<ProductBrand, int> ProductBrandRepository {  get; }
        IGenericRepository<Image, int> ImageRepository { get; }
        IGenericRepository<Governorate, int> GovernorateRepository { get; }
        IGenericRepository<Deliverymethod, int> DeliveryMethodRepository { get; }

        IUserRepository UserRepository { get; }


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
