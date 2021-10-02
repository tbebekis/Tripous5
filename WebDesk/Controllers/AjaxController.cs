
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Tripous;
using WebLib;
using WebLib.Models;
using WebLib.AspNet;
 
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Tripous.Data;

namespace WebDesk.Controllers
{
    /// <summary>
    /// Ajax controller.
    /// <para>NOTE: All AJAX calls, except of <see cref="BrokerController"/> calls, should be handled by this controller.</para>
    /// </summary>
    public class AjaxController : ControllerMvc, IAjaxController
    {

        #region IAjaxController
        /// <summary>
        /// Renders a partial view to a string.
        /// <para>See AjaxController.MiniSearch() for an example.</para>
        /// </summary>
        string IAjaxController.ViewToString(string ViewName, object Model, IDictionary<string, object> PlusViewData)
        {
            return this.RenderPartialViewToString(ViewName, Model, PlusViewData);
        }
        /// <summary>
        /// Renders a partial view to a string.
        /// <para>See AjaxController.MiniSearch() for an example.</para>
        /// </summary>
        string IAjaxController.ViewToString(string ViewName, IDictionary<string, object> PlusViewData)
        {
            return this.RenderPartialViewToString(ViewName, PlusViewData);
        }
        #endregion

        [Route("/AjaxExecute", Name = "AjaxExecute")]
        public async Task<JsonResult> AjaxExecute([FromBody] AjaxRequest R)
        {
            await Task.CompletedTask;
            HttpActionResult Result = DataStore.AjaxExecute(this, R);
            return Json(Result);
        }


    }
}
