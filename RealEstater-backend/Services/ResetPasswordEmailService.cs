using MimeKit;
using MailKit.Net.Smtp;
using RealEstater_backend.Data.DTOs;
using RealEstater_backend.Services.Interfaces;

namespace RealEstater_backend.Services
{
    public class ResetPasswordEmailService : IEmailService
    {
        private readonly IConfiguration configuration;

        public ResetPasswordEmailService(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
        public void SendEmail(ResetPasswordEmailDto email)
        {
            var emailMessage = new MimeMessage();
            var from = this.configuration["EmailService:From"];
            emailMessage.From.Add(new MailboxAddress("RealEstater", from));
            emailMessage.To.Add(new MailboxAddress(email.Recipient, email.Recipient));
            emailMessage.Subject = email.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = string.Format(email.Content)
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(this.configuration["EmailService:SmtpServer"], 465, true);
                    client.Authenticate(this.configuration["EmailService:From"], this.configuration["EmailService:Password"]);
                    client.Send(emailMessage);
                }
                catch (Exception)
                {
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                }

            }
        }
    }
}
