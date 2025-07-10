using ECommerce.Core.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Watch_Ecommerce.Services;
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
        public CartService CartService { get; }

        public CartController(ICartRepository cartRepository , CartService cartService )
        {
            CartRepository = cartRepository;
            CartService = cartService;
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

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(List<CartItem> items)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var basket = await CartRepository.GetBasketAsync(userId);
            if (basket == null)
            {
                basket = new CustomerBasket(userId);
            }
            if (basket.Items == null)
            {
                basket.Items = new List<CartItem>();
            }

            foreach (var newItem in items)
            {
                var existingItem = basket.Items.FirstOrDefault(i => i.id == newItem.id);
                if (existingItem != null)
                {
                    existingItem.Quantity = newItem.Quantity; 
                }
                else
                {
                    basket.Items.Add(newItem);
                }
            }

            var createdOrUpdated = await CartRepository.UpdateBasketAsync(basket);
            if (createdOrUpdated == null) return BadRequest();

            return Ok(createdOrUpdated);
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            return await CartRepository.DeleteBasketAsync(id);
        }

        [HttpDelete("item/{productId}")]
        public async Task<ActionResult<CustomerBasket>> RemoveItemFromBasket(int productId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var updatedBasket = await CartRepository.RemoveItemFromBasketAsync(userId, productId);
            if (updatedBasket == null) return NotFound("Basket not found or item does not exist");

            return Ok(updatedBasket);
        }

        [HttpPost("AddItem")]
        public async Task<ActionResult<CustomerBasket>> AddItemToBasket([FromBody] CartItem newItem)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            try
            {
                var updatedBasket = await CartService.AddItemToBasketWithStockCheck(userId, newItem);
                return Ok(updatedBasket);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //[HttpPost("AddItem")]
        //public async Task<ActionResult<CustomerBasket>> AddItemToBasket([FromBody] CartItem newItem)
        //{
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //    if (string.IsNullOrEmpty(userId)) return Unauthorized();

        //    var basket = await CartRepository.GetBasketAsync(userId);
        //    if (basket == null)
        //    {
        //        basket = new CustomerBasket(userId);
        //    }
        //    if (basket.Items == null)
        //    {
        //        basket.Items = new List<CartItem>();
        //    }

        //    var existingItem = basket.Items.FirstOrDefault(i => i.id == newItem.id);
        //    if (existingItem != null)
        //    {
        //        existingItem.Quantity += newItem.Quantity;  
        //    }
        //    else
        //    {
        //        basket.Items.Add(newItem);
        //    }

        //    var updatedBasket = await CartRepository.UpdateBasketAsync(basket);
        //    if (updatedBasket == null) return BadRequest();

        //    return Ok(updatedBasket);
        //}


    }
}
