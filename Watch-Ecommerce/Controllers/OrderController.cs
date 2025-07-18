﻿using AutoMapper;
using ECommerce.Core.model;
using Humanizer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Watch_Ecommerce.DTOs.Order;
using Watch_Ecommerce.DTOS.Category;
using Watch_Ecommerce.DTOS.Order;
using Watch_Ecommerce.Services;
using Watch_EcommerceBl.Interfaces;
using Watch_EcommerceBl.UnitOfWorks;
using Watch_EcommerceDAL.Models;

namespace Watch_Ecommerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class OrderController : ControllerBase
    {
        private readonly OrderService OrderService;
        private readonly IMapper mapper;
        private readonly IUnitOfWorks _unitOfWork;

        public OrderController(OrderService orderService, IUnitOfWorks unitOfWork, IMapper mapper)
        {
            OrderService = orderService;
            _unitOfWork = unitOfWork;
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

                var order = await OrderService.CreateOrderAsync(userId, dto.DeliveryMethodId, dto.ShippingAddress, dto.PaymentMethod);

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
                var myorder = await OrderService.GetOrderByIdAsynce(id);
                if (myorder == null)
                    return NotFound();
                var orderDetails = mapper.Map<OrderDetailsDto>(myorder);
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
                var orderCancel = await OrderService.CancelorderAsync(userId, orderid);
                if (!orderCancel)
                    return BadRequest("Cannot cancel this order. It may have already been shipped or does not exist.");
                return Ok("Order cancelled successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server Error: {ex.Message}");
            }
        }

        [HttpGet("orders")]
        public async Task<ActionResult<IEnumerable<OrderDetailsDto>>> GetOrders(int page = 1, int pageSize = 10)
        {
            try
            {
                var orders = await _unitOfWork.OrderRepository.GetAllAsync();
                int totalCount = orders.Count();
                orders = orders.Skip((page - 1) * pageSize).Take(pageSize).OrderByDescending(o => o.Date);
                var dto = mapper.Map<IEnumerable<OrderDetailsDto>>(orders);
                return Ok(new
                {
                    orders = dto,
                    totalCount = totalCount
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving orders: {ex.Message}");
            }
        }

        [HttpGet("ordersForUser")]
        public async Task<ActionResult<IEnumerable<OrderDto>>> GetOrdersForUser(int page = 1, int pageSize = 10)
        {
            try
            {
                var userclaims = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userclaims == null)
                    return Unauthorized("User not authenticated");
                string userId = userclaims.Value;

                var orders = await _unitOfWork.OrderRepository.GetAllAsync();
                int totalCount = orders.Where(o => o.UserId == userId).Count();
                orders = orders.Where(o => o.UserId == userId).Skip((page - 1) * pageSize).Take(pageSize).OrderByDescending(o => o.Date);
                var dto = mapper.Map<IEnumerable<OrderDto>>(orders);
                return Ok(new
                {
                    orders = dto,
                    totalCount = totalCount
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving orders: {ex.Message}");
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<IEnumerable<OrderDetailsDto>>> UpdateOrder(int id, OrderDetailsDto orderDetailsDto)
        {
            if (id != orderDetailsDto.Id)
            {
                return BadRequest();
            }

            var order = await _unitOfWork.OrderRepository.GetByIdAsync(id);
            if (order == null)
            {
                return BadRequest();
            }
            mapper.Map(orderDetailsDto, order);
            await _unitOfWork.OrderRepository.UpdateAsync(order);
            await _unitOfWork.CompleteAsync();
            return NoContent();
        }


        [HttpGet("Governorate")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<GovernorateDto>>> GetGovernorates()
        {
            try
            {
                var governorates = await _unitOfWork.GovernorateRepository.GetAllAsync();
                var dto = mapper.Map<IEnumerable<GovernorateDto>>(governorates);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving governorates: {ex.Message}");
            }
        }

        [HttpGet("DeliveryMethods")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<DeliverymethodDto>>> GetDeliveries()
        {
            try
            {
                var deliveryMethods = await _unitOfWork.DeliveryMethodRepository.GetAllAsync();
                var dto = mapper.Map<IEnumerable<DeliverymethodDto>>(deliveryMethods);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error retrieving delivery methods: {ex.Message}");
            }
        }

        [HttpGet("DeliveryCost")]
        [AllowAnonymous]
        public async Task<IActionResult> GetDeliveryCost(int governorateId, int deliveryMethodId)
        {
            try
            {
                var cost = await OrderService.GetDeliveryCostAsync(governorateId, deliveryMethodId);

                if (cost == null)
                    return NotFound("No delivery cost found for this governorate and method combination.");

                return Ok(cost);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server error: {ex.Message}");
            }
        }


        [HttpPost("CreateStripe")]
        public async Task<IActionResult> CreateStripeSession([FromBody] CreatedOrderDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var order = await OrderService.CreateOrderAsync(userId, dto.DeliveryMethodId, dto.ShippingAddress, "card");

            if (order == null)
                return BadRequest("Failed to create order.");

            var sessionUrl = await OrderService.CreateStripeSessionAsync(order.Id);

            if (string.IsNullOrEmpty(sessionUrl))
                return BadRequest("Could not create Stripe session.");

            return Ok(new { url = sessionUrl });
        }


        [HttpPost("confirm-payment")]
        public async Task<IActionResult> ConfirmPayment([FromQuery] string sessionId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId)) return Unauthorized();

            var confirmed = await OrderService.ConfirmStripeOrderAsync(sessionId, userId);
            if (!confirmed)
                return BadRequest("Payment not confirmed.");

            return Ok("Payment confirmed and order updated.");
        }


        [HttpGet("delivery-orders")]
        [Authorize(Roles = "Delivery")]
        public async Task<IActionResult> GetOrdersForDelivery(int page = 1, int pageSize = 10)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized("User not authenticated");

            var user = await _unitOfWork.UserRepository.GetByIdAsync(userId);
            if (user == null || user.GovernorateId == null)
                return BadRequest("Delivery user must have a governorate assigned.");

            var orders = await OrderService.GetOrdersByGovernorateAsync(user.GovernorateId.Value, page, pageSize);
            var dto = mapper.Map<IEnumerable<OrderDetailsDto>>(orders);
            return Ok(new
            {
                orders = dto,
                totalCount = orders.Count
            });
        }

    }
}
