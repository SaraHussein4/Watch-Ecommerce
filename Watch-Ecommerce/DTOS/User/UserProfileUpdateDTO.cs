using System.ComponentModel.DataAnnotations;
using Watch_Ecommerce.DTOS.Address;

namespace Watch_Ecommerce.DTOS.User
{
    public class UserProfileUpdateDTO
    {
        public string Name { get; set; }
        [RegularExpression(@"^01[0-2,5]\d{8}$", ErrorMessage = "Invalid phone number format.")]
        public string PhoneNumber { get; set; }

        public List<AddressDTO> Addresses { get; set; }
        public int? GovernorateId { get; set; }
    }
}
