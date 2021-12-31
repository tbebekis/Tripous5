using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using System.Net.Http;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

using Tripous;
using WebLib.AspNet;

namespace WebDesk.Controllers
{
    [AllowAnonymous]
    public class DemoController : Controller
    {
        [Route("/demos")]
        public IActionResult Demos()
        {
            return View();
        }

        [Route("/demo/DomElements")]
        public IActionResult DomElements()
        {
            return View();
        }
        [Route("/demo/TabIndexAndFocus")]
        public IActionResult TabIndexAndFocus()
        {
            return View();
        }
        [Route("/demo/EmptyTest")]
        public IActionResult EmptyTest()
        {
            return View();
        }


    }
}
