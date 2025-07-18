﻿using ECommerce.Core.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Watch_EcommerceBl.Interfaces
{
    public interface IFavouriteRepository:IGenericRepository<Favourite,int>
    {
        public  Task<Favourite> AddToFav(string userid, int productid);
        public Task<bool> RemoveFromFav(string userid, int productid);

        Task<IEnumerable<Favourite>> GetAllForUser(string UserId);
        Task<Favourite> GetByIdAsync(int ProductId, string UserId);

        Task<int> GetCountAsync(string UserId);
    }
}
