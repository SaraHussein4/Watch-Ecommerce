using ECommerce.Core.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Watch_EcommerceBl.Interfaces;
using Watch_EcommerceDAL.Models;

namespace Watch_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CartController : ControllerBase
    {

        public ICartRepository CartRepository { get; }

        public CartController(ICartRepository cartRepository)
        {
            CartRepository = cartRepository;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerBasket>> GetCustomerBasket(string id)
        {
            var Basket = await CartRepository.GetBasketAsync(id);
            if (Basket is null)
            {
                return new CustomerBasket(id);

            }

            else
                return Basket;
        }

        //[HttpPost]
        //public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (string.IsNullOrEmpty(userId))
        //        return Unauthorized("User not authenticated");

        //    basket.Id = userId;
        //    var CreatedOrUpdatedBasket = await CartRepository.UpdateBasketAsync(basket);
        //    if (CreatedOrUpdatedBasket is null) return BadRequest();
        //    return Ok(CreatedOrUpdatedBasket);
        //}
        [HttpPost]

        public async Task<ActionResult<CustomerBasket>> UpdateBasket( List<CartItem> items)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var basket = await CartRepository.GetBasketAsync(userId);
            if (basket == null)
            {
                basket = new CustomerBasket(userId);
            }

            basket.Items = items;

            var createdOrUpdated = await CartRepository.UpdateBasketAsync(basket);
            if (createdOrUpdated == null) return BadRequest();

            return Ok(createdOrUpdated);
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            return await CartRepository.DeleteBasketAsync(id);
        }

    }
}
