using Watch_Ecommerce.DTOS.Product;

namespace Watch_Ecommerce.DTOS.ProductBrand
{
    public class ProductBrandReadDTO
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public virtual ICollection<ProductReadDTO> Products { get; set; }
    }
}
