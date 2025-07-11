using ECommerce.Core.model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch_EcommerceBl.Interfaces;
using Watch_EcommerceBl.Repositories;
using Watch_EcommerceDAL.Models;

namespace Watch_EcommerceBl.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWorks
    {

        private readonly TikrContext _context;
        ProductRepository productRepository;
        public IGenericRepository<Category, int> categoryRepository;
        public IGenericRepository<ProductBrand, int> productBrandRepository;
        public IGenericRepository<Image, int> imageRepository;
        
        public IUserRepository userRepository;

        public IGenericRepository<Order, int> orderRepository;

        public IGenericRepository<Governorate, int> governorateRepository;
        public IGenericRepository<Deliverymethod, int> deliveryMethodRepository;

        IProductRepository _productrepo;
        IFavouriteRepository favouriteRepository;

        //public Hashtable _repositories;
        public UnitOfWork(TikrContext dbContext)
        {
            _context = dbContext;
        }

        public IGenericRepository<Category, int> CategoryRepository
        {
            get
            {
                if (categoryRepository == null) {
                    categoryRepository = new GenericRepository<Category, int>(_context);
                }
                return categoryRepository;
            }
        }

        public IGenericRepository<ProductBrand, int> ProductBrandRepository
        {
            get
            {
                if (productBrandRepository == null)
                {
                    productBrandRepository = new GenericRepository<ProductBrand, int>(_context);
                }
                return productBrandRepository;
            }
        }


        public IGenericRepository<Image, int> ImageRepository
        {
            get
            {
                if (imageRepository == null)
                {
                    imageRepository = new GenericRepository<Image, int>(_context);
                }
                return imageRepository;
            }
        }

        public IGenericRepository<Product, int> ProductRepository
        {
            get
            {
                if (productRepository == null)
                {
                    productRepository = new ProductRepository(_context);
                }
                return productRepository;
            }
        }

        //product
        public IProductRepository productrepo
        {
            get
            {
                if (_productrepo == null)
                {
                    _productrepo = new ProductRepository(_context);
                }
                return _productrepo;
            }
        }
        //fav
        public IFavouriteRepository FavoriteRepo
        {
            get
            {
                if (favouriteRepository == null)
                {
                    favouriteRepository = new FavouriteRepository(_context);
                }
                return favouriteRepository;
            }
            
        }

        public IGenericRepository<Governorate, int> GovernorateRepository
        {
            get
            {
                if (governorateRepository == null)
                {
                    governorateRepository = new GenericRepository<Governorate, int>(_context);
                }
                return governorateRepository;
            }
        }


        public IGenericRepository<Order, int> OrderRepository
        {
            get
            {
                if (orderRepository == null)
                {
                    orderRepository = new GenericRepository<Order, int>(_context);
                }
                return orderRepository;
            }
        }

        public IGenericRepository<Deliverymethod, int> DeliveryMethodRepository
        {
            get
            {
                if (deliveryMethodRepository == null)
                {
                    deliveryMethodRepository = new GenericRepository<Deliverymethod, int>(_context);
                }
                return deliveryMethodRepository;
            }
        }


        // User
        public IUserRepository UserRepository
        {
            get { 
                if(userRepository == null)
                {
                    userRepository = new UserRepository(_context);
                }
                return userRepository;
            }
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

    }
}

