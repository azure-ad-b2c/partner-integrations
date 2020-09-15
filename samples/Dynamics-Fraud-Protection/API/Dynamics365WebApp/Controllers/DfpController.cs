using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dynamics365WebApp.Models;
using Dynamics365WebApp.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Dynamics365WebApp.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class DfpController : ControllerBase
    {
        public readonly DfpService dfpService;
        public DfpController(DfpService dfpService)
        {
            this.dfpService = dfpService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccount(DfpCreateAccountInputClaims input)
        {
            if (input == null || !input.Validate())
            {
                return Conflict(new B2CErrorResponseContent("Cannot deserialize input claims"));
            }

            var correlationId = dfpService.NewCorrelationId;
            var signUpId = dfpService.NewCorrelationId;

            var response = await dfpService.CreateAccount(input, correlationId, signUpId);

            if (!response.Status)
            {
                return Conflict(new B2CErrorResponseContent(response.Message, $"Correlation Id : {correlationId}"));
            }

            var result = response.Data?.ResultDetails?.FirstOrDefault();

            if (result == null)
            {
                return Conflict(new B2CErrorResponseContent(response.Message, $"Correlation Id : {correlationId}"));
            }

            var botScore = result.Scores.FirstOrDefault(x => x.ScoreType == "Bot")?.ScoreValue ?? 0;
            var riskScore = result.Scores.FirstOrDefault(x => x.ScoreType == "Bot")?.ScoreValue ?? 0;

            return Ok(new DfpCreateAccountOutputClaims() { CorrelationId = correlationId, SignUpId = signUpId, Decision = result.Decision, BotScore = botScore, RiskScore = riskScore });
        }

        [HttpPost]
        public async Task<IActionResult> CreateAccountStatus(DfpCreateAccountStatusInputClaims input)
        {
            if (input == null || !input.Validate())
            {
                return Conflict(new B2CErrorResponseContent("Cannot deserialize input claims"));
            }

            var correlationId = dfpService.NewCorrelationId;

            var response = await dfpService.CreateAccountStatus(input, correlationId);

            if (!response.Status)
            {
                return Conflict(new B2CErrorResponseContent(response.Message, $"Correlation Id : {correlationId}"));
            }

            return Ok(new DfpCreateAccountStatusOutputClaims() { CorrelationId = correlationId });
        }

        [HttpPost]
        public async Task<IActionResult> LoginAccount(DfpLoginAccountInputClaims input)
        {
            if (input == null || !input.Validate())
            {
                return Conflict(new B2CErrorResponseContent("Cannot deserialize input claims"));
            }

            var correlationId = dfpService.NewCorrelationId;
            var loginId = dfpService.NewCorrelationId;

            var response = await dfpService.LoginAccount(input, correlationId, loginId);

            if (!response.Status)
            {
                return Conflict(new B2CErrorResponseContent(response.Message, $"Correlation Id : {correlationId}"));
            }

            var result = response.Data?.ResultDetails?.FirstOrDefault();

            if (result == null)
            {
                return Conflict(new B2CErrorResponseContent(response.Message, $"Correlation Id : {correlationId}"));
            }

            var botScore = result.Scores.FirstOrDefault(x => x.ScoreType == "Bot")?.ScoreValue ?? 0;
            var riskScore = result.Scores.FirstOrDefault(x => x.ScoreType == "Bot")?.ScoreValue ?? 0;

            return Ok(new DfpLoginAccountOutputClaims() { CorrelationId = correlationId, LoginId = loginId, Decision = result.Decision, BotScore = botScore, RiskScore = riskScore });
        }

        [HttpPost]
        public async Task<IActionResult> LoginAccountStatus(DfpLoginAccountStatusInputClaims input)
        {
            if (input == null || !input.Validate())
            {
                return Conflict(new B2CErrorResponseContent("Cannot deserialize input claims"));
            }

            var correlationId = dfpService.NewCorrelationId;

            var response = await dfpService.LoginAccountStatus(input, correlationId);

            if (!response.Status)
            {
                return Conflict(new B2CErrorResponseContent(response.Message, $"Correlation Id : {correlationId}"));
            }

            return Ok(new DfpCreateAccountStatusOutputClaims() { CorrelationId = correlationId });
        }
    }
}