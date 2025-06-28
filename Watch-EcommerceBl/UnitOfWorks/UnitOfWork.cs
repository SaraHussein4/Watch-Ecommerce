using ECommerce.Core.model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch_EcommerceBl.Interfaces;
using Watch_EcommerceBl.Repositories;

namespace Watch_EcommerceBl.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWorks
    {

        private readonly TikrContext _context;
        ProductRepository productRepository;
        public IGenericRepository<Category, int> categoryRepository;
        public IGenericRepository<ProductBrand, int> productBrandRepository;

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

        IGenericRepository<Product, int> IUnitOfWorks.ProductRepository => throw new NotImplementedException();

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }

        //public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : class
        //{
        //    var type = typeof(TEntity).Name;
        //    if (!_repositories.ContainsKey(type))
        //    {
        //        var Repository = new GenericRepository<TEntity>(_dbContext);
        //        _repositories.Add(type, Repository);
        //    }
        //    return _repositories[type] as IGenericRepository<TEntity>;
        //}

        //public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : class
        //{
        //    var type= typeof(TEntity).Name + typeof(TKey).Name;

        //    if (!_repositories.ContainsKey(type))
        //    {
        //        var Repository = new GenericRepository<TEntity, TKey>(_context);
        //        _repositories.Add(type, Repository);
        //    }

        //    return _repositories[type] as IGenericRepository<TEntity, TKey>;
        //}
    }
}

