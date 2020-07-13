using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Onfido.Api.Model;
using Onfido.Api.Services;

namespace Onfido.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    ///<summary>
    ///This controller contains endpoints that recieve and format requests from B2C and calls the OnFido Endpoints with the built in API key and returns type-safe results to B2C
    /// </summary>
    public class OnfidoController : ControllerBase
    {
        public readonly HttpService httpService;
        public readonly OnfidoSettings onfidoSettings;
        public OnfidoController(HttpService httpService, IOptions<OnfidoSettings> optionOnfidoSettings)
        {
            this.httpService = httpService;
            this.onfidoSettings = optionOnfidoSettings.Value;
        }

        /// <summary>
        /// This method is called by the B2C IdentityExperienceFramework at the start of a verification transaction to create an instance of the user with OnFido
        /// Recieves user details and issues a user Id for the particular user along with the saved details.
        /// </summary>
        /// <param name="input">Receives the first and last name of the registered user</param>
        /// <returns>InitializeUserOutput contains a user Id assigned by OnFido along with the user details</returns>
        [HttpPost]
        public async Task<IActionResult> Initialize(InitializeUserInput input)
        {
            if (input == null || !input.Validate())
            {
                return Conflict(new B2CErrorResponseContent("Cannot deserialize input claims"));
            }

            var response = await httpService.PostAsync<InitializeUserOutput>($"{onfidoSettings.BaseUrl}/v3/applicants", input);

            if(!response.Status)
            {
                return Conflict(new B2CErrorResponseContent(response.Message));
            }
            return Ok(response.Data);

        }

        /// <summary>
        /// This method is called by the B2C IdentityExperienceFramework to request for a User Specific SDK token. 
        /// This token will be passed to the front end script later where it will be used to authenticate the user and the referrer location, and has a maximum lifetime of 90 minutes.
        /// This approach of generating short-lived SDK tokens is used to prevent exposing the API key to the front end scripts, and potential key leakage
        /// </summary>
        /// <param name="input">Contains the previously assigned Applicant Id along with a referrer parameter. 
        /// The referrer parameter is mandatory and it ensures that the issued token is only valid if the referrer header of requests which use this key match the referrer it was issued for</param>
        /// <returns>GenerateTokenOutput contains the SDK token</returns>
        [HttpPost]
        public async Task<IActionResult> GenerateSdkToken(GenerateSdkTokenInput input)
        {
            if (input == null || !input.Validate())
            {
                return Conflict(new B2CErrorResponseContent("Cannot deserialize input claims"));
            }

            var response = await httpService.PostAsync<GenerateSdkTokenOutput>($"{onfidoSettings.BaseUrl}/v3/sdk_token", input);

            if (!response.Status)
            {
                return Conflict(new B2CErrorResponseContent(response.Message));
            }

            return Ok(response.Data);
        }

        /// <summary>
        /// Post Upload, OnFido requires that checks be initialized to validate the user.
        /// This endpoint is called by B2C IdentityExperienceFramework to request a check operation
        /// </summary>
        /// <param name="input">Contains the referrer Id of the user as assigned by onfido and optionally, the checks to be performed. By default it checks the document and the selfie</param>
        /// <returns>The CheckObject contains detailed information on the check</returns>
        [HttpPost]
        public async Task<IActionResult> CreateCheck(CreateCheckInput input)
        {
            if (input == null || !input.Validate())
            {
                return Conflict(new B2CErrorResponseContent("Cannot deserialize input claims"));
            }

            var response = await httpService.PostAsync<CheckObject>($"{onfidoSettings.BaseUrl}/v3/checks", input);

            if (!response.Status)
            {
                return Conflict(new B2CErrorResponseContent(response.Message));
            }
            return Ok(response.Data);
        }

        /// <summary>
        /// Polling endpoint called from the front end script. Has CORS enabled to accept calls from a script embedded in a webpage.
        /// Result is filtered to only contain information relevant to the front end script
        /// </summary>
        /// <param name="input">Contains the reference Href of the user object</param>
        /// <returns>output contains the status of the verification. Polling will continue as long as status remains "in_progress"</returns>
        [HttpPost]
        [EnableCors("PollingPolicy")]
        public async Task<IActionResult> PollCheck(ResultInput input)
        {
            if (input == null || !input.Validate())
            {
                return Conflict("An error has occured. Please retry.");
            }

            var response = await httpService.GetAsync<CheckObject>($"{onfidoSettings.BaseUrl}{input.href}");

            if (!response.Status)
            {
                return Conflict(response.Message);
            }

            var output = new {
                response.Data.status
            };

            return Ok(output);
        }

        /// <summary>
        /// Verification endpoint called by B2C to confirm that the documents of the user have been successfully verified. 
        /// This endpoint returns all the information recieved from the check object
        /// </summary>
        /// <param name="input">Contains the Href parameter which references the onFido</param>
        /// <returns>entire CheckObject</returns>
        [HttpPost]
        public async Task<IActionResult> VerifyResults(ResultInput input)
        {
            if (input == null || !input.Validate())
            {
                return Conflict(new B2CErrorResponseContent("Cannot deserialize input claims"));
            }

            var response = await httpService.GetAsync<CheckObject>($"{onfidoSettings.BaseUrl}{input.href}");

            if (!response.Status)
            {
                return Conflict(new B2CErrorResponseContent(response.Message));
            }
            
            return Ok(response.Data);
        }
    }
}