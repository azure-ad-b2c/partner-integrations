using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onfido.Api.Model
{
    public class CreateCheckInput
    {
        public string applicant_id { get; set; }
        public string[] report_names { get; set; }
        public CreateCheckInput()
        {
            report_names ??= new string[] { "document","facial_similarity_photo" };
        }
        public bool Validate()
        {
            return !string.IsNullOrEmpty(applicant_id);
        }
    }
            
}
