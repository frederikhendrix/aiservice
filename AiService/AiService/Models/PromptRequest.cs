namespace AiService.Models
{
    public class PromptRequest
    {
        public string Model { get; set; } = "llama3";
        public string Prompt { get; set; }
    }
}
