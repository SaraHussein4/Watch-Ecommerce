using ECommerce.Core.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NuGet.ContentModel;
using Stripe;
using Stripe.Checkout;
using Watch_Ecommerce.Helpers;
using Watch_EcommerceBl.Interfaces;
using Watch_EcommerceDAL.Models;

namespace Watch_Ecommerce.Services
{
    public class PaymentService
    {
        private readonly StripeSettings _stripeSettings;
        private readonly ICartRepository _cartRepository;

        public PaymentService(IOptions<StripeSettings> stripeOptions,ICartRepository cartRepository, TikrContext context)
        {
            _stripeSettings = stripeOptions.Value;
            _cartRepository = cartRepository;
            Context = context;
        }

        public TikrContext Context { get; }

        public async Task<string?> CreateStripeSessionAsync(string userId, int deliveryMethodId, int governorateId)
        {
            var basket = await _cartRepository.GetBasketAsync(userId);
            if (basket == null || !basket.Items.Any())
                return null;

            var deliveryMethod = await Context.Deliverymethods.FindAsync(deliveryMethodId);
            if (deliveryMethod == null)
                return null;

            var governorate = await Context.Governorates.FindAsync(governorateId);
            if (governorate == null)
                return null;

            decimal deliveryCost = governorate.DeliveryCost + deliveryMethod.Cost;
            long deliveryCostInCents = (long)(deliveryCost * 100);

            var lineItems = new List<SessionLineItemOptions>();

            foreach (var item in basket.Items)
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
                            Name = item.Name,
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
                SuccessUrl = $"https://localhost:4200/payment-success?sessionId={{CHECKOUT_SESSION_ID}}",
                CancelUrl = "https://localhost:4200/cart",
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
                CustomerCreation = "always" 
            };

            var service = new SessionService(new StripeClient(_stripeSettings.SecretKey));
            var session = await service.CreateAsync(options);

            Console.WriteLine($"Created session with total: {session.AmountTotal / 100m} EGP");
            Console.WriteLine($"Includes delivery: {(session.AmountTotal - basket.Items.Sum(i => i.Price * i.Quantity * 100)) / 100m} EGP");

            return session.Url;
        }


      
    }
}
