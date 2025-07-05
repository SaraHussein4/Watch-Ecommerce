using Microsoft.EntityFrameworkCore.Storage;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Watch_EcommerceBl.Interfaces;
using Watch_EcommerceDAL.Models;

namespace Watch_EcommerceBl.Repositories
{
    public class CartRepositry : ICartRepository
    {
        private readonly StackExchange.Redis.IDatabase _database;
        public CartRepositry(IConnectionMultiplexer redis)
        {
            _database = redis.GetDatabase();
        }
        public async Task<bool> DeleteBasketAsync(string BasketId)
        {
            return await _database.KeyDeleteAsync(BasketId);
        }

        public async Task<CustomerBasket?> GetBasketAsync(string BasketId)
        {
            var Basket = await _database.StringGetAsync(BasketId);
            return Basket.IsNull ? null : JsonSerializer.Deserialize<CustomerBasket>(Basket);


        }

        public async Task<CustomerBasket?> RemoveItemFromBasketAsync(string userId, int productId)
        {
            var basket = await GetBasketAsync(userId);
            if (basket == null) return null;

            var itemToRemove = basket.Items.FirstOrDefault(item => item.id == productId);
            if (itemToRemove == null) return basket;

            basket.Items.Remove(itemToRemove);

            return await UpdateBasketAsync(basket);
        }

        public async Task<CustomerBasket?> UpdateBasketAsync(CustomerBasket Basket)
        {
            var JsonBasket = JsonSerializer.Serialize(Basket);
            var CreatedOrUpdated = await _database.StringSetAsync(Basket.Id, JsonBasket, TimeSpan.FromDays(10));
            if (!CreatedOrUpdated) return null;

            return await GetBasketAsync(Basket.Id);
        }
    }
}
