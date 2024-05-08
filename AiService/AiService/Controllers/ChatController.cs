using AiService.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AiService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ChatRequest request)
        {
            var result = await _chatService.SendMessageAsync(request.model, request.messages);
            if (string.IsNullOrEmpty(result))
            {
                return BadRequest("Failed to receive a valid response from the chat service.");
            }

            return Ok(new { message = result });
        }
    }

    public class ChatRequest
    {
        public string model { get; set; }
        public List<MessageStructure> messages { get; set; }
    }

    public class MessageStructure
    {
        public string role { get; set; }
        public string content { get; set; }
    }
}
