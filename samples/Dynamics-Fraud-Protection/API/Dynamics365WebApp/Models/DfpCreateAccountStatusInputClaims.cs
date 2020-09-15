using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dynamics365WebApp.Models
{
    public class DfpCreateAccountStatusInputClaims
    {
        public string SignUpId { get; set; }

        public string StatusType { get; set; }

        public string UserId { get; set; }

        public string ReasonType { get; set; }

        public string ChallengeType { get; set; }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(SignUpId)
                && !string.IsNullOrEmpty(StatusType);
        }
    }
}
