using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Kramerica.BankID.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Microsoft.AspNetCore.Hosting.Internal.HostingApplication;

namespace Kramerica.BankID.OIDCServer.Controllers
{
    public class IndexController : Controller
    {

        [HttpGet("~/")]
        public async Task<ActionResult> GetIndex()
        {
            return View("Index");
        }

    }
}