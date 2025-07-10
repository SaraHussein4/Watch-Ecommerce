using Watch_EcommerceBl.Interfaces;
using Watch_EcommerceDAL.Models;

namespace Watch_Ecommerce.Services
{
    public class CartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository;

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }
        public async Task<CustomerBasket> AddItemToBasketWithStockCheck(string userId, CartItem newItem)
        {
            var basket = await _cartRepository.GetBasketAsync(userId);
            if (basket == null)
                basket = new CustomerBasket(userId);

            var product = await _productRepository.GetProductByIdAsync(newItem.id);
            if (product == null)
                throw new Exception("Product not found");

            if (newItem.Quantity > product.Quantity)
                newItem.Quantity = product.Quantity;

            var existingItem = basket.Items.FirstOrDefault(i => i.id == newItem.id);
            if (existingItem != null)
            {
                existingItem.Quantity += newItem.Quantity;

                if (existingItem.Quantity > product.Quantity)
                    existingItem.Quantity = product.Quantity;
            }
            else
            {
                basket.Items.Add(newItem);
            }

            return await _cartRepository.UpdateBasketAsync(basket);
        }

    }
}
