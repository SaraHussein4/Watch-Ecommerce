using ECommerce.Core.Enumerators;
using System.Drawing;
using Watch_EcommerceDAL.Models;

namespace Watch_Ecommerce.DTOs.Product
{
    public class ProductCreateDTO
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string Status { get; set; }
        public string GenderCategory { get; set; }
        public bool WaterResistance { get; set; }
        public int WarrentyYears { get; set; }

        public string Colors { get; set; }
        public string Sizes { get; set; }

        // Foreign Key
        public int ProductBrandId { get; set; }
        public int CategoryId { get; set; }


        // images
        public  IFormFileCollection Images { get; set; }
    }
}
