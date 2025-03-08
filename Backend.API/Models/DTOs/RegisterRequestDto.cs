using System.ComponentModel.DataAnnotations;

namespace Backend.API.Models.DTOs
{
    public class RegisterRequestDto
    {
        //[Required]
        //[MinLength(3, ErrorMessage = "Username has to be a minimum of 3 characters.")]
        //[MaxLength(50, ErrorMessage = "Username has to be a maximum of 50 characters.")]
        public string UserName { get; set; }
        //[Required]
        //[EmailAddress(ErrorMessage = "Invalid email format.")]
        public string Email { get; set; }
        //[Required]
        //[DataType(DataType.Password)]
        public string Password { get; set; }
        //[Required]
        //[MinLength(3, ErrorMessage = "First name has to be a minimum of 3 characters.")]
        //[MaxLength(50, ErrorMessage = "First name has to be a maximum of 50 characters.")]
        public string FirstName { get; set; }
        //[Required]
        //[MinLength(3, ErrorMessage = "Last name has to be a minimum of 3 characters.")]
        //[MaxLength(50, ErrorMessage = "Last name has to be a maximum of 50 characters.")]
        public string LastName { get; set; }
        //[Required]
        //[Range(18, 100, ErrorMessage = "Age must be at least 18.")]
        public int Age { get; set; }
        public List<string> Roles { get; set; }
    }
}
