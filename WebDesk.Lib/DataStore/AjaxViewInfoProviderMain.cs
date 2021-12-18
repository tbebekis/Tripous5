using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tripous;
using Tripous.Data;

using WebLib.Models;

namespace WebLib
{
    internal class AjaxViewInfoProviderMain: AjaxViewInfoProvider
    {
        AjaxViewInfo GetDefaultDataView(string BrokerName)
        {
            AjaxViewInfo Result = new AjaxViewInfo();
  
            ViewDef ViewDef = ViewDef.Find(BrokerName);

            // no view definition, construct a default one.
            if (ViewDef == null)
            {
                SqlBrokerDef BrokerDef = SqlBrokerDef.Find(BrokerName);
                ViewDef = new ViewDef(BrokerDef);
                ViewDef.ToolBarFlags = ViewToolBarFlags.List | ViewToolBarFlags.Filters | ViewToolBarFlags.AllEdits | ViewToolBarFlags.Cancel | ViewToolBarFlags.Close;
            }

            DataViewModel ViewModel = new DataViewModel(ViewDef);
            ViewModel.Setup.BrokerName = BrokerName;


            Result.RazorViewNameOrPath = "DataView";
            Result.Model = ViewModel;

            return Result;
        }


        public override AjaxViewInfo GetViewInfo(AjaxRequest AjaxRequest, AjaxPacket Packet)
        {
            AjaxViewInfo Result = null;
 
            switch (AjaxRequest.OperationName)
            {
                case "Ui.Traders":
                    Result = GetDefaultDataView("Trader");
                    break;

            }

            return Result;
        }
    }
}
