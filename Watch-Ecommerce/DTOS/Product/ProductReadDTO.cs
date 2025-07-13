using ECommerce.Core.Enumerators;
using Watch_Ecommerce.DTOS.Color;
using Watch_Ecommerce.DTOS.Size;

namespace Watch_Ecommerce.DTOS.Product
{
    public class ProductReadDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string GenderCategory { get; set; }
        public bool WaterResistance { get; set; }
        public int WarrentyYears { get; set; }
        public List<string> Colors { get; set; }
        public List<string> Sizes { get; set; }
    }
}
