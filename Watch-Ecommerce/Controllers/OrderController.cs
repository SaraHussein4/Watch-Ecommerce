using ECommerce.Core.model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Watch_Ecommerce.DTOs.Order;
using Watch_Ecommerce.Services;
using Watch_EcommerceDAL.Models;

namespace Watch_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly OrderService OrderService;

        public OrderController(OrderService orderService)
        {
            OrderService = orderService;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreatedOrderDto dto)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User not authenticated");

                var order = await OrderService.CreateOrderAsync(userId, dto.DeliveryMethodId, dto.ShippingAddress);

                if (order == null)
                    return BadRequest("Basket is empty or something went wrong.");

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }
    }
}
