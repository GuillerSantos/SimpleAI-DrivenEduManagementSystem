using Microsoft.AspNetCore.Mvc;
using SimpleAIChatbot.Server.Models;
using SimpleAIChatbot.Server.Services.Contracts;
using System;
using System.Threading.Tasks;

namespace SimpleAIChatbot.Server.Controllers
{
    [ApiController]
    [Route("api/chat")]
    public class ChatController : ControllerBase
    {
        private readonly IChatbotService _chatbotService;

        public ChatController(IChatbotService chatbotService)
        {
            _chatbotService = chatbotService;
        }

        [HttpPost("ask")]
        public async Task<IActionResult> AskChatbot([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
                return BadRequest(new { Error = "Message cannot be empty" });

            var response = await _chatbotService.GetResponseAsync(request.Message);

            return Ok(new ChatResponse { Response = response });
        }

        [HttpGet("getdata")]
        public IActionResult GetData()
        {
            return Ok(new
            {
                Message = "Hello from the server!",
                Timestamp = DateTime.UtcNow
            });
        }
    }
}
