using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch_EcommerceDAL.Models;

namespace Watch_EcommerceBl.Interfaces
{
    public interface IGenericRepository<T> where T : BaseEntity
    {
        Task<IReadOnlyList<T>> GetAllAsync();
        Task<T> GetByIdAsync(int id);
        void Update(T entity, int id);
        void Delete(int id);
        Task AddAsync(T item);
    }
}
