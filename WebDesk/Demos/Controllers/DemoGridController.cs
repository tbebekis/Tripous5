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
    public class DemoGridController : Controller
    {

        /* actions */
        [Route("/demo/Grid")]
        public IActionResult Grid()
        {
            return View();
        }
        [Route("/demo/Grid2")]
        public IActionResult Grid2()
        {
            return View();
        }
        [Route("/demo/Grid3")]
        public IActionResult Grid3()
        {
            return View();
        }
    }
}
