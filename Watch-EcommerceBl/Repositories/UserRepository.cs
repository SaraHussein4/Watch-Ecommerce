using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Core.model;
using Microsoft.EntityFrameworkCore;
using Watch_EcommerceBl.Interfaces;

namespace Watch_EcommerceBl.Repositories
{
    public class UserRepository : GenericRepository<User, string>, IUserRepository
    {
        private readonly TikrContext _context;
        public UserRepository(TikrContext context): base(context)
        {
            _context = context;
        }

        public IEnumerable<User> GetCustomers()
        {
            var customers = _context.UserRoles
                .Where(ur => ur.RoleId == "2") 
                .Join(_context.Users,
                    ur => ur.UserId,  
                    u => u.Id,       
                    (ur, u) => u)    
                .ToList();

            return customers;
        }
        public async Task<User> GetByIdWithAddressesAsync(string id)
        {
            return await _context.Users
                .Include(u => u.Addresses)
                .FirstOrDefaultAsync(u => u.Id == id);
        }

    }
}
