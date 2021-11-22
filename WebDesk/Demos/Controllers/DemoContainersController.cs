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
    public class DemoContainersController: Controller
    {
        /* actions */
        [Route("/demo/DropDownBox")]
        public IActionResult DropDownBox()
        {
            return View();
        }
        [Route("/demo/VirtualScroller")]
        public IActionResult VirtualScroller()
        {
            return View();
        }
        [Route("/demo/Splitter")]
        public IActionResult Splitter()
        {
            return View();
        }
        [Route("/demo/GroupBox")]
        public IActionResult GroupBox()
        {
            return View();
        }
        [Route("/demo/Accordion")]
        public IActionResult Accordion()
        {
            return View();
        }
        [Route("/demo/TabControl")]
        public IActionResult TabControl()
        {
            return View();
        }
        [Route("/demo/PanelList")]
        public IActionResult PanelList()
        {
            return View();
        }
        [Route("/demo/ImageSlider")]
        public IActionResult ImageSlider()
        {
            return View();
        }
        [Route("/demo/IFrame")]
        public IActionResult IFrame()
        {
            return View();
        }

    }
}
