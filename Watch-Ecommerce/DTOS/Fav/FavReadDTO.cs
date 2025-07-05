using Watch_Ecommerce.DTOs.Product;

namespace Watch_Ecommerce.DTOS.Fav
{
    public class FavReadDTO
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public virtual DisplayProductDTO Product { get; set; }

    }
}
