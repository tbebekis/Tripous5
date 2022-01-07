using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using Tripous;
using Tripous.Data;

using WebLib.Models;

namespace WebLib
{

    internal class AjaxViewInfoProviderDefault: AjaxViewInfoProvider
    {
        AjaxViewInfo GetDefaultBrokerViewInfo(AjaxRequest Request, AjaxPacket Packet, string BrokerName)
        {
            AjaxViewInfo Info = new AjaxViewInfo();
  
            ViewDef ViewDef = ViewDef.Find(BrokerName);

            // no view definition, construct a default one.
            if (ViewDef == null)
            {
                SqlBrokerDef BrokerDef = SqlBrokerDef.Find(BrokerName);
                ViewDef = new ViewDef(BrokerDef);
            }

            ViewDef.ViewName = Request.IsSingleInstance ? Request.OperationName : Names.Next(Request.OperationName);
            ViewDef.ClassType = "tp.DeskDataView";
            ViewDef.BrokerClass = "tp.Broker";
            ViewDef.BrokerName = BrokerName;            
 
            ViewModel ViewModel = new ViewModel(ViewDef);

            Info.RazorViewNameOrPath = "View";
            Info.Model = ViewModel;

            return Info;
        }
        AjaxViewInfo GetDefaultSysDataViewInfo(AjaxRequest Request, AjaxPacket Packet, string DataType)
        {
            AjaxViewInfo Info = new AjaxViewInfo();

            string ViewDefName = $"SysData.{DataType}";
            ViewDef ViewDef = ViewDef.Find(ViewDefName);
            if (ViewDef == null)
                Sys.Throw($"ViewDef not found: {ViewDefName}");

            ViewDef.ViewName = Request.IsSingleInstance ? Request.OperationName : Names.Next(Request.OperationName);
            ViewDef.ClassType = "tp.DeskSysDataView";            
            ViewDef["DataType"] = DataType;            

            ViewModel ViewModel = new ViewModel(ViewDef);

            Info.RazorViewNameOrPath = "View";  // until now no SysDataView is needed
            Info.Model = ViewModel;

            Info.ViewData["DataType"] = DataType;
 
            return Info;
        }
 
        public override AjaxViewInfo GetViewInfo(AjaxRequest Request, AjaxPacket Packet)
        {
            AjaxViewInfo Result = null;

            // Example OperationName
            // Ui.Data.Trader
            // Ui.SysData.Table

            if (!string.IsNullOrWhiteSpace(Request.OperationName))
            {
                string[] Parts = Request.OperationName.Split('.', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                if (Parts != null && Parts.Length == 3)
                {
                    if (Sys.IsSameText(Parts[1], "Data"))
                    {
                        Result = GetDefaultBrokerViewInfo(Request, Packet, Parts[2]);
                    }
                    else if (Sys.IsSameText(Parts[1], "SysData"))
                    {
                        Result = GetDefaultSysDataViewInfo(Request, Packet, Parts[2]);
                    } 
                }

            }
 
            return Result;
        }
    }
}
