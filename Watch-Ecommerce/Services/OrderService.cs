using ECommerce.Core.model;
using Microsoft.EntityFrameworkCore;
using Watch_Ecommerce.DTOs.Order;
using Watch_EcommerceBl.Interfaces;
using Watch_EcommerceDAL.Models;

namespace Watch_Ecommerce.Services
{
    public class OrderService
    {
        private readonly ICartRepository _cartRepository;
        private readonly TikrContext _context;
        public OrderService(ICartRepository cartRepository , TikrContext tikrContext)
        {
            _cartRepository = cartRepository;
            _context = tikrContext;
        }
        public async Task<Order?> CreateOrderAsync(string userId, int deliveryMethodId, OrderAddressDto dto)
        {
            var basket = await _cartRepository.GetBasketAsync(userId);
            if (basket == null || !basket.Items.Any())
                return null;
          
            var deliveryMethod = await _context.Deliverymethods.FindAsync(deliveryMethodId);
            if (deliveryMethod == null)
                return null;
            var order = new Order
            {
                UserId = userId,
                DeliveryMethodId = deliveryMethodId,
                Status = "Pending",
                Date = DateTime.Now,
                Amount = basket.Items.Sum(i => i.Price * i.Quantity),

                OrderAddress = new OrderAddress
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    City = dto.City,
                    Street = dto.Street,
                    GovernorateId = dto.GovernorateId
                },

                OrderItems = basket.Items.Select(i => new OrderItem
                {
                    ProductId = i.id,
                    Quantity = i.Quantity,
                    Price = i.Price,
                    Amount = (int)(i.Price * i.Quantity)
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            await _cartRepository.DeleteBasketAsync(userId);

            return order;
        }
      
        public async Task<Order> GetOrderByIdAsynce(int id)
        {
            return await _context.Orders.FirstOrDefaultAsync(f=>f.Id==id);
        }

        public async Task<IEnumerable<Governorate>> GetAllGovernorate()
        {
            return await _context.Governorates.ToListAsync();
        }

        public async Task<IEnumerable<Deliverymethod>> GetDeliverymethodsAsync()
        {
            return await _context.Deliverymethods.ToListAsync();
        }
        public async Task<bool> CancelorderAsync(string userid,int orderid)
        {
            var order= await _context.Orders.FirstOrDefaultAsync(o=>o.UserId==userid && o.Id==orderid);
            if(order==null) return false;
            order.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<decimal?> GetDeliveryCostAsync(int governorateId, int deliveryMethodId)
        {
            var entry = await _context.GovernorateDeliveryMethods
                .FirstOrDefaultAsync(x => x.GovernorateId == governorateId && x.DeliveryMethodId == deliveryMethodId);

            return entry?.DeliveryCost;
        }



    }
}
