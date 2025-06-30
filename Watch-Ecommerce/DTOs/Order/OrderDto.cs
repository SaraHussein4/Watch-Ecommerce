namespace Watch_Ecommerce.DTOs.Order
{
    public class OrderDto
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Status { get; set; }
        public decimal Amount { get; set; }
        public OrderAddressDto OrderAddress { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
        public DeliverymethodDto Deliverymethod { get; set; }

    }
}
