namespace RealEstater_backend.Data.DTOs
{
    public class ResetPasswordEmailDto
    {
        public ResetPasswordEmailDto(string recipient, string subject, string content)
        {
            this.Recipient = recipient;
            this.Subject = subject;
            this.Content = content;
        }

        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
