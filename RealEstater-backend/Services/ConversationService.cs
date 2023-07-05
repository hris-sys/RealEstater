using RealEstater_backend.Data.DTOs;
using RealEstater_backend.Data.Models;
using RealEstater_backend.Services.Interfaces;
using RealEstater_backend.Repositories.Interfaces;

namespace RealEstater_backend.Services
{
    public class ConversationService : IConversationService
    {
        private readonly IUserRepository userRepository;
        private readonly IConversationRepository conversationRepository;
        private readonly IStatusRepository statusRepository;
        private readonly IReplyRepository replyRepository;

        public ConversationService(IUserRepository userRepository, IConversationRepository conversationRepository,
                                   IStatusRepository statusRepository, IReplyRepository replyRepository)
        {
            this.userRepository = userRepository;
            this.conversationRepository = conversationRepository;
            this.statusRepository = statusRepository;
            this.replyRepository = replyRepository;
        }

        public void SendMessage(string message, string userFrom, string userTo)
        {
            var userOne = this.userRepository.FindByCondition(x => x.Email ==  userFrom);
            var userTwo = this.userRepository.FindByCondition(x => x.Email == userTo);

            var conversation = this.conversationRepository.GetAllConversations().Where(x => (x.UserOne == userOne && x.UserTwo == userTwo) ||
                                                                                            (x.UserOne == userTwo && x.UserTwo == userOne));

            ConversationReplyModel reply;

            if (!conversation.Any())
            {
                var defaultStatus = this.statusRepository.FindByCondition(x => x.Title == "Created");
                this.conversationRepository.Add(new ConversationModel
                {
                    UserOne = userOne,
                    UserTwo = userTwo,
                    Status = defaultStatus
                });
                this.conversationRepository.SaveChanges();
                var createdConversation = this.conversationRepository.GetAllConversations().OrderBy(x => x.Id);
                reply = CreateReply(createdConversation.LastOrDefault(), message, userOne, true);
            }
            else
                reply = CreateReply(conversation.FirstOrDefault(), message, userOne, true);

            this.replyRepository.Add(reply);
            this.replyRepository.SaveChanges();
        }

        private ConversationReplyModel CreateReply(ConversationModel conversation, string message, UserModel userFrom, bool needsReply)
        {
            return new ConversationReplyModel
            {
                Conversation = conversation,
                Reply = message,
                Time = DateTime.Now,
                User = userFrom,
                NeedsReplyFromOne = !needsReply,
                NeedsReplyFromTwo = needsReply
            };
        }

        public List<ConversationDto> GetAllUserConversations(string email)
        {
            var allConversations =  this.conversationRepository.GetAllConversations().Where(x => (x.UserOne.Email == email) || (x.UserTwo.Email == email)).ToList();
            var user = this.userRepository.FindByCondition(x => x.Email == email);
            var mappedConversations = new List<ConversationDto>();
            
            if (!allConversations.Any())
                return mappedConversations;

            foreach (var conversation in allConversations)
            {
                mappedConversations.Add(new ConversationDto
                {
                    ConversationId = conversation.Id,
                    UserOneName = $"{conversation.UserOne.FirstName} {conversation.UserOne.LastName}",
                    UserTwoName = $"{conversation.UserTwo.FirstName} {conversation.UserTwo.LastName}",
                    UserOneId = conversation.UserOne.Id,
                    UserTwoId = conversation.UserTwo.Id,
                    LastMessage = conversation.Replies.Last().Reply,
                    PictureUserOne = conversation.UserOne.PictureURL,
                    PictureUserTwo = conversation.UserTwo.PictureURL,
                    LastMessageSentOn = conversation.Replies.OrderBy(x => x.Time).FirstOrDefault().Time,
                    HasNewMessage = this.NeedsReply(conversation, user),
                    UserOneEmail = conversation.UserOne.Email,
                    UserTwoEmail = conversation.UserTwo.Email
                });
            }

            return mappedConversations;
        }

        private bool NeedsReply(ConversationModel conversation, UserModel user)
        {
            if (conversation.Replies.Last().User == user)
                return false;
            return true;
        }
    }
}
