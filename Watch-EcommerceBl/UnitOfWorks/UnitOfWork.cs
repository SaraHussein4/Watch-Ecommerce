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
        public TikrContext _context { get; set; }

        public Hashtable _repositories;
        public UnitOfWork(TikrContext dbContext)
        {
            _context = dbContext;
            _repositories = new Hashtable();
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

        public IGenericRepository<TEntity, TKey> Repository<TEntity, TKey>() where TEntity : class
        {
            var type= typeof(TEntity).Name + typeof(TKey).Name;

            if (!_repositories.ContainsKey(type))
            {
                var Repository = new GenericRepository<TEntity, TKey>(_context);
                _repositories.Add(type, Repository);
            }

            return _repositories[type] as IGenericRepository<TEntity, TKey>;
        }
    }
}
