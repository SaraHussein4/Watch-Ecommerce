using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Watch_EcommerceBl.Interfaces;
using Watch_EcommerceDAL.Models;

namespace Watch_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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

        [HttpPost]
        public async Task<ActionResult<CustomerBasket>> UpdateBasket(CustomerBasket basket)
        {
            var CreatedOrUpdatedBasket = await CartRepository.UpdateBasketAsync(basket);
            if (CreatedOrUpdatedBasket is null) return BadRequest();
            return Ok(CreatedOrUpdatedBasket);
        }

        [HttpDelete]
        public async Task<ActionResult<bool>> DeleteBasket(string id)
        {
            return await CartRepository.DeleteBasketAsync(id);
        }

    }
}
