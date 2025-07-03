using StackExchange.Redis;

namespace Watch_Ecommerce.DTOS.User
{
    public class UserReadDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
