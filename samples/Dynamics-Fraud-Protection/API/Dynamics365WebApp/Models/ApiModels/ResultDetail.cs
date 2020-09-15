using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dynamics365WebApp.Models.ApiModels
{
    public class ResultDetail
    {
        public string Decision { get; set; }

        public string ChallengeType { get; set; }

        public string[] Reasons { get; set; }

        public string Rule { get; set; }

        public string[] SupportMessages { get; set; }

        public Dictionary<string, object> Other { get; set; }

        public List<Score> Scores { get; set; }
    }
}
