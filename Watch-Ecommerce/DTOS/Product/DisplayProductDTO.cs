using Watch_Ecommerce.DTOS.Category;
using Watch_Ecommerce.DTOS.Product;
using Watch_Ecommerce.DTOS.ProductBrand;

namespace Watch_Ecommerce.DTOs.Product
{
    public class DisplayProductDTO : ProductReadDTO
    {
        public int CategoryId { get; set; }
        public int ProductBrandId { get; set; }
        public List<ImageDTO> Images { get; set; } = new List<ImageDTO>();
        public ProductBrandWithoutProductsCollectioDTO ProductBrand { get; set; }
        public CategoryDTO Category {  get; set; }
    }
}
