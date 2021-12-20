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
    public class DemoControlsController : Controller
    {

        /* actions */

        [Route("/demo/AutocompleteList")]
        public IActionResult AutocompleteList()
        {
            return View();
        }
        /// <summary>
        /// For testing the tp.AutocompleteList javascript class
        /// </summary>
        [Route("/demo/GetAutocompleteList")]
        public IActionResult GetAutocompleteList(string Text, bool UseStartsWith)
        {
            int Id = 1;
            string[] A = { "html5", "javascript", "typescript", "script", "scriptable", "html", "css rules", "responsive", "restfull" };

            object List = A.Where(S => UseStartsWith ? S.StartsWith(Text, StringComparison.OrdinalIgnoreCase) : S.IndexOf(Text, StringComparison.OrdinalIgnoreCase) >= 0).
                Select(S => new { Id = Id++, Name = S }).ToArray();

            // should look like a Tripous.HttpActionResult
            object Data = new
            {
                ErrorText = "",
                IsSuccess = true,
                Packet = List,
            };

            IActionResult Result = Json(Data);
            return Result;

        }

        

        [Route("/demo/Label")]
        public IActionResult Label()
        {
            return View();
        }
        [Route("/demo/TextBox")]
        public IActionResult TextBox()
        {
            return View();
        }
        [Route("/demo/Memo")]
        public IActionResult Memo()
        {
            return View();
        }
        [Route("/demo/CheckBox")]
        public IActionResult CheckBox()
        {
            return View();
        }
        [Route("/demo/NumberBox")]
        public IActionResult NumberBox()
        {
            return View();
        }

        [Route("/demo/ComboBox")]
        public IActionResult ComboBox()
        {
            return View();
        }
        [Route("/demo/ListBox")]
        public IActionResult ListBox()
        {
            return View();
        }
        [Route("/demo/CheckListBox")]
        public IActionResult CheckListBox()
        {
            return View();
        }
        [Route("/demo/CheckComboBox")]
        public IActionResult CheckComboBox()
        {
            return View();
        }

        [Route("/demo/HtmlComboBox")]
        public IActionResult HtmlComboBox()
        {
            return View();
        }
        [Route("/demo/HtmlListBox")]
        public IActionResult HtmlListBox()
        {
            return View();
        }
        [Route("/demo/HtmlNumberBox")]
        public IActionResult HtmlNumberBox()
        {
            return View();
        }
        [Route("/demo/HtmlNumberBoxEx")]
        public IActionResult HtmlNumberBoxEx()
        {
            return View();
        }

        [Route("/demo/Calendar")]
        public IActionResult Calendar()
        {
            return View();
        }
        [Route("/demo/DateBox")]
        public IActionResult DateBox()
        {
            return View();
        }
        [Route("/demo/HtmlDateBox")]
        public IActionResult HtmlDateBox()
        {
            return View();
        }

        [Route("/demo/ImageBox")]
        public IActionResult ImageBox()
        {
            return View();
        }
        [Route("/demo/RadioGroup")]
        public IActionResult RadioGroup()
        {
            return View();
        }
        [Route("/demo/ValueSlider")]
        public IActionResult ValueSlider()
        {
            return View();
        }

        [Route("/demo/ProgressBar")]
        public IActionResult ProgressBar()
        {
            return View();
        }
        [Route("/demo/TreeView")]
        public IActionResult TreeView()
        {
            return View();
        }


    }
}
