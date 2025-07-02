using Watch_Ecommerce.DTOs.Product;
using Watch_Ecommerce.DTOS.Product;

namespace Watch_Ecommerce.DTOS.ProductBrand
{
    public class ProductBrandReadDTO : ProductBrandWithoutProductsCollectioDTO
    {
        public virtual ICollection<DisplayProductDTO> Products { get; set; }
    }
}
