using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

namespace Dynamics365WebApp.Models
{
    public class B2CResponseContent
    {
        public string version { get; set; }
        public int status { get; set; }
        public string userMessage { get; set; }

        public string developerMessage { get; set; }

        public B2CResponseContent(string message, HttpStatusCode status)
        {
            this.userMessage = message;
            this.status = (int)status;
            this.version = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        }
    }

    public class B2CErrorResponseContent : B2CResponseContent
    {
        public B2CErrorResponseContent(string message, string developerMessage = null) : base(message, HttpStatusCode.Conflict)
        {
            this.developerMessage = developerMessage;
        }
    }
}
