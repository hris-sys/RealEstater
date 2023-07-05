namespace RealEstater_backend.Data.Models
{
    public class UserModel : IdModel
    {
        public string FirstName { get; set; }
        public string? Token { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime RegisteredAt { get; set; }
        public string? PictureURL { get; set; }
        public string? WebsiteURL { get; set; }
        public Role Role { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
        public string? ResetPasswordToken { get; set; }
        public DateTime ResetPasswordExpiryTime { get; set; }
        public IEnumerable<LandholdingModel>? Landholdings { get; set; }
    }
}
