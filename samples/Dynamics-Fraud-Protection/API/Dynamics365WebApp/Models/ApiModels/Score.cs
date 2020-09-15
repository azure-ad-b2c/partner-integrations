using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dynamics365WebApp.Models.ApiModels
{
    public class Score
    {
        public string ScoreType { get; set; }

        public double ScoreValue { get; set; }

        public string Reason { get; set; }
    }
}
