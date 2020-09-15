using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dynamics365WebApp.Models
{
    public class DfpCreateAccountOutputClaims
    {
        public string CorrelationId { get; set; }

        public string SignUpId { get; set; }

        public string Decision { get; set; }

        public double BotScore { get; set; }

        public double RiskScore { get; set; }
    }
}
