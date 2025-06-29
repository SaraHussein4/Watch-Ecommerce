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
    public class FavouriteRepository : GenericRepository<Favourite, int>, IFavouriteRepository
    {
        TikrContext con;
        public FavouriteRepository(TikrContext con) : base(con)
        {
            this.con = con;
        }
        public async Task<Favourite> AddToFav(string userid,int productid)
        {
            var favWatch= await con.Favourites.FirstOrDefaultAsync(f=>f.UserId == userid 
            && f.ProductId == productid);
            if ((favWatch != null)) return null;
            var newFav = new Favourite()
            {
                UserId = userid,
                ProductId = productid
            };
            con.Favourites.Add(newFav);
             await con.SaveChangesAsync();
            return newFav;
        }
        public async Task<bool> RemoveFromFav(string userid, int productid)
        {
            var myFavWatch = await con.Favourites.FirstOrDefaultAsync(f=>f.UserId == userid
            && f.ProductId == productid);
            if (myFavWatch == null)
                return false;
            con.Favourites.Remove(myFavWatch);
            await con.SaveChangesAsync();
            return true;
        }
    }
}
