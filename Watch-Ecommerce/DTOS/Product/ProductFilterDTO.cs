namespace Watch_Ecommerce.DTOS.Product
{
    public class ProductFilterDTO
    {
        public string? SearchTerm { get; set; }

        public string SortBy { get; set; } = "newest"; // Default value

        public List<string> Genders { get; set; } = new List<string> { "male", "female" }; // Default values

        public List<int> CategoryIds { get; set; } = new List<int>();

        public List<int> BrandIds { get; set; } = new List<int>();

        public int Page { get; set; } = 1; // Default value

        public int PageSize { get; set; } = 10; // Default value
    }
}
