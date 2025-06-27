using ECommerce.Core.Enumerators;

namespace Watch_Ecommerce.DTOS.Product
{
    public class ProductReadDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public Gender GenderCategory { get; set; }
        public bool WaterResistance { get; set; }
        public int WarrentyYears { get; set; }
        public List<string> Colors { get; set; }
        public List<string> Sizes { get; set; }
    }
}
