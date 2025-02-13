using System.Threading.Tasks;
using SimpleAIChatbot.Client.Models;

namespace SimpleAIChatbot.Client.Services.Contracts
{
    public interface IChatbotService
    {
        Task<string> AskChatbot(string message);
        Task<string> GetConversations();
    }
}
