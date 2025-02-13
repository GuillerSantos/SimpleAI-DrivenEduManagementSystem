using SimpleAIChatbot.Client.Models;
using SimpleAIChatbot.Client.Services.Contracts;
using System.Net.Http;

namespace SimpleAIChatbot.Client.Services
{
    public class ChatbotService : IChatbotService
    {
        private readonly HttpClient httpClient;

        public ChatbotService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        public async Task<string> AskChatbot(string message)
        {
            var response = await httpClient.PostAsJsonAsync("api/chat/ask", new { Message = message });

            if (!response.IsSuccessStatusCode)
                return "Error: Unable to reach chatbot API.";

            var result = await response.Content.ReadFromJsonAsync<ChatResponse>();
            return result?.Response ?? "I don't understand.";
        }

        public async Task<string> GetConversations()
        {
            return await httpClient.GetStringAsync("api/chat/conversations");
        }
    }
}
