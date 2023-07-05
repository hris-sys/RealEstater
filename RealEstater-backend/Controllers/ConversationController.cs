using Microsoft.AspNetCore.Mvc;
using RealEstater_backend.Data.DTOs;
using RealEstater_backend.Services.Interfaces;
using RealEstater_backend.Repositories.Interfaces;

namespace RealEstater_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConversationController : ControllerBase
    {
        private readonly IConversationRepository conversationRepository;
        private readonly IConversationService conversationService;

        public ConversationController(IConversationRepository conversationRepository, IConversationService conversationService)
        {
            this.conversationRepository = conversationRepository;
            this.conversationService = conversationService;
        }

        [HttpPost("sendMessage")]
        public async Task<IActionResult> SendMessage([FromBody] SendMessageDto messageDto)
        {
            this.conversationService.SendMessage(messageDto.Message, messageDto.UserFrom, messageDto.UserTo);
            return Ok();
        }

        [HttpGet("getConversation/{id}")]
        public async Task<IActionResult> GetConversation(int id)
        {
            return Ok(this.conversationRepository.GetFullConversationFromId(id));
        }

        [HttpGet("getAllUserConversations/{email}")]
        public async Task<IActionResult> GetAllUserConversations(string email)
        {
            return Ok(this.conversationService.GetAllUserConversations(email));
        }
    }
}
