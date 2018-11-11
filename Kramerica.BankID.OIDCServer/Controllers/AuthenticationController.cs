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
    public class AuthenticationController : Controller
    {
        private BankIDClient _bankIDClient;

        public AuthenticationController(BankIDClient bankIDClient)
        {
            _bankIDClient = bankIDClient;
        }

        [HttpGet("~/signin")]
        public async Task<ActionResult> GetSignIn(string returnUrl = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View("SignIn");
        }

        //Classic BankID with personalNumber entered by the user.
        //Both auth and collect are done in the AuthenticateHandler
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

            //We will set the pesonalNumber property so that the AuthenticationHandler can get it from there.
            //It could get it from the FORM directly but this offers some abstraction.
            Request.HttpContext.Items[BankIDAuthenticationDefaults.PersonalNumberPropertyName] = personalNumber;

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

        //The auth call is made in the controller action to get the AutoStartToken needed for the QR-code.
        [HttpGet("~/signinqr")]
        public async Task<ActionResult> GetSignInQR(string returnUrl = null)
        {
            if (string.IsNullOrEmpty(returnUrl))
            {
                return BadRequest();
            }

            var authResponse = await _bankIDClient.Auth(null, this.Request.HttpContext.Connection.RemoteIpAddress.ToString());

            if (authResponse == null)
            {
                return BadRequest();
            }

            ViewBag.ReturnUrl = returnUrl;
            ViewBag.AutoStartToken = authResponse.autoStartToken;

            //Store the orderRef in TempData
            TempData[authResponse.autoStartToken] = authResponse.orderRef;

            return View("SignInQR");
        }

        [HttpPost("~/signinqr")]
        public async Task<ActionResult> SignInQR(string autoStartToken, string returnUrl)
        {
            if (string.IsNullOrEmpty(autoStartToken))
            {
                return BadRequest();
            }

            if (string.IsNullOrEmpty(returnUrl))
            {
                return BadRequest();
            }

            Request.HttpContext.Items[BankIDAuthenticationDefaults.OrderRefPropertyName] = TempData[autoStartToken];

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