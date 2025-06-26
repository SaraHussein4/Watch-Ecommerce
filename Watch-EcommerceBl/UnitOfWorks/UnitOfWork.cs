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
        public IGenericRepository<Category, int> categoryRepository;
        public IGenericRepository<ProductBrand, int> productBrandRepository;

        public TikrContext _context { get; set; }

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


        //public Hashtable _repositories;
        public UnitOfWork(TikrContext dbContext)
        {
            _context = dbContext;
            //_repositories = new Hashtable();
        }
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
    }
}
