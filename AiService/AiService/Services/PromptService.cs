using AiService.Interfaces;
using AiService.Models;
using Newtonsoft.Json;
using System.Text;

namespace AiService.Services
{
    public class PromptService : IPromptService
    {
        private readonly HttpClient _httpClient;

        public PromptService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> CheckServerAvailability()
        {
            _httpClient.BaseAddress = new Uri("http://localhost:11434"); // Ensure this is set correctly
            try
            {
                var response = await _httpClient.GetAsync("");
                if (response.IsSuccessStatusCode)
                {
                    // Successfully connected and received a response from the server
                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    // The server responded, but with a non-success status code
                    return "Server responded with error: " + response.StatusCode;
                }
            }
            catch (HttpRequestException ex)
            {
                // An error occurred in processing the request, such as a network connection issue
                return "Failed to connect to server: " + ex.Message;
            }
            catch (Exception ex)
            {
                // General error handling if something else went wrong
                return "An error occurred: " + ex.Message;
            }
        }

        public async Task<string> GenerateResponseAsync(string prompt)
        {
            var requestContent = new StringContent(
                "{\"model\": \"llama3\", \"prompt\":\"" + prompt + "\"}",
                Encoding.UTF8,
                "application/json");

            var response = await _httpClient.PostAsync("http://localhost:11434/api/generate", requestContent);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to get response: {response.StatusCode}");
            }

            // Assuming the response is a stream of JSON objects
            using (var stream = await response.Content.ReadAsStreamAsync())
            using (var reader = new StreamReader(stream))
            {
                StringBuilder completeResponse = new StringBuilder();
                while (!reader.EndOfStream)
                {
                    var line = await reader.ReadLineAsync();
                    completeResponse.AppendLine(line); // Process each line as JSON
                    // Optionally, deserialize here if needed or after accumulating full response
                }
                return completeResponse.ToString();
            }
        }
    }
}
