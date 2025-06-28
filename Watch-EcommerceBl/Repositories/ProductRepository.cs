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
    public class ProductRepository : GenericRepository<Product, int>, IProductRepository
    {
        TikrContext context;

        public ProductRepository(TikrContext context) : base(context)
        {
           this.context = context;
        }
        public async Task<Product> GetProdByName(string name)
        {
            return await context.Products.FirstOrDefaultAsync(f => f.Name == name);

        }
        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products.FindAsync(id);
        }
    }

}
