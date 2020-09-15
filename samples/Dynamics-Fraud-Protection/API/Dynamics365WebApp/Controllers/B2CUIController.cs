using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dynamics365WebApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Dynamics365WebApp.Controllers
{
    public class B2CUIController : BaseController
    {
        private readonly FraudProtectionSettings fraudProtectionSettings;
        public B2CUIController(IOptions<FraudProtectionSettings> fraudProtectionSettings)
        {
            this.fraudProtectionSettings = fraudProtectionSettings.Value;
        }
        public IActionResult GetUIPage([FromRoute]string PageName, [FromRoute] string SessionId)
        {

            ViewBag.DfpInstanceId = fraudProtectionSettings.InstanceId;
            ViewBag.DfpDomain = fraudProtectionSettings.DeviceFingerprintingDomain;
            ViewBag.DfpSessionId = SessionId;
            return View(PageName);
        }
    }
}
