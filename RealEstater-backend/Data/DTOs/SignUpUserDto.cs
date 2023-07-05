using RealEstater_backend.Data.Models;

namespace RealEstater_backend.Data.DTOs
{
    public class SignUpUserDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? Password { get; set; }
    }
}
