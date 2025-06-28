using System.ComponentModel.DataAnnotations;

namespace Watch_Ecommerce.DTOs
{
    public class RegisterDto
    {
        [Required(ErrorMessage = "Name Is Required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email Is Required")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }



        [RegularExpression(@"^(011|012|015|010)\d{8}$",
          ErrorMessage = "Phone number is invalid !")]
        [Required(ErrorMessage = "Phone Number Is Required")]

        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Password Is Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Confirm Password Is Required")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Password Doesn't Match")]
        public string ConfirmPassword { get; set; }

        public int BuildingNumber { get; set; }
        public string Street { get; set; }
        public string State { get; set; }
        public bool IsDefault { get; set; }
    }
}
