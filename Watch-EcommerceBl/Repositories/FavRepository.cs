using ECommerce.Core.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch_EcommerceBl.Interfaces;

namespace Watch_EcommerceBl.Repositories
{
    public class FavRepository : IFavRepository
    {
        public TikrContext _context { get; }
        public FavRepository(TikrContext context)
        {
            _context = context;
        }


        public async Task AddAsync(Favourite favourite)
        {
            await _context.Favourites.AddAsync(favourite);
        }

        public async Task<IEnumerable<Favourite>> GetAllAsync()
        {
            return await _context.Favourites.ToListAsync();
        }

        public async Task<Favourite> GetByIdAsync(int ProductId, string UserId)
        {
            return await _context.Favourites.FirstOrDefaultAsync(f => f.ProductId == ProductId && f.UserId == UserId);
        }

        public async Task DeleteAsync(int ProductId, string UserId)
        {
            var fav = await GetByIdAsync(ProductId, UserId);
            _context.Favourites.Remove(fav);
        }

        public async Task<IEnumerable<Favourite>> GetAllForUser(string UserId)
        {
            return await _context.Favourites.Where(f => f.UserId == UserId).ToListAsync();
        }
    }
}
