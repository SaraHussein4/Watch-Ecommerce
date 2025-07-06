using ECommerce.Core.model;
using Watch_EcommerceDAL.Models;

namespace Watch_Ecommerce.Services
{
    public interface IPaymentServices
    {
        //fun to create or update payment intern
        Task<CustomerBasket?> CreateOrUpdatePayment(string BasketId);


    }
}
