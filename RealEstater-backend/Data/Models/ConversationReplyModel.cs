namespace RealEstater_backend.Data.Models
{
    public class ConversationReplyModel : IdModel
    {
        public ConversationModel Conversation { get; set; }
        public UserModel User { get; set; }
        public DateTime Time { get; set; }
        public string Reply { get; set; }
        public bool NeedsReplyFromOne { get; set; }
        public bool NeedsReplyFromTwo { get; set; }
    }
}
