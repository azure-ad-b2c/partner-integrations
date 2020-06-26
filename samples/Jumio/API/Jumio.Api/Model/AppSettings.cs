using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Jumio.Api.Model
{
    public class AppSettings
    {
        public string SigningCertThumbprint { get; set; }

        public string IdTokenSigningKey { get; set; }

        public string IdTokenEncryptionKey { get; set; }

        public string IdTokenIssuer { get; set; }

        public string IdTokenAudience { get; set; }

        public string BaseRedirectUrl { get; set; }

        public SecurityKey TokenSigningKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IdTokenSigningKey));
        public SecurityKey TokenEncryptionKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IdTokenEncryptionKey));

    }
}
