using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.IO;
using System.Text;
using System.Globalization;
using System.Data;
using System.Data.Common;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Options;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using Tripous;
using Tripous.Logging;
using Tripous.Data;


using WebLib.AspNet;
using WebLib.Models;

namespace WebLib
{
 
    static public partial class DataStore
    {
        /// <summary>
        /// Prepares and returns an <see cref="HttpActionResult"/> for a Ui request
        /// </summary>
        static HttpActionResult GetHtmlView(IAjaxController Controller, AjaxRequest Request)
        {
            AjaxPacket Packet = new AjaxPacket(Request.OperationName);
 
            // find a view info provider that handles this Ui request
            AjaxViewInfo ViewInfo = null;
            foreach (var ViewInfoProvider in AjaxViewInfoProviders)
            {
                ViewInfo = ViewInfoProvider.GetViewInfo(Request, Packet);
                if (ViewInfo != null)
                    break;
            }

            if (ViewInfo == null)
                Sys.Throw($"No View Info for requested view. Operation: {Request.OperationName}");
 
            // set the HtmlText if empty
            string HtmlText = Packet["HtmlText"] as string;
            if (string.IsNullOrWhiteSpace(HtmlText) && !string.IsNullOrWhiteSpace(ViewInfo.RazorViewNameOrPath))
            {
                HtmlText = Controller.ViewToString(ViewInfo.RazorViewNameOrPath, ViewInfo.Model, ViewInfo.ViewData);
                Packet["HtmlText"] = HtmlText;
            }

            // return the result
            HttpActionResult Result = HttpActionResult.SetPacket(Packet.GetPacketObject(), true);
            return Result;
        }

        /// <summary>
        /// Executes a request coming from an ajax call to ajax controller and returns the result.
        /// </summary>
        static public HttpActionResult AjaxExecute(IAjaxController Controller, AjaxRequest R)
        {
            HttpActionResult Result = null;

            object CmdResult = null;

            if (R.IsCommandRequest)
            {
                string S = R.GetParam("CommandName") as string;
                if (!string.IsNullOrWhiteSpace(S))
                    CmdResult = Command.ExecuteByName(S);

                if (CmdResult == null)
                {
                    S = R.GetParam("CommandId") as string;
                    if (!string.IsNullOrWhiteSpace(S))
                        CmdResult = Command.ExecuteById(S);
                } 
            }

            if (CmdResult != null)
            {
                Result = HttpActionResult.SetPacket(CmdResult, true);
            }
            else if (R.IsUiRequest)
            {
                Result = GetHtmlView(Controller, R);
            }

            if (Result == null)
                Sys.Throw($"Ajax Operation not supported: {R.OperationName}");

            return Result;
        }

    }
}
