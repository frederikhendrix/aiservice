using AiService.Interfaces;
using AiService.Models;
using Microsoft.AspNetCore.Mvc;

namespace AiService.Controllers
{
        [ApiController]
        [Route("[controller]")]
        public class PromptController : ControllerBase
        {
            private readonly IPromptService _promptService;

            public PromptController(IPromptService promptService)
            {
                _promptService = promptService;
            }

        [HttpGet("check-server")]
        public async Task<IActionResult> CheckServer()
        {
            var result = await _promptService.CheckServerAvailability();
            return Ok(result);
        }

        [HttpPost]
            public async Task<IActionResult> Post(string request)
            {
                var response = await _promptService.GenerateResponseAsync(request);
                return Ok(response);
            }
        }
}
