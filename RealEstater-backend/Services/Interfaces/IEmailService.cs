using RealEstater_backend.Data.DTOs;

namespace RealEstater_backend.Services.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(ResetPasswordEmailDto email);
    }
}
