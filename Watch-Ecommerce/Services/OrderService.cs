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
        private readonly IUnitOfWorks _unitOfWorks;


        public OrderService(ICartRepository cartRepository,IUnitOfWorks unitOfWorks , TikrContext tikrContext, IOptions<StripeSettings> stripeOptions)
        {
            _cartRepository = cartRepository;
            _unitOfWorks = unitOfWorks;
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

            var governorate = await _context.Governorates.FindAsync(dto.GovernorateId);
            if (governorate == null)
                return null;
            var subTotal = basket.Items.Sum(i => i.Price * i.Quantity);

            var deliveryCost = governorate.DeliveryCost + deliveryMethod.Cost;

            var orderItems = basket.Items.Select(i => new OrderItem
            {
                ProductId = i.id,
                Quantity = i.Quantity,
                Price = i.Price,
                Amount = (int)(i.Price * i.Quantity)
            }).ToList();

            foreach (var item in orderItems)
            {
                var product = await _unitOfWorks.productrepo.GetProductByIdAsync(item.ProductId);
                if (product != null)
                {
                    product.Quantity -= item.Quantity;

                    if (product.Quantity <= 0)
                    {
                        product.Quantity = 0;
                        product.Status = "OutOfStock";
                    }
                }
            }

            var order = new Order
            {
                UserId = userId,
                DeliveryMethodId = deliveryMethodId,
                Status = "Pending",
                Date = DateTime.Now,
                SubTotal = subTotal,
                Amount = subTotal + deliveryCost,
                PaymentMethod = paymentMethod,
                PaymentStatus = "Pending",

                OrderAddress = new OrderAddress
                {
                    FirstName = dto.FirstName,
                    LastName = dto.LastName,
                    City = dto.City,
                    Street = dto.Street,
                    GovernorateId = dto.GovernorateId
                },
                OrderItems = orderItems
            };

            _context.Orders.Add(order);
            await _unitOfWorks.CompleteAsync(); 

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
        public async Task<decimal> GetDeliveryCostAsync(int governorateId, int deliveryMethodId)
        {
            var governorate = await _context.Governorates.FindAsync(governorateId);
            var deliveryMethod = await _context.Deliverymethods.FindAsync(deliveryMethodId);

            decimal governorateCost = governorate?.DeliveryCost ?? 0;
            decimal deliveryMethodCost = deliveryMethod?.Cost ?? 0;

            return governorateCost + deliveryMethodCost;
        }

        public async Task<Order?> GetOrderByIdForUserAsync(string userId, int orderId)
        {
            return await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
        }



        public async Task<string?> CreateStripeSessionAsync(int orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .Include(o => o.OrderAddress)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) return null;

            var deliveryMethod = await _context.Deliverymethods.FindAsync(order.DeliveryMethodId);
            var governorate = await _context.Governorates.FindAsync(order.OrderAddress.GovernorateId);

            if (deliveryMethod == null || governorate == null) return null;

            var deliveryCost = order.Amount - order.SubTotal;
            long deliveryCostInCents = (long)(deliveryCost * 100);

            var client = new StripeClient(_stripeSettings.SecretKey);
            var service = new SessionService(client);

            var lineItems = new List<SessionLineItemOptions>();

            foreach (var item in order.OrderItems)
            {
                lineItems.Add(new SessionLineItemOptions
                {
                    Quantity = item.Quantity,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        UnitAmount = (long)(item.Price * 100),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = $"Product {item.ProductId}",
                            Metadata = new Dictionary<string, string>
                            {
                                {"item_type", "product"}
                            }
                        }
                    }
                });
            }

            lineItems.Add(new SessionLineItemOptions
            {
                Quantity = 1,
                PriceData = new SessionLineItemPriceDataOptions
                {
                    Currency = "egp",
                    UnitAmount = deliveryCostInCents,
                    ProductData = new SessionLineItemPriceDataProductDataOptions
                    {
                        Name = "DELIVERY FEE (REQUIRED)",
                        Description = $"{deliveryMethod.ShortName} to {governorate.Name}",
                        Metadata = new Dictionary<string, string>
                        {
                            {"item_type", "delivery"},
                            {"mandatory", "true"}
                        }
                    }
                }
            });

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                LineItems = lineItems,
                SuccessUrl = $"http://localhost:4200/payment-success?sessionId={{CHECKOUT_SESSION_ID}}&orderId={orderId}",
                CancelUrl = "http://localhost:4200/cart",
                InvoiceCreation = new SessionInvoiceCreationOptions
                {
                    Enabled = true,
                    InvoiceData = new SessionInvoiceCreationInvoiceDataOptions
                    {
                        Description = $"Order total including {deliveryCost} EGP delivery fee",
                        Footer = "Thank you for your purchase!",
                        Metadata = new Dictionary<string, string>
                        {
                            {"show_detailed_breakdown", "true"}
                        }
                    }
                },
                SubmitType = "pay",
                CustomerCreation = "always",
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
            var client = new StripeClient(_stripeSettings.SecretKey);
            var service = new SessionService(client);
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
