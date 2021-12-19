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
        AjaxViewInfo GetDefaultDataViewInfo(AjaxRequest Request, AjaxPacket Packet, string BrokerName)
        {
            AjaxViewInfo Info = new AjaxViewInfo();
  
            ViewDef ViewDef = ViewDef.Find(BrokerName);

            // no view definition, construct a default one.
            if (ViewDef == null)
            {
                SqlBrokerDef BrokerDef = SqlBrokerDef.Find(BrokerName);
                ViewDef = new ViewDef(BrokerDef);
            }

            DataViewModel ViewModel = new DataViewModel(ViewDef);
            ViewModel.DataSetup.ClassType  = "tp.DeskDataView"; 
            ViewModel.DataSetup.BrokerClass = "tp.Broker";
            ViewModel.DataSetup.BrokerName = BrokerName;

            Info.RazorViewNameOrPath = "DataView";
            Info.Model = ViewModel;

            Packet["ViewName"] = Request.OperationName;
            Packet["ViewTitle"] = ViewDef.Title;

            return Info;
        }
        AjaxViewInfo GetDefaultSysDataViewInfo(AjaxRequest Request, AjaxPacket Packet, string DataType)
        {
            AjaxViewInfo Info = new AjaxViewInfo();

            string ViewDefName = $"SysData.{DataType}";
            ViewDef ViewDef = ViewDef.Find(ViewDefName);
            if (ViewDef == null)
                Sys.Throw($"ViewDef not found: {ViewDefName}");

            DataViewModel ViewModel = new DataViewModel(ViewDef);
            ViewModel.DataSetup.ClassType = "tp.DeskSysDataView";
            ViewModel.DataSetup["DataType"] = DataType;

            Info.RazorViewNameOrPath = "DataView";  // until now no SysDataView is needed
            Info.Model = ViewModel;

            Info.ViewData["DataType"] = DataType;

            Packet["ViewName"] = Request.OperationName;
            Packet["ViewTitle"] = ViewDef.Title;
            Packet["DataType"] = DataType;
            DataTable Table = SysData.Select(DataType, NoBlobs: true);
            Packet["ListTable"] = JsonDataTable.ToJObject(Table);
            
            // EDW
            // Να γίνει ViewDef registration για SysData.Table
            // με ToolBar
            // και Edit μέρος
 
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
                        Result = GetDefaultDataViewInfo(Request, Packet, Parts[2]);
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
