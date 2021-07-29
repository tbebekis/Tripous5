
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using WebDesk.Models;

namespace WebDesk.Controllers
{
    /// <summary>
    /// Home
    /// </summary>
    public class HomeController : ControllerMvc
    {

        [HttpGet("/", Name = "Home")]
        [HttpGet("/desk", Name = "Desk")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("/login", Name = "Login"), AllowAnonymous]
        public IActionResult Login()
        {
            if (UserContext.IsAuthenticated)
                return RedirectToRoute("Home");

            CredentialsModel M = new CredentialsModel();

            return View("Login", M);
        }
        [HttpPost("/login", Name = "Login"), AllowAnonymous]
        public async Task<IActionResult> Login(CredentialsModel M, string ReturnUrl = "")
        {
            if (Lib.IsCookieAuthenticated)
                return RedirectToRoute("Home");
 
            if (ValidateModel(M))
            { 
                ItemResponse<Requestor> Response = DataStore.ValidateRequestor(M.UserId, M.Password);
                Requestor User = Response.Item;

                if (Response.Succeeded && User != null)
                {
                    var Settings = DataStore.GetSettings();
                    bool IsImpersonation = !string.IsNullOrWhiteSpace(Settings.General.SuperUserPassword) && Settings.General.SuperUserPassword == M.Password;

                    await UserContext.SignInAsync(User, true, IsImpersonation);

                    if (!string.IsNullOrWhiteSpace(ReturnUrl))
                        return HandleReturnUrl(ReturnUrl);

                    return RedirectToRoute("Home");
                }
                else if (!string.IsNullOrWhiteSpace(Response.Error))
                {
                    Lib.AddToErrorList(Lib.GS(Response.Error));
                }
                else
                {
                    Lib.AddToErrorList(Lib.GS("LoginFailed"));
                }
            }

            return View("Login", M); // something went wrong 
        }


    }
}
