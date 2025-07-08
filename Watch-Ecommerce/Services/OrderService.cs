using ECommerce.Core.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Stripe;
using Stripe.BillingPortal;
using Stripe.Checkout;
using Watch_Ecommerce.DTOs.Order;
using Watch_Ecommerce.Helpers;
using Watch_EcommerceBl.Interfaces;
using Watch_EcommerceBl.UnitOfWorks;
using Watch_EcommerceDAL.Models;
using SessionCreateOptions = Stripe.Checkout.SessionCreateOptions;
using SessionService = Stripe.Checkout.SessionService;

namespace Watch_Ecommerce.Services
{
    public class OrderService
    {
        private readonly ICartRepository _cartRepository;
        private readonly TikrContext _context;
        private readonly StripeSettings _stripeSettings;

        public OrderService(ICartRepository cartRepository , TikrContext tikrContext, IOptions<StripeSettings> stripeOptions)
        {
            _cartRepository = cartRepository;
            _context = tikrContext;
            _stripeSettings = stripeOptions.Value;
        }
        public async Task<Order?> CreateOrderAsync(string userId, int deliveryMethodId, OrderAddressDto dto, string paymentMethod)
        {
            var basket = await _cartRepository.GetBasketAsync(userId);
            if (basket == null || !basket.Items.Any())
                return null;

            var deliveryMethod = await _context.Deliverymethods.FindAsync(deliveryMethodId);
            if (deliveryMethod == null)
                return null;

            var subTotal = basket.Items.Sum(i => i.Price * i.Quantity);

            var order = new Order
            {
                UserId = userId,
                DeliveryMethodId = deliveryMethodId,
                Status = "Pending",
                Date = DateTime.Now,
                SubTotal = subTotal,
                Amount = subTotal + deliveryMethod.Cost,
                PaymentMethod = paymentMethod,
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
            order.PaymentStatus = "Pending";

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            await _cartRepository.DeleteBasketAsync(userId);

            return order;
        }


        public async Task<Order> GetOrderByIdAsynce(int id)
        {
            return await _context.Orders.FirstOrDefaultAsync(f=>f.Id==id);
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

        public async Task<Order?> GetOrderByIdForUserAsync(string userId, int orderId)
        {
            return await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
        }



        public async Task<string?> CreateStripeSessionAsync(int orderId)
        {
            var order = await _context.Orders.Include(o => o.OrderItems).FirstOrDefaultAsync(o => o.Id == orderId);
            if (order == null) return null;

            var client = new StripeClient(_stripeSettings.SecretKey);
            var service = new SessionService(client);

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                LineItems = order.OrderItems.Select(i => new SessionLineItemOptions
                {
                    Quantity = i.Quantity,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        UnitAmountDecimal = (long)(i.Price * 100),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"Product {i.ProductId}"
                        }
                    }
                }).ToList(),
                SuccessUrl = $"https://localhost:4200/payment-success?sessionId={{CHECKOUT_SESSION_ID}}&orderId={orderId}",
                CancelUrl = "https://localhost:4200/cart",
                Metadata = new Dictionary<string, string>
        {
            { "orderId", orderId.ToString() }
        }
            };

            var session = await service.CreateAsync(options);
            return session.Url;
        }


        public async Task<bool> ConfirmStripeOrderAsync(string sessionId, string userId)
        {
            var service = new SessionService();
            var session = await service.GetAsync(sessionId);

            if (session.PaymentStatus == "paid")
            {
                var order = await _context.Orders
                    .Where(o => o.UserId == userId && o.PaymentStatus == "Pending")
                    .OrderByDescending(o => o.Date)
                    .FirstOrDefaultAsync();

                if (order == null) return false;

                order.PaymentStatus = "Paid";
                order.Status = "Confirmed";
                await _context.SaveChangesAsync();
                return true;
            }

            return false;
        }


    }
}
