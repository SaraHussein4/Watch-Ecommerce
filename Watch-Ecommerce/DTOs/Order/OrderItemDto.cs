namespace Watch_Ecommerce.DTOs.Order
{
    public class OrderItemDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string image { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Amount { get; set; }
    }
}
