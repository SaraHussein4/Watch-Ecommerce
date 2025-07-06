using ECommerce.Core.model;
using Microsoft.AspNetCore.Http.HttpResults;
using Stripe;
using Watch_EcommerceBl.Interfaces;
using Watch_EcommerceBl.Repositories;
using Watch_EcommerceDAL.Models;

namespace Watch_Ecommerce.Services
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IConfiguration configuration;
        private readonly CartRepositry cartRepositry;
        private readonly IUnitOfWorks unitOfWorks;

        public PaymentServices(IConfiguration configuration,CartRepositry cartRepositry,IUnitOfWorks unitOfWorks)
        {
            this.configuration = configuration;
            this.cartRepositry = cartRepositry;
            this.unitOfWorks = unitOfWorks;
        }
        public async Task<CustomerBasket?> CreateOrUpdatePayment(string BasketId)
        {
            //secret key
            StripeConfiguration.ApiKey = configuration["StripeKeys:Secretkey"];
            //get basket
            var Basket= await cartRepositry.GetBasketAsync(BasketId);
            if (Basket == null) return null;  //decimal
            var ShipingPrice = 0M;
            if (Basket.DeliveryMethodId.HasValue)
            {
                var DeliveryMethod= await unitOfWorks.DeliveryMethodRepository.GetByIdAsync(Basket.DeliveryMethodId.Value);
                ShipingPrice = DeliveryMethod.Cost;
            }
            //total= subtotal+dm.cost
            if(Basket.Items.Count > 0)
            {
                foreach (var item in Basket.Items)
                {
                     var Product= await unitOfWorks.productrepo.GetByIdAsync(item.id);
                    if(item.Price !=  Product.Price) 
                        item.Price = Product.Price;
                }
            }
            var SubTotal= Basket.Items.Sum(s=>s.Price * s.Quantity);
            //create payment
            //make object from packge
            var Services = new PaymentIntentService();
            PaymentIntent paymentIntent; 
            if (string.IsNullOrEmpty(Basket.PaymentInterntId))
            {//create
                var Options = new PaymentIntentCreateOptions()
                {
                    Amount = (long)(SubTotal * 100 + ShipingPrice * 100),
                    Currency = "EGP",
                    PaymentMethodTypes= new List<string>() { "card"}
                };
                paymentIntent= await Services.CreateAsync(Options);
                Basket.PaymentInterntId=paymentIntent.Id;
                Basket.ClientSecret=paymentIntent.ClientSecret;
            }
            else
            {//update
                var Options = new PaymentIntentUpdateOptions()
                {
                    // only amount because other knows already
                    Amount = (long)(SubTotal * 100 + ShipingPrice * 100),
                };
                paymentIntent = await Services.UpdateAsync(Basket.PaymentInterntId, Options);
                Basket.PaymentInterntId = paymentIntent.Id;
                Basket.ClientSecret = paymentIntent.ClientSecret;
            }
          await  cartRepositry.UpdateBasketAsync(Basket);
            return Basket;
        }
    }
}
