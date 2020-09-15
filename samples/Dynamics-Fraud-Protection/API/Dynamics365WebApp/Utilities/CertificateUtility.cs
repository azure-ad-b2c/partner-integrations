using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace Dynamics365WebApp.Utilities
{
    public static class CertificateUtility
    {
        public static X509Certificate2 GetCertificateBy<T>(X509FindType findType, T findValue)
        {
            var certStore = new X509Store(StoreLocation.CurrentUser);
            certStore.Open(OpenFlags.ReadOnly);

            X509Certificate2Collection certs = certStore.Certificates.Find(findType, findValue, false);
            if (certs.Count > 0)
            {
                return certs[0];
            }
            else
            {
                throw new Exception("Certificate is missing from your current user storage.");
            }
        }

        public static X509Certificate2 GetByThumbprint(string thumbprint)
        {
            return GetCertificateBy(X509FindType.FindByThumbprint, thumbprint);
        }
    }
}
