using Microsoft.EntityFrameworkCore;
using RealEstater_backend.Data.DTOs;
using RealEstater_backend.Data.Models;
using RealEstater_backend.Data.Database;
using RealEstater_backend.Repositories.Interfaces;

namespace RealEstater_backend.Repositories
{
    public class ConversationRepository : GenericRepository<ConversationModel>, IConversationRepository
    {
        public ConversationRepository(RealEstaterDbContext realEstaterDbContext) : base(realEstaterDbContext)
        {
        }

        public List<ConversationModel> GetAllConversations()
        {
            return this._dbContext.Conversations
               .Include(x => x.UserOne)
               .Include(x => x.UserTwo)
               .Include(x => x.Replies)
               .ToList();
        }

        public FullConversationDto GetFullConversationFromId(int id)
        {
            var result = this._dbContext.Conversations
               .Include(x => x.UserOne)
               .Include(x => x.UserTwo)
               .Include(x => x.Replies)
               .Include(x => x.Status)
               .Where(x => x.Id == id)
               .FirstOrDefault();

            if (result == null)
                return new FullConversationDto();

            var mappedResult = new FullConversationDto();

            mappedResult.ConversationId = result.Id;

            foreach (var reply in result.Replies)
            {
                mappedResult.Replies.Add(new ReplyDto
                {
                    Id = reply.Id,
                    Reply = reply.Reply,
                    Time = reply.Time,
                    UserFullName = $"{reply.User.FirstName} {reply.User.LastName}",
                    UserPictureURL = reply.User.PictureURL!,
                    UserId = reply.User.Id,
                    UserEmail = reply.User.Email!
                });
            }

            return mappedResult;
        }
    }
}
