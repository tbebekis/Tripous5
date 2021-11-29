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
    public class DemoDialogsController: Controller
    {

        /* actions */
        [Route("/demo/Notifications")]
        public IActionResult Notifications()
        {
            return View();
        }
        [Route("/demo/NotificationDialogs")]
        public IActionResult NotificationDialogs()
        {
            return View();
        }
       
        [Route("/demo/FrameBox")]
        public IActionResult FrameBox()
        {
            return View();
        }
        [Route("/demo/ContentWindow")]
        public IActionResult ContentWindow()
        {
            return View();
        }


        [Route("/demo/DataSetBox")]
        public IActionResult DataSetBox()
        {
            return View();
        }
        [Route("/demo/TableBox")]
        public IActionResult TableBox()
        {
            return View();
        }
      
        [Route("/demo/RowBox")]
        public IActionResult RowBox()
        {
            return View();
        }
        [Route("/demo/PickRowsBox")]
        public IActionResult PickRowsBox()
        {
            return View();
        }
        [Route("/demo/PickRowBox")]
        public IActionResult PickRowBox()
        {
            return View();
        }


        [Route("/demo/MiscDialogs")]
        public IActionResult MiscDialogs()
        {
            return View();
        }
        
    }
}