namespace Watch_Ecommerce.DTOS.Order
{
    public class OrderDetailsDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; } 
        public string Status { get; set; }
        public decimal Amount { get; set; }

    }
}
