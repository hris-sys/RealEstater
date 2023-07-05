namespace RealEstater_backend.Data.DTOs
{
    public class UpdateUserDto
    {
        public string? Email { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? PictureURL { get; set; }
        public string? WebsiteURL { get; set; }
    }
}
