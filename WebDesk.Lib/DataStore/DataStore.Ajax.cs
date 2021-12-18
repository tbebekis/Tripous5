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


    /*

     //Packet["ViewName"] = R.IsSingleInstance ? R.OperationName : Names.Next(R.OperationName);

    Dictionary<string, object> ViewData = new Dictionary<string, object>();     // ViewData for the razor view

    string RazorViewNameOrPath = string.Empty;
    object Model = null;
    DataTable Table; 

    void PrepareDefaultDataView(string BrokerName)
    {
        RazorViewNameOrPath = "DataView";

        ViewDef ViewDef = ViewDef.Find(BrokerName);
        // no view definition, construct a default one.
        if (ViewDef == null)
        {
            SqlBrokerDef BrokerDef = SqlBrokerDef.Find(BrokerName);
            ViewDef = new ViewDef(BrokerDef);
            ViewDef.ToolBarFlags = ViewToolBarFlags.List | ViewToolBarFlags.Filters | ViewToolBarFlags.AllEdits | ViewToolBarFlags.Cancel | ViewToolBarFlags.Close;
        }

        DataViewModel DVM = new DataViewModel(ViewDef);
        DVM.Setup.BrokerName = BrokerName;  


        Model = DVM;
    }

    switch (R.OperationName)
    {
        case "Ui.SysData.Table":
            RazorViewNameOrPath = "SysData.List";
            ViewData["DataType"] = "Table";
            Packet["DataType"] = "Table";
            Table = SysData.Select("Table", NoBlobs: true);
            Packet["Table"] = JsonDataTable.ToJObject(Table);
            break;

        case "Ui.SysData.Insert.Table":
            RazorViewNameOrPath = "SysData.Insert.Table";
            ViewData["DataType"] = "Table";
            Packet["DataType"] = "Table";
            Packet["IsInsert"] = true;
            break;

        case "Ui.Traders":
            PrepareDefaultDataView("Trader");
            break;
    }

    if (!string.IsNullOrWhiteSpace(RazorViewNameOrPath))
    {
        string HtmlText = Controller.ViewToString(RazorViewNameOrPath, Model, ViewData);
        Packet["HtmlText"] = HtmlText;
    }
    */



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

            // set the ViewName if empty
            string ViewName = Packet["ViewName"] as string;
            if (string.IsNullOrWhiteSpace(ViewName))
            {
                ViewName = Request.IsSingleInstance ? Request.OperationName : Names.Next(Request.OperationName);
                Packet["ViewName"] = ViewName;
            }

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

            if (R.IsUiRequest)
            {
                Result = Result = GetHtmlView(Controller, R);
            }

            if (Result == null)
                Sys.Throw($"Ajax Operation not supported: {R.OperationName}");

            return Result;
        }

    }
}
