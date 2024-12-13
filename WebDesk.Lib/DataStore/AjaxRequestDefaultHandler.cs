namespace WebLib
{

    /// <summary>
    /// Represents an object that processes an <see cref="AjaxRequest"/> and returns an <see cref="AjaxResponse"/>
    /// </summary>
    internal class AjaxRequestDefaultHandler: IAjaxRequestHandler
    {

        AjaxViewInfo GetDefaultBrokerViewInfo(AjaxRequest Request, string BrokerName)
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

        /*
        How to Create New SysData view
        ------------------------------------------
        - DataStore.GetMainMenu() add the menu item
        - DataStore.RegisterViews() register the view
        - app.MainCommandExecutor add the view to the ValidCommands
        - tp-SysData.js create SysDataHandler class to handle the view
        - tp.DeskSysDataView.InitializeView() create a SysDataHandler instance to handle the view 

         */


        AjaxViewInfo GetDefaultSysDataViewInfo(AjaxRequest Request, string DataType)
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

        AjaxViewInfo GetViewInfo(AjaxRequest Request)
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
                        Result = GetDefaultBrokerViewInfo(Request, Parts[2]);
                    }
                    else if (Sys.IsSameText(Parts[1], "SysData"))
                    {
                        Result = GetDefaultSysDataViewInfo(Request, Parts[2]);
                    }
                }

            }

            return Result;
        }

 

        /// <summary>
        /// Processes an <see cref="AjaxRequest"/> and if it handles the request returns an <see cref="AjaxResponse"/>. Else returns null.
        /// </summary>
        public AjaxResponse Process(AjaxRequest Request, IViewToStringConverter ViewToStringConverter)
        {
            AjaxResponse Result = null;

            // it is a razor view request
            if (Request.Type == RequestType.Ui)
            {
                // find a view info provider that handles this Ui request
                AjaxViewInfo ViewInfo = GetViewInfo(Request);
                if (ViewInfo != null)
                {
                    Result = new AjaxResponse(Request.OperationName);

                    // set the HtmlText if empty
                    string HtmlText = Result["HtmlText"] as string;
                    if (string.IsNullOrWhiteSpace(HtmlText) && !string.IsNullOrWhiteSpace(ViewInfo.RazorViewNameOrPath))
                    {
                        HtmlText = ViewToStringConverter.ViewToString(ViewInfo.RazorViewNameOrPath, ViewInfo.Model, ViewInfo.ViewData);
                        Result["HtmlText"] = HtmlText;
                    }
                }

            }

            return Result;
        }
    }
}
