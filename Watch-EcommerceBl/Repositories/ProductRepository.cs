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
        public async Task<Product> GetProductByIdAsync(int pid)
        {
            return await context.Products.FirstOrDefaultAsync(f=>f.Id==pid);
        }
        // For GetAllProducts()
        public async Task<IEnumerable<Product>> GetAllWithPrimaryImageAsync()
        {
            return await _context.Products
                                 .Include(p => p.Images.Where(img => img.isPrimary))
                                 .ToListAsync();
        }

        // For GetProductById()
        public async Task<Product?> GetByIdWithImagesAsync(int id)
        {
            return await _context.Products
                                 .Include(p => p.Images)
                                 .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetOneProductPerBrandAsync()
        {
            return await context.Products
                .Include(p => p.Images)
                .Include(p => p.ProductBrand)
                .GroupBy(p => p.ProductBrandId)
                .Select(g => g.OrderByDescending(p => p.Id).FirstOrDefault())
                .ToListAsync();
        }
    }

}
