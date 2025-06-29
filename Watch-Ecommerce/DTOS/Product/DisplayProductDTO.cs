using Watch_Ecommerce.DTOS.Product;

namespace Watch_Ecommerce.DTOs.Product
{
    public class DisplayProductDTO : ProductReadDTO
    {
        public int CategoryId { get; set; }
        public int ProductBrandId { get; set; }
    }
}
