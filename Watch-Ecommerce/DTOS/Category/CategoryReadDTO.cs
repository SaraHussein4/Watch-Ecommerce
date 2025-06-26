using Watch_Ecommerce.DTOS.Product;

namespace Watch_Ecommerce.DTOS.Category
{
    public class CategoryReadDTO
    {
        public string Name { get; set; }
        public virtual ICollection<ProductReadDTO> Products { get; set; }

    }
}
