
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
using WebDesk.AspNet;

using WebDesk.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Tripous.Data;

namespace WebDesk.Controllers
{
    /// <summary>
    /// Ajax controller.
    /// <para>NOTE: All AJAX calls should be handled by this controller.</para>
    /// </summary>
    public class AjaxController : ControllerMvc
    {
 
        HttpActionResult GetHtmlView(AjaxRequest R)
        {
            string ViewNameOrPath = string.Empty;
            object Model = null;
            Dictionary<string, object> PlusViewData = new Dictionary<string, object>();

            string ViewName = R.Params["ViewName"].ToString();

            JObject Packet = new JObject();
            Packet["OperationName"] = R.OperationName;
            Packet["ViewName"] = ViewName;

            DataTable Table;
            switch (ViewName)
            {
                case "Ui.SysData.List.Table":
                    ViewNameOrPath = "SysData.List";
                    PlusViewData["DataType"] = "Table";
                    Packet["DataType"] = "Table";
                    Table = SysData.Select("Table", NoBlobs: true);
                    Packet["Table"] = JsonDataTable.ToJObject(Table);
                    break;
                case "Ui.SysData.Insert.Table":
                    ViewNameOrPath = "SysData.Insert.Table";
                    PlusViewData["DataType"] = "Table";
                    Packet["DataType"] = "Table";
                    Packet["IsInsert"] = true;
                    break;
            }

            string HtmlText = string.Empty;
            if (!string.IsNullOrWhiteSpace(ViewNameOrPath))
                HtmlText = this.RenderPartialViewToString(ViewNameOrPath, Model, PlusViewData);
            
            Packet["HtmlText"] = HtmlText;

            HttpActionResult Result = HttpActionResult.SetPacket(Packet, true);
            return Result;
        }

        [Route("/AjaxExecute", Name = "AjaxExecute")]
        public async Task<JsonResult> AjaxExecute([FromBody] AjaxRequest R)
        {
            await Task.CompletedTask;
            HttpActionResult Result = null;

            switch (R.OperationName)
            {
                case "GetHtmlView": 
                    Result = GetHtmlView(R);
                    break;
                default:
                    Sys.Throw($"Ajax Operation not supported: {R.OperationName}");
                    break;
            }
 
            return Json(Result);
        }

 
    }
}
