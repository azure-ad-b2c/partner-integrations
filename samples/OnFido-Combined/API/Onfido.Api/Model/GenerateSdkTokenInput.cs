using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onfido.Api.Model
{
    public class GenerateSdkTokenInput
    {
        public string applicant_id { get; set; }

        public string referrer { get; set; }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(applicant_id)
                & !string.IsNullOrEmpty(referrer);
        }
    }
}
