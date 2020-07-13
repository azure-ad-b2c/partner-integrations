using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Onfido.Api.Model
{
    public class InitializeUserOutput
    {
        public string id { get; set; }
        public DateTime created_at { get; set; }
        public bool sandbox { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string href { get; set; }
    }
}
