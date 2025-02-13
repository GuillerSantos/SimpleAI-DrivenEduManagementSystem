using SimpleAIChatbot.Server.Models;

namespace SimpleAIChatbot.Server.Services.Contracts
{
    public interface IChatbotService
    {
        Task<string> GetResponseAsync(string input);
    }
}