using Watch_Ecommerce.DTOS.Product;

namespace Watch_Ecommerce.DTOs.Product
{
    public class DisplayProductDTO : ProductReadDTO
    {
        public int CategoryId { get; set; }
        public int ProductBrandId { get; set; }
        public List<ImageDTO> Images { get; set; } = new List<ImageDTO>();
    }
}
