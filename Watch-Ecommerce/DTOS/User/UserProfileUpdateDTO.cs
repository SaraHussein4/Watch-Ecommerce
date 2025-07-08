using Watch_Ecommerce.DTOS.Address;

namespace Watch_Ecommerce.DTOS.User
{
    public class UserProfileUpdateDTO
    {
        public string Name { get; set; }
        public string PhoneNumber { get; set; }

        public List<AddressDTO> Addresses { get; set; }
    }
}
