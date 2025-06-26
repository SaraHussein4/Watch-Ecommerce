using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch_EcommerceBl.Interfaces;
using Watch_EcommerceDAL.Contexts;
using Watch_EcommerceDAL.Models;

namespace Watch_EcommerceBl.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : BaseEntity
    {
        public GenericRepository(StoreContext storeContext)
        {
            StoreContext = storeContext;
        }

        public StoreContext StoreContext { get; }

        public async Task AddAsync(T item)
        {
            await StoreContext.Set<T>().AddAsync(item);
        }

        public async void Delete(int id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                StoreContext.Set<T>().Remove(entity);
            }
        }

        public async Task<IReadOnlyList<T>> GetAllAsync()
        {
            return await StoreContext.Set<T>().ToListAsync();

        }

        public async Task<T> GetByIdAsync(int id)
        {
            return await StoreContext.Set<T>().FindAsync(id);
        }

        public void Update(T entity, int id)
        {
            StoreContext.Set<T>().Update(entity);
        }
    }
}
