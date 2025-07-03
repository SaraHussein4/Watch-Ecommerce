using ECommerce.Core.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch_EcommerceBl.Interfaces
{
    public interface IFavRepository
    {
        Task<IEnumerable<Favourite>> GetAllAsync();
        Task<Favourite> GetByIdAsync(int ProductId, string UserId);
        Task AddAsync(Favourite favourite);
        Task DeleteAsync(int ProductId, string UserId);
        Task<IEnumerable<Favourite>> GetAllForUser(string UserId);
    }
}
