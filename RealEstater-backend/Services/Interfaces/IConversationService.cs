using RealEstater_backend.Data.DTOs;

namespace RealEstater_backend.Services.Interfaces
{
    public interface IConversationService
    {
        void SendMessage(string message, string userFrom, string userTo);
        List<ConversationDto> GetAllUserConversations(string email);
    }
}
