using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Dynamics365WebApp.Controllers
{
    public class BaseController : Controller
    {
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            ViewBag.BaseUrl = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}";
            base.OnActionExecuted(context);
        }
    }
}