using Jumio.Api.Model;
using Jumio.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Jumio.Api.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class JumioController : ControllerBase
    {
        public readonly HttpService httpService;

        public readonly AppSettings appSettings;

        public readonly JumioSettings jumioSettings;

        private static Lazy<X509SigningCredentials> SigningCredentials;
        public JumioController(HttpService httpService, IOptions<AppSettings> optionAppSettings, IOptions<JumioSettings> optionjumioSettings)
        {
            this.httpService = httpService;
            this.appSettings = optionAppSettings.Value;
            this.jumioSettings = optionjumioSettings.Value;

            SigningCredentials = new Lazy<X509SigningCredentials>(() =>
            {
                var certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                certStore.Open(OpenFlags.ReadOnly);
                var certCollection = certStore.Certificates.Find(
                    X509FindType.FindByThumbprint,
                    appSettings.SigningCertThumbprint,
                    false);

                if (certCollection.Count > 0)
                {
                    return new X509SigningCredentials(certCollection[0]);
                }

                throw new Exception("Certificate not found");
            });
        }

        /// <summary>
        /// This method is for initializing a Jumio transaction and return redirect URL & transaction reference id to the client. 
        /// By accessing the redirect URL, the user can upload their documents.
        /// After uploading the documents, the user will get redirected to the Success URL specified when initiating the transaction.
        /// By using the transaction reference id, the client can fetch the transaction status.
        /// </summary>
        /// <param name="input">It contains the properties to build a success URL and to initialize a transaction.</param>
        /// <returns>InitializeTransactionOutput contains TransactionReference & RedirectUrl</returns>
        [HttpPost]
        public async Task<IActionResult> InitializeTransaction(InitializeTransactionInput input)
        {
            if (input == null || !input.Validate())
            {
                return Conflict(new B2CErrorResponseContent("Cannot deserialize input claims"));
            }

            var jumioInput = new
            {
                customerInternalReference = input.CorrelationId,
                userReference = input.ObjectId,
                successUrl = BuidLink(input)
            };

            var response = await httpService.PostAsync<InitializeTransactionOutput>($"{jumioSettings.BaseUrl}/api/v4/initiate", jumioInput);

            if (!response.Status)
            {
                return Conflict(new B2CErrorResponseContent(response.Message));
            }

            return Ok(response.Data);
        }

        /// <summary>
        /// This method is for initializing the verification process of a jumio transaction. 
        /// It returns a signed JWT containing the transaction reference Id and user's object id.
        /// By using the JWT token, The client can access the VerifyTransactionStatus endpoint to check the transaction status even from an un trusted source.
        /// </summary>
        /// <param name="input">It contains the user's object Id & transaction reference id to be included in the token.</param>
        /// <returns>InitializeVerificationOutput contains the signed JWT token</returns>
        [HttpPost]
        public IActionResult InitializeVerification(InitializeVerificationInput input)
        {
            if (input == null || !input.Validate())
            {
                return Conflict(new B2CErrorResponseContent("Cannot deserialize input claims"));
            }

            var token = new VerificationToken(appSettings) { ObjectId = input.ObjectId, TransactionReference = input.TransactionReference };

            return Ok(new InitializeVerificationOutput() { VerificationToken = token.GenerateToken() });
        }

        /// <summary>
        /// This method is for verifying the status of a jumio transaction.
        /// It requires the signed JWT created using the InitializeVerification endpoint.
        /// The endpoint then decrypts the token and reads the transaction reference id from claims, fetches the status from jumio.
        /// If it's a success - updates the IsVerified field in the JWT token to True.
        /// Finally - sends back the token to the client along with the status (SUCCESS/FAILED/RETRY).
        /// </summary>
        /// <param name="input">It contains the JWT token generated using the InitializeVerification endpoint.</param>
        /// <returns>VerifyTransactionStatusOutput contains the signed JWT token and status</returns>
        [HttpPost]
        public async Task<IActionResult> VerifyTransactionStatus(VerifyTransactionStatusInput input)
        {
            if (input == null || !input.Validate())
            {
                return Conflict(new B2CErrorResponseContent("Cannot deserialize input claims"));
            }

            var token = new VerificationToken(appSettings);
            token.Decrypt(input.VerificationToken);

            var statusResponse = await httpService.GetAsync<JumioTransactionStatus>($"{jumioSettings.BaseUrl}/api/netverify/v2/scans/{token.TransactionReference}");

            if (!statusResponse.Status)
            {
                return Conflict(new B2CErrorResponseContent(statusResponse.Message));
            }
            else if (statusResponse.Data.Status == Constants.JumioTransactionStatus.Pending)
            {
                token.Message = $"The document verification is still pending. (Status {statusResponse.Data.Status})";
                return Ok(new VerifyTransactionStatusOutput() { Status = "RETRY", VerificationToken = token.GenerateToken() });
            }
            else if (statusResponse.Data.Status == Constants.JumioTransactionStatus.Failed)
            {
                token.Message = $"The document uploading has failed. (Status {statusResponse.Data.Status})";
                return Ok(new VerifyTransactionStatusOutput() { Status = "FAILED", VerificationToken = token.GenerateToken() });
            }

            var dataResponse = await httpService.GetAsync<JumioTransactionData>($"{jumioSettings.BaseUrl}/api/netverify/v2/scans/{token.TransactionReference}/data");

            if (!dataResponse.Status)
            {
                return Conflict(new B2CErrorResponseContent(dataResponse.Message));
            }
            else if (dataResponse.Data?.Document?.Status != Constants.JumioDocumentStatus.ApprovedVerified)
            {
                token.Message = dataResponse.Data?.Document == null ? $"Document failed. (Status {dataResponse.Data?.Document?.Status})"
                      : $"Document failed. (Status {dataResponse.Data.Document.Type} - {dataResponse.Data.Document.Status})";

                return Ok(new VerifyTransactionStatusOutput() { Status = "FAILED", VerificationToken = token.GenerateToken() });
            }

            token.Message = $"Document verified successfully. (Status {dataResponse.Data?.Document?.Status})";
            token.IsVerified = true;

            return Ok(new VerifyTransactionStatusOutput() { Status = "SUCCESS", VerificationToken = token.GenerateToken() });
        }

        /// <summary>
        /// This method is for validating the token generated using InitializeVerification or VerifyTransactionStatus endpoint.
        /// The endpoint decrypts the token and reads the IsVerified claim.
        /// If IsVerified=True - The upload to the Jumio is succesful. Else - error.
        /// </summary>
        /// <param name="input">It contains the JWT token, Object Id & transacation reference id.</param>
        /// <returns>ValidateVerificationTokenOutput containing the success flag and message field</returns>
        [HttpPost]
        public IActionResult ValidateVerificationToken(ValidateVerificationTokenInput input)
        {
            if (input == null || !input.Validate())
            {
                return Conflict(new B2CErrorResponseContent("Cannot deserialize input claims"));
            }

            var token = new VerificationToken(appSettings);
            token.Decrypt(input.VerificationToken);

            if (token.ObjectId != input.ObjectId || token.TransactionReference != input.TransactionReference)
            {
                return Conflict(new B2CErrorResponseContent("Invalid Token"));
            }

            return Ok(new ValidateVerificationTokenOutput() { Success = token.IsVerified, Message = string.IsNullOrEmpty(token.Message) ? null : token.Message });
        }

        /// <summary>
        /// This method is for bulding success url for jumio
        /// </summary>
        /// <param name="input">InitializeTransactionInput</param>
        /// <returns></returns>
        private string BuidLink(InitializeTransactionInput input)
        {
            return $"{appSettings.BaseRedirectUrl}/{input.Policy}/oauth2/v2.0/authorize?client_id={input.ClientId}" +
                 $"&redirect_uri={input.RedirectUri}&scope={input.Scope}&response_type=id_token&id_token_hint={BuildIdToken(input)}";
        }

        /// <summary>
        /// This method is for bulding signed JWT token.
        /// </summary>
        /// <param name="input">InitializeTransactionInput</param>
        /// <returns></returns>
        private string BuildIdToken(InitializeTransactionInput input)
        {
            IList<Claim> claims = new List<Claim>();
            claims.Add(new Claim(Constants.IdTokenClaims.ObjectId, input.ObjectId, ClaimValueTypes.String, appSettings.IdTokenIssuer));

            var token = new JwtSecurityToken(
                appSettings.IdTokenIssuer,
                appSettings.IdTokenAudience,
                claims,
                DateTime.Now,
                DateTime.Now.AddDays(7),
                SigningCredentials.Value);

            var jwtHandler = new JwtSecurityTokenHandler();

            return jwtHandler.WriteToken(token);
        }
    }
}