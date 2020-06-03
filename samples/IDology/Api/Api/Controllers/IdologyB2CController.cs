namespace Api.Controllers
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
    public class IdologyB2CController : ControllerBase
    {
        private readonly IOptions<IdologyConfig> _config;
        private readonly ILogger<IdologyB2CController> _logger;
        private readonly IIdologyService _service;

        public IdologyB2CController(
            ILogger<IdologyB2CController> logger,
            IOptions<IdologyConfig> config,
            IIdologyService service
        )
        {
            _logger = logger;
            _config = config;
            _service = service;
        }

        [BasicAuth]
        [HttpPost]
        [Route("ExpectId")]
        public async Task<IActionResult> ExpectId([FromBody] ExpectIdInput expectIdInput)
        {
            var output = await _service.ExpectIdCall(expectIdInput);

            if (!output.Success)
            {
                return Conflict(new B2CResponse() {UserMessage = "Your identity could not be verified based on the details you provided." });
            }

            return Ok(output);
        }
    }
}