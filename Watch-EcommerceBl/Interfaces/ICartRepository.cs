using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Watch_EcommerceDAL.Models;

namespace Watch_EcommerceBl.Interfaces
{
    public interface ICartRepository
    {
        Task<CustomerBasket?> GetBasketAsync(string BasketId);
        Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket Basket);
        Task<bool> DeleteBasketAsync(string BasketId);

        Task<CustomerBasket?> RemoveItemFromBasketAsync(string userId, int productId);

    }
}
