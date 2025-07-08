using ECommerce.Core.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NuGet.ContentModel;
using Stripe;
using Stripe.Checkout;
using Watch_Ecommerce.Helpers;
using Watch_EcommerceBl.Interfaces;

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

        public async Task<string?> CreateStripeSessionAsync(string userId, int deliveryMethodId)
        {
            var basket = await _cartRepository.GetBasketAsync(userId);
            if (basket == null || !basket.Items.Any())
                return null;

            var deliveryMethod = await Context.Deliverymethods.FindAsync(deliveryMethodId);
            if (deliveryMethod == null)
                return null;

            var client = new StripeClient(_stripeSettings.SecretKey);
            var service = new SessionService(client);

            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                Mode = "payment",
                LineItems = basket.Items.Select(item => new SessionLineItemOptions
                {
                    Quantity = item.Quantity,
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "egp",
                        UnitAmountDecimal = (long)(item.Price * 100),
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Name
                        }
                    }
                }).ToList(),
                SuccessUrl = $"https://localhost:4200/payment-success?sessionId={{CHECKOUT_SESSION_ID}}",
                CancelUrl = "https://localhost:4200/cart"
            };

            var session = await service.CreateAsync(options);
            return session.Url;
        }
    }
}
