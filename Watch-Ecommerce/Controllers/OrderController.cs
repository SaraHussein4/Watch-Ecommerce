using AutoMapper;
using ECommerce.Core.model;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Watch_Ecommerce.DTOs.Order;
using Watch_Ecommerce.DTOS.Order;
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
        private readonly IMapper mapper;

        public OrderController(OrderService orderService, IMapper mapper)
        {
            OrderService = orderService;
            this.mapper = mapper;
        }
        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreatedOrderDto dto)
        {
            try
            {
                var userclaims = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userclaims == null)
                    return Unauthorized("User not authenticated");
                string userId = userclaims.Value;

                var order = await OrderService.CreateOrderAsync(userId, dto.DeliveryMethodId, dto.ShippingAddress);

                if (order == null)
                    return BadRequest("Basket is empty or something went wrong.");

                var orderDto = mapper.Map<OrderDto>(order);
                return Ok(orderDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderByIdForCurrentUser(int orderId)
        {
            try
            {
                var userClaims = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userClaims == null)
                    return Unauthorized("User not authenticated");

                string userId = userClaims.Value;

                var order = await OrderService.GetOrderByIdForUserAsync(userId, orderId);
                if (order == null)
                    return NotFound("Order not found or does not belong to this user.");

                var orderDto = mapper.Map<OrderDto>(order);
                return Ok(orderDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }


        [HttpGet]
        public async Task<IActionResult> GetOrderByID(int id)
        {
            try
            {
                var myorder= await OrderService.GetOrderByIdAsynce(id);
                if (myorder == null)
                    return NotFound();
                var orderDetails=mapper.Map<OrderDetailsDto>(myorder);
                return Ok(orderDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving product: {ex.Message}");
            }

        }
        [HttpPost("cancel/{orderId}")]
        public async Task<IActionResult> CanceldOrder(int orderid)
        {
            try
            {
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (string.IsNullOrEmpty(userId))
                    return Unauthorized("User not authenticated");
                var orderCancel= await OrderService.CancelorderAsync(userId,orderid);
                if(orderCancel == null)
                    return BadRequest("Cannot cancel this order. It may have already been shipped or does not exist.");
                return Ok("Order cancelled successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }
    }
}
