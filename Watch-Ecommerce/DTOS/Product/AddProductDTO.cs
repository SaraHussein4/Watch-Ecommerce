using ECommerce.Core.Enumerators;
using Watch_Ecommerce.DTOS.Product;

namespace Watch_Ecommerce.DTOs.Product
{
    public class AddProductDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        //public string Status { get; set; }
        public Gender GenderCategory { get; set; }
        public bool WaterResistance { get; set; }
        public int WarrentyYears { get; set; }
        public List<string> Colors { get; set; }
        public List<string> Sizes { get; set; }
        public int CategoryId { get; set; }
        public int ProductBrandId { get; set; }
        public List<ImageDTO> Images { get; set; } = new List<ImageDTO>();
    }
}
