namespace RealEstater_backend.Data.Models
{
    public class ConversationModel : IdModel
    {
        public ConversationModel()
        {
            this.Replies = new List<ConversationReplyModel>();
        }
        public UserModel UserOne { get; set; }
        public UserModel UserTwo { get; set; }
        public ConversationStatusModel Status { get; set; }
        public List<ConversationReplyModel> Replies { get; set; }
    }
}
