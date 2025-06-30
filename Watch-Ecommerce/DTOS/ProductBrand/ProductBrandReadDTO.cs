using Watch_Ecommerce.DTOS.Product;

namespace Watch_Ecommerce.DTOS.ProductBrand
{
    public class ProductBrandReadDTO : ProductBrandWithoutProductsCollectioDTO
    {
        public virtual ICollection<ProductReadDTO> Products { get; set; }
    }
}
