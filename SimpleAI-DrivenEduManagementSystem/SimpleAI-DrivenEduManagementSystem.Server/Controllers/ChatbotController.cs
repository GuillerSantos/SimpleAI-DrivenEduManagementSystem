using Microsoft.AspNetCore.Mvc;
using SimpleAI_DrivenEduManagementSystem.Server.Services;

[ApiController]
[Route("api/chatbot")]
public class ChatbotController : ControllerBase
{
    private readonly DatabaseService _databaseService;

    public ChatbotController(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    [HttpGet("ping")]
    public IActionResult Ping()
    {
        try
        {
            var database = _databaseService.GetDatabase();
            return Ok(new { Status = "Connected", DatabaseName = database.DatabaseNamespace.DatabaseName });
        }
        catch
        {
            return BadRequest(new { Status = "Failed to connect" });
        }
    }
}
