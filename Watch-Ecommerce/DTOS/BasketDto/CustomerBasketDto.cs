using System.ComponentModel.DataAnnotations;
using Watch_EcommerceDAL.Models;

namespace Watch_Ecommerce.DTOS.BasketDto
{
    public class CustomerBasketDto
    {

        public string? PaymentInterntId { get; set; }
        public string? ClientSecret { get; set; }
        public int? DeliveryMethodId { get; set; }

        [Required]
        public string Id { get; set; }
        public List<CartItem> Items { get; set; }
    }
}
