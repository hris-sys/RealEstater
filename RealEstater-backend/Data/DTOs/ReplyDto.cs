namespace RealEstater_backend.Data.DTOs
{
    public class ReplyDto
    {
        public int Id { get; set; }
        public string Reply { get; set; }
        public DateTime Time { get; set; }
        public int UserId { get; set; }
        public string UserFullName { get; set; }
        public string UserEmail { get; set; }
        public string UserPictureURL { get; set; }
    }
}