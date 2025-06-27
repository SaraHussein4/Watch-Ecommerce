using ECommerce.Core.model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch_EcommerceBl.Interfaces;


namespace Watch_EcommerceBl.Repositories
{
    public class GenericRepository<TEntity, Tkey> : IGenericRepository<TEntity, Tkey> where TEntity : class
    {
        protected readonly TikrContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public GenericRepository(TikrContext context)
        {
            _context = context;
            _dbSet = context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity)
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Tkey id)
        {
            TEntity entity = await GetByIdAsync(id);
            _dbSet.Remove(entity);
        }

        public async Task<bool> ExistsAsync(Tkey id)
        {
            return await _dbSet.FindAsync(id) != null;

        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await _dbSet.ToListAsync();
        }
        public async Task<Tuple<IEnumerable<TEntity>, int>> GetPageAsync(int page, int pageSize)
        {
            var entites = await _dbSet.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
            return new Tuple<IEnumerable<TEntity>, int>(entites, _dbSet.Count());
        }

        public async Task<TEntity> GetByIdAsync(Tkey id)
        {
            return await _dbSet.FindAsync(id);
        }

        public async Task UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
        }
    }
}
