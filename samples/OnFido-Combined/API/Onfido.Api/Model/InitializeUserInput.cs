using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onfido.Api.Model
{
    public class InitializeUserInput
    {
        public string first_name { get; set; }
        public string last_name { get; set; }

        public bool Validate()
        {
            return !string.IsNullOrEmpty(first_name)
                & !string.IsNullOrEmpty(last_name);
        }
    }
}
