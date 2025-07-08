namespace Watch_Ecommerce.DTOS.ImageDTO
{
    public class AddImgDto
    {
        public int Id { get; set; }
        public bool isPrimary { get; set; }
        public List<IFormFile> Images { get; set; }
        public int ProductId { get; set; }
    }
}
