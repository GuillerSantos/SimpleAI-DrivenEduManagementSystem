using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SimpleAIChatbot.Server.Services.Contracts;

namespace SimpleAIChatbot.Server.Services
{
    public class ChatbotService : IChatbotService
    {
        private  Dictionary<string, List<string>> _conversations;
        private readonly ILogger<ChatbotService> _logger;

        public ChatbotService(ILogger<ChatbotService> logger)
        {
            _logger = logger;
            _conversations = new Dictionary<string, List<string>>();

            try
            {
                Task.Run(async () => _conversations = await LoadConversationsAsync("conversations.txt")).Wait();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load chatbot conversations.");
            }
        }

        private async Task<Dictionary<string, List<string>>> LoadConversationsAsync(string filePath)
        {
            _logger.LogInformation("Loading chatbot conversations...");
            var conversations = new Dictionary<string, List<string>>();

            try
            {
                var lines = await File.ReadAllLinesAsync(filePath);
                foreach (var line in lines)
                {
                    var parts = line.Split('|');
                    if (parts.Length == 2)
                    {
                        var key = parts[0].Trim().ToLowerInvariant();
                        var value = parts[1].Trim();

                        if (!conversations.ContainsKey(key))
                            conversations[key] = new List<string>();

                        conversations[key].Add(value);

                        _logger.LogInformation($"Loaded: {key} -> {value}");
                    }
                    else
                    {
                        _logger.LogWarning($"Invalid line format: {line}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading chatbot conversations.");
            }

            return conversations;
        }

        public Task<string> GetResponseAsync(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return Task.FromResult("I don't understand.");

            var normalizedInput = input.Trim().ToLowerInvariant();
            return Task.FromResult(FindBestResponse(normalizedInput));
        }

        private string FindBestResponse(string normalizedInput)
        {
            // Exact match first
            if (_conversations.TryGetValue(normalizedInput, out var responses))
            {
                return responses[new Random().Next(responses.Count)]; // Random response variation
            }

            // Fuzzy matching
            int bestDistance = int.MaxValue;
            string bestMatch = null;

            foreach (var key in _conversations.Keys)
            {
                int distance = LevenshteinDistance(key, normalizedInput);
                if (distance < bestDistance)
                {
                    bestDistance = distance;
                    bestMatch = key;
                }
            }

            // Dynamic threshold based on input length
            int threshold = Math.Max(2, (int)(normalizedInput.Length * 0.3));

            if (bestMatch != null && bestDistance <= threshold)
            {
                return _conversations[bestMatch][new Random().Next(_conversations[bestMatch].Count)];
            }

            return "I don't understand.";
        }

        private int LevenshteinDistance(string s, string t)
        {
            int n = s.Length, m = t.Length;
            if (n == 0) return m;
            if (m == 0) return n;

            int[,] d = new int[n + 1, m + 1];

            for (int i = 0; i <= n; i++) d[i, 0] = i;
            for (int j = 0; j <= m; j++) d[0, j] = j;

            for (int i = 1; i <= n; i++)
                for (int j = 1; j <= m; j++)
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + (s[i - 1] == t[j - 1] ? 0 : 1)
                    );

            return d[n, m];
        }
    }
}
