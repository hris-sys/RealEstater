using RealEstater_backend.Data.DTOs;
using RealEstater_backend.Data.Models;

namespace RealEstater_backend.Repositories.Interfaces
{
    public interface IConversationRepository : IGenericRepository<ConversationModel>
    {
        List<ConversationModel> GetAllConversations();
        FullConversationDto GetFullConversationFromId(int id);
    }
}
