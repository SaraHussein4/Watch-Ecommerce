namespace Watch_Ecommerce.DTOS.User
{
    public class UserProfileReadDTO :UserProfileUpdateDTO
    {
        public string Id { get; set; }
        public string Email { get; set; }

        public int? GovernorateId { get; set; }
        public string GovernorateName { get; set; }

    }
}
