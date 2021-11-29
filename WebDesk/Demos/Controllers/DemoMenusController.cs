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
    /// A test controller
    /// </summary>
    [AllowAnonymous]
    public class DemoMenusController : Controller
    {
        /* actions */
        [Route("/demo/Button")]
        public IActionResult Button()
        {
            return View();
        }
        [Route("/demo/ToolBar")]
        public IActionResult ToolBar()
        {
            return View();
        }

        [Route("/demo/Menu")]
        public IActionResult Menu()
        {
            return View();
        }
        [Route("/demo/ContextMenu")]
        public IActionResult ContextMenu()
        {
            return View();
        }

        [Route("/demo/SiteMenu")]
        public IActionResult SiteMenu()
        {
            return View();
        }

    }
}
