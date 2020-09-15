using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dynamics365WebApp.Models
{
    public class DfpCreateAccountInputClaims
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string DisplayName { get; set; }

        public string Language { get; set; }

        public string Email { get; set; }

        public bool IsEmailUsername { get; set; }

        public bool IsEmailValidated { get; set; }

        public string IpAddress { get; set; }

        public string DeviceContextId { get; set; }

        public string AuthenticationProvider { get; set; }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(DeviceContextId)
                && !string.IsNullOrEmpty(Email);
        }
    }
}
