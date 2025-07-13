namespace Watch_Ecommerce.DTOS.Address
{
    public class AddressDTO
    {
        public int? Id { get; set; } 
        public string Street { get; set; }
        public string State { get; set; }
        public int BuildingNumber { get; set; }
        public bool IsDefault { get; set; }

    }
}
