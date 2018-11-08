using System.Collections.Generic;
using System.Threading.Tasks;
using Kramerica.BankID.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Kramerica.BankID.OIDCServer.Controllers
{
    public class AuthenticationController : Controller
    {
        [HttpGet("~/signin")]
        public async Task<ActionResult> GetSignIn(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View("SignIn");
        }

        [HttpPost("~/signin")]
        public async Task<ActionResult> SignIn(string personalNumber, string returnUrl)
        {
            if (string.IsNullOrEmpty(personalNumber))
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(returnUrl))
            {
                return BadRequest();
            }

            var result = await HttpContext.AuthenticateAsync(BankIDAuthenticationDefaults.AuthenticationScheme);

            if (result.Succeeded)
            {
                await HttpContext.SignInAsync("ServerCookie", result.Principal, new AuthenticationProperties
                {
                    RedirectUri = returnUrl
                });
            }

            return new EmptyResult();

        }

        [HttpGet("~/signout"), HttpPost("~/signout")]
        public ActionResult SignOut()
        {
            return SignOut("ServerCookie");
        }
    }
}