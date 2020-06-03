namespace Api.Controllers
{
    using System.Net;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Models;
    using Models.Configuration;
    using Services;

    [Route("api/[controller]")]
    [ApiController]
    public class IdologyExtIdController : ControllerBase
    {
        private readonly ILogger<IdologyB2CController> _logger;
        private readonly IIdologyService _service;
        private IOptions<IdologyConfig> _config;

        public IdologyExtIdController(
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
        [Route("submit")]
        public async Task<IActionResult> ExpectId([FromBody] ExpectIdExtIdInput expectIdInput)
        {
            var output = await _service.ExpectIdCall(expectIdInput);

            if (!output.Success)
            {
                return BadRequest(new ExtIdResponse("EXPECTIDERR001", output.Error, HttpStatusCode.BadRequest, "ValidationError"));
            }

            return Ok(output);
        }
    }
}