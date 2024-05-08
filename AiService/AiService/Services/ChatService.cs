using AiService.Controllers;
using AiService.Interfaces;
using System.Text;
using System.Text.Json;

namespace AiService.Services
{
    public class ChatService : IChatService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ChatService> _logger;

        public ChatService(HttpClient httpClient, ILogger<ChatService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<string> SendMessageAsync(string model, List<MessageStructure> message)
        {
            var body = new ChatRequest()
            {
                model = model,
                messages = message
            };

            var json = JsonSerializer.Serialize(body);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("http://host.docker.internal:11434/api/chat", content);
                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("Error: Server responded with status code {StatusCode}", response.StatusCode);
                    return null;
                }

                // Start reading the stream progressively
                using var responseStream = await response.Content.ReadAsStreamAsync();
                using var reader = new StreamReader(responseStream);
                StringBuilder result = new StringBuilder();
                while (!reader.EndOfStream)
                {
                    var currentChunk = await reader.ReadLineAsync();
                    if (!string.IsNullOrEmpty(currentChunk))
                    {
                        try
                        {
                            var jsonElement = JsonDocument.Parse(currentChunk).RootElement;
                            var contentPart = jsonElement.GetProperty("message").GetProperty("content").GetString();
                            result.Append(contentPart);
                        }
                        catch (JsonException jsonEx)
                        {
                            _logger.LogError(jsonEx, "Failed to parse JSON from response chunk");
                            // Optionally continue to the next chunk or handle the error appropriately
                        }
                    }
                }
                return result.ToString();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send message");
                return null;
            }
        }
    }
}
