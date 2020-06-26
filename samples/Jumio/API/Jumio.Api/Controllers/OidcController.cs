using Jumio.Api.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Security.Cryptography.X509Certificates;

namespace Jumio.Api.Controllers
{
    [Produces("application/json")]
    [Route("api/Oidc")]
    public class OidcController : Controller
    {
        private static Lazy<X509SigningCredentials> SigningCredentials;
        private readonly AppSettings AppSettings;

        public OidcController(IOptions<AppSettings> options)
        {
            AppSettings = options.Value;
            SigningCredentials = new Lazy<X509SigningCredentials>(() =>
            {
                var certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
                certStore.Open(OpenFlags.ReadOnly);
                var certCollection = certStore.Certificates.Find(
                    X509FindType.FindByThumbprint,
                    AppSettings.SigningCertThumbprint,
                    false);

                if (certCollection.Count > 0)
                {
                    return new X509SigningCredentials(certCollection[0]);
                }

                throw new Exception("Certificate not found");
            });
        }

        [Route(".well-known/openid-configuration", Name = "OIDCMetadata")]
        public ActionResult Metadata()
        {
            return Content(JsonConvert.SerializeObject(new OidcModel
            {
                Issuer = AppSettings.IdTokenIssuer,

                JwksUri = Url.Link("JWKS", null),

                IdTokenSigningAlgValuesSupported = new[] { SigningCredentials.Value.Algorithm }
            }), "application/json");
        }

        [Route(".well-known/keys", Name = "JWKS")]
        public ActionResult JwksDocument()
        {
            return Content(JsonConvert.SerializeObject(new JwksModel { Keys = new[] { JwksKeyModel.FromSigningCredentials(SigningCredentials.Value) } }),
                "application/json");
        }
    }
}