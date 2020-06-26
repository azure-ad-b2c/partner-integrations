using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Jumio.Api.Model
{
    public class VerificationToken
    {
        private AppSettings appSettings;

        public VerificationToken(AppSettings appSettings)
        {
            this.appSettings = appSettings;
        }
        public string ObjectId { get; set; }

        public string TransactionReference { get; set; }

        public bool IsVerified { get; set; }

        public string Message { get; set; }

        public void Decrypt(string jwt)
        {
            var vp = new TokenValidationParameters
            {
                ValidAudience = appSettings.IdTokenAudience,
                ValidIssuer = appSettings.IdTokenIssuer,
                IssuerSigningKey = appSettings.TokenSigningKey,
                ValidateIssuer = true,
                ValidateLifetime = false,
                TokenDecryptionKey = appSettings.TokenEncryptionKey
            };

            var jwtHandler = new JwtSecurityTokenHandler();
            jwtHandler.ValidateToken(jwt, vp, out SecurityToken jwtToken);
            var jt = jwtToken as JwtSecurityToken;

            if (jt.Claims.Any())
            {
                ObjectId = jt.Claims.FirstOrDefault(x => x.Type == Constants.Claims.ObjectId)?.Value;
                TransactionReference = jt.ValidTo < DateTime.UtcNow ? null : jt.Claims.FirstOrDefault(x => x.Type == Constants.Claims.TransactionReference)?.Value;
                IsVerified = Convert.ToBoolean(jt.Claims.FirstOrDefault(x => x.Type == Constants.Claims.IsVerified)?.Value ?? "false");
                Message = jt.Claims.FirstOrDefault(x => x.Type == Constants.Claims.Message)?.Value;
            }
        }

        public string GenerateToken()
        {
            var signingCredentials = new SigningCredentials(appSettings.TokenSigningKey, SecurityAlgorithms.HmacSha256);
            var ec = new EncryptingCredentials(appSettings.TokenEncryptionKey, JwtConstants.DirectKeyUseAlg, SecurityAlgorithms.Aes256CbcHmacSha512);

            var handler = new JwtSecurityTokenHandler();

            IList<Claim> claims = new List<Claim>();
            claims.Add(new Claim(Constants.Claims.ObjectId, ObjectId, ClaimValueTypes.String, appSettings.IdTokenIssuer));
            claims.Add(new Claim(Constants.Claims.TransactionReference, TransactionReference, ClaimValueTypes.String, appSettings.IdTokenIssuer));
            claims.Add(new Claim(Constants.Claims.IsVerified, IsVerified.ToString(), ClaimValueTypes.String, appSettings.IdTokenIssuer));
            claims.Add(new Claim(Constants.Claims.Message, Message ?? string.Empty, ClaimValueTypes.String, appSettings.IdTokenIssuer));

            var jwt = handler.CreateEncodedJwt(
                appSettings.IdTokenIssuer,
                appSettings.IdTokenAudience,
                new ClaimsIdentity(claims),
                DateTime.Now,
                DateTime.Now.AddDays(1),
                DateTime.Now,
                signingCredentials,
                ec);

            return jwt;
        }
    }
}
