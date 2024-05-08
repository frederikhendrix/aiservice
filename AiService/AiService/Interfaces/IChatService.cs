using AiService.Controllers;

namespace AiService.Interfaces
{
    public interface IChatService
    {
        Task<string> SendMessageAsync(string model, List<MessageStructure> message);
    }
}
