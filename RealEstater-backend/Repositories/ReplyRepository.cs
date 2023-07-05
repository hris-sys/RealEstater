using RealEstater_backend.Data.Models;
using RealEstater_backend.Data.Database;
using RealEstater_backend.Repositories.Interfaces;

namespace RealEstater_backend.Repositories
{
    public class ReplyRepository : GenericRepository<ConversationReplyModel>, IReplyRepository
    {
        public ReplyRepository(RealEstaterDbContext realEstaterDbContext) : base(realEstaterDbContext)
        {
        }
    }
}
