using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECommerce.Core.model;
using Watch_EcommerceBl.Interfaces;

namespace Watch_EcommerceBl.Repositories
{
    public class ProductRepository : GenericRepository<Product, int>, IProductRepository
    {
        public ProductRepository(TikrContext context) : base(context)
        {
        }
    }

}
