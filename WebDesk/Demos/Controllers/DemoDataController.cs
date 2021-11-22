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

    /// <summary>
    /// Test controller
    /// </summary>
    [AllowAnonymous]
    public class DemoDataController : Controller
    {
        /* actions */
        [Route("/demo/SimpleBinding")]
        public IActionResult SimpleBinding()
        {
            return View();
        }
        [Route("/demo/ListBinding")]
        public IActionResult ListBinding()
        {
            return View();
        }
        [Route("/demo/MasterDetail")]
        public IActionResult MasterDetail()
        {
            return View();
        }
        [Route("/demo/DataView")]
        public IActionResult DataView()
        {
            return View();
        }

        [Route("/demo/Locator")]
        public IActionResult Locator()
        {
            return View();
        }
    }
}
