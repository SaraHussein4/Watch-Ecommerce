using System.ComponentModel.DataAnnotations;

namespace Watch_Ecommerce.DTOs.Order
{
    public class CreatedOrderDto
    {

        [Required]
        public OrderAddressDto ShippingAddress { get; set; }

        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Delivery method is required.")]
        public int DeliveryMethodId { get; set; }
        public string PaymentMethod { get; set; }
    }
}
