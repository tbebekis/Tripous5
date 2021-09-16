
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ModelBinding;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Tripous;
using WebDesk.AspNet;

using WebDesk.Models;

namespace WebDesk.Controllers
{
    /// <summary>
    /// Ajax controller.
    /// <para>NOTE: All AJAX calls should be handled by this controller.</para>
    /// </summary>
    public class AjaxController : ControllerMvc
    {
        HttpActionResult GetAjaxHtmlViewResponse(AjaxRequest R)
        {
            string ViewNameOrPath = string.Empty;
            object Model = null;
            IDictionary<string, object> PlusViewData = null;

            switch (R.Name)
            {
                case "AppTable.Ui.List":
                    ViewNameOrPath = "SysData.ItemList";
                    break;
            }

            string HtmlText = string.Empty;
            if (!string.IsNullOrWhiteSpace(ViewNameOrPath))
                HtmlText = this.RenderPartialViewToString(ViewNameOrPath, Model, PlusViewData);

            JObject Packet = new JObject();
            Packet["HtmlText"] = HtmlText;


            HttpActionResult Result = HttpActionResult.SetPacket(Packet, true);
            return Result;
        }

        [Route("/GetHtmlView", Name = "GetHtmlView")]
        public async Task<JsonResult> GetHtmlView([FromBody] AjaxRequest R)
        {
            await Task.CompletedTask;           
            HttpActionResult Result = GetAjaxHtmlViewResponse(R);
            return Json(Result);
        }
    }
}
