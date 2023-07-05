namespace RealEstater_backend.Data.DTOs
{
    public class ConversationDto
    {
        public int ConversationId { get; set; }
        public int UserOneId { get; set; }
        public string UserOneName { get; set; }
        public string UserOneEmail { get; set; }
        public int UserTwoId { get; set; }
        public string UserTwoName { get; set; }
        public string UserTwoEmail { get; set; }
        public string LastMessage { get; set; }
        public string PictureUserOne { get; set; }
        public string PictureUserTwo { get; set; }
        public DateTime LastMessageSentOn { get; set; }
        public bool HasNewMessage { get; set; }
    }
}
