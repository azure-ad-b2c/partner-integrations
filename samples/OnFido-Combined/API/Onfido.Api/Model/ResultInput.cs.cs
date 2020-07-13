using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onfido.Api.Model
{
    public class ResultInput
    {
        public string href { get; set; }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(href);
        }
    }
}
