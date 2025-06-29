using System.ComponentModel.DataAnnotations;

namespace Watch_Ecommerce.DTOs.Order
{
    public class OrderAddressDto
    {
        [Required]

        public string FirstName { get; set; }
        [Required]

        public string LastName { get; set; }
        [Required]

        public string City { get; set; }
        [Required]

        public string Street { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Governorate is required.")]

        public int GovernorateId { get; set; }
    }
}
