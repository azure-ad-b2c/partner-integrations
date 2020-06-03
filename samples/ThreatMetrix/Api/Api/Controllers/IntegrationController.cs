namespace Api.Controllers
{
    using System.Threading.Tasks;
    using Authentication;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Models;
    using Services;

    [Route("api/[controller]")]
    [BasicAuth]
    [ApiController]
    public class IntegrationController : Controller
    {
        private readonly ILogger<IntegrationController> _logger;
        private readonly IIntegrationService _sessionQueryService;

        public IntegrationController(
            ILogger<IntegrationController> logger,
            IIntegrationService sessionQueryService)
        {
            _logger = logger;
            _sessionQueryService = sessionQueryService;
        }

        [HttpPost]
        [BasicAuth]
        [Consumes("application/json")]
        [Route("SessionQuery")]
        public async Task<IActionResult> SessionQueryCall([FromBody] SessionQueryServiceInput inputData)
        {
            if (inputData == null)
            {
                return BadRequest("Null Parameters received");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest("One or more parameters received are not valid");
            }

            var output = await _sessionQueryService.GetSessionData(inputData);

            if (output.ReviewStatus != "pass")
            {
                return Conflict(new B2CResponse { UserMessage = $"Your identity could not be verified based on the details you provided." });
            }

            return Json(output);
        }
    }
}