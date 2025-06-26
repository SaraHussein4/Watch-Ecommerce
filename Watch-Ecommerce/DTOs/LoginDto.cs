using System.ComponentModel.DataAnnotations;

namespace Watch_Ecommerce.DTOs
{
    public class LoginDto
    {
        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email or Password")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]

        public string Password { get; set; }
    }
}
