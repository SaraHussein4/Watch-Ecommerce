using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Core.model;

namespace Watch_EcommerceBl.Interfaces
{
    public interface IProductRepository :IGenericRepository<Product,int>
    {
        public  Task<Product> GetProdByName(string name);
        public Task<Product> GetProductByIdAsync(int id);

    }
}
