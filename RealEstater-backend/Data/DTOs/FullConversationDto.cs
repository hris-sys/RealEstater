namespace RealEstater_backend.Data.DTOs
{
    public class FullConversationDto
    {
        public FullConversationDto()
        {
            this.Replies = new List<ReplyDto>();
        }
        public int ConversationId { get; set; }
        public List<ReplyDto> Replies { get; set; }
    }
}
