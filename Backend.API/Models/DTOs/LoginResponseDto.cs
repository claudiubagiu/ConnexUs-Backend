namespace Backend.API.Models.DTOs
{
    public class LoginResponseDto
    {
        public string JwtToken { get; set; }
        public string Email { get; set; }
        public List<string> Roles { get; set; }
    }
}
