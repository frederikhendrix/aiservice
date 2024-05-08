using AiService.Models;

namespace AiService.Interfaces
{
    public interface IPromptService
    {
        /// <summary>
        /// Generates a response from a given prompt request.
        /// </summary>
        /// <param name="request">The prompt request containing the model and prompt text.</param>
        /// <returns>A task that represents the asynchronous operation, with a result of PromptResponse.</returns>
        Task<string> GenerateResponseAsync(string prompt);

        Task<string> CheckServerAvailability();

    }
}
