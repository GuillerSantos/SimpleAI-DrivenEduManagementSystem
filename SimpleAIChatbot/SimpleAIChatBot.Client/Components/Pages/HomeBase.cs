using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using SimpleAIChatbot.Client.Services.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleAIChatbot.Client.Components.Pages
{
    public class HomeBase : ComponentBase
    {
        [Inject] public IChatbotService chatbotService { get; set; } = default!;

        public string UserMessage { get; set; } = "";
        public List<(string sender, string message)> ChatHistory { get; set; } = new();

        public async Task HandleKeyDownAsync(KeyboardEventArgs args)
        {
            if (args.Key == "Enter")
            {
                await SendMessageAsync();
            }
        }

        public async Task SendMessageAsync()
        {
            if (string.IsNullOrWhiteSpace(UserMessage)) return;

            ChatHistory.Add(("You", UserMessage));
            var response = await chatbotService.AskChatbot(UserMessage);
            ChatHistory.Add(("Bot", response));

            UserMessage = ""; // Clear input field
            StateHasChanged(); // Refresh UI
        }
    }
}