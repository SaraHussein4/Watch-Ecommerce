using Watch_Ecommerce.DTOS.Product;

namespace Watch_Ecommerce.DTOs.Product
{
    public class DisplayProductDTO : ProductReadDTO
    {
        public string CategoryName { get; set; }
        public string ProductBrandName { get; set; }
    }
}
