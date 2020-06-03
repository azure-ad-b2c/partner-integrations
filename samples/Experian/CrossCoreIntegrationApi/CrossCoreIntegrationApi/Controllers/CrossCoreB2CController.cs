namespace CrossCoreIntegrationApi.Controllers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Models;
    using Models.Configuration;
    using Services;

    [Route("api/[controller]")]
    [ApiController]
    public class CrossCoreB2CController : ControllerBase
    {
        private readonly IOptions<CrossCoreConfig> _config;
        private readonly ILogger<CrossCoreB2CController> _logger;
        private readonly IService _service;

        public CrossCoreB2CController(
            ILogger<CrossCoreB2CController> logger,
            IOptions<CrossCoreConfig> config,
            IService crossCoreService
        )
        {
            _logger = logger;
            _config = config;
            _service = crossCoreService;
        }

        [HttpPost]
        [Route("CrossCore")]
        public async Task<IActionResult> CrossCore([FromBody] CrossCoreB2CInput b2CInput)
        {
            var output = await _service.ServiceCall(b2CInput);

            if (!output.Decision.Equals("CONTINUE"))
            {
                return Conflict(new B2CResponse {UserMessage = $"Your identity could not be verified based on the details you provided. Score: {output.Score}" });
            }

            return Ok(output);
        }
    }
}