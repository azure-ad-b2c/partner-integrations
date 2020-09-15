using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dynamics365WebApp.Models.ApiModels
{
    public class DfpAccountActionResponse
    {
        public List<ResultDetail> ResultDetails { get; set; }
        public string TransactionReferenceId { get; set; }
        public string Description { get; set; }
        public string Code { get; set; }
    }
}
