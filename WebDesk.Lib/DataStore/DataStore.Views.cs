using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Reflection;

using Tripous;
using Tripous.Data;

 

namespace WebLib
{
    static public partial class DataStore
    {
        static ViewDef CreateSysDataViewDef(string DataType, string TitleKey)
        {
            ViewDef View = ViewDef.Register($"SysData.{DataType}", TitleKey);

            View.ClassType = "tp.SysDataView";
            View.AddCssClasses("tp-View tp-DeskView");

            View.JS.Add("/tp/js/Desk/tp-SysData.js");

            // tool-bar
            View.ToolBarButtons.Clear();
            View.ToolBarButtons.AddRange(new ViewToolBarButtonDef[]
            {
                ViewToolBarButtonDef.ButtonHome,
                ViewToolBarButtonDef.ButtonList,

                ViewToolBarButtonDef.ButtonEdit,
                ViewToolBarButtonDef.ButtonInsert,
                ViewToolBarButtonDef.ButtonDelete,
                ViewToolBarButtonDef.ButtonSave,

                ViewToolBarButtonDef.ButtonCancel,
                ViewToolBarButtonDef.ButtonClose,
            });


            // main TabControl
            View.TabControl = new ViewTabControlDef();
            View.TabControl["SelectedIndex"] = 0;
            View.TabControl.CssClasses.AddRange(new string[] { "MainContainer" });


            // List (browse) page
            ViewTabPageDef ListPage = View.AddTabPage("List", "List");

            ViewRowDef Row = ListPage.AddRow();
            Row.AddCssClasses("ListGridContainer");

            Row.SetGrid();
            Row.Grid["Height"] = "100%";


            // Edit page (contains a tab pager, i.e. its tabs is not empty)
            ViewTabPageDef EditPage = View.AddTabPage("Edit", "Edit");                               
 
            // EditPage controls
            EditPage.TabControl = new ViewTabControlDef();
            EditPage.TabControl["SelectedIndex"] = 0;
            EditPage.TabControl["Height"] = "100%";

            ViewTabPageDef ControlsPanel = EditPage.TabControl.Add("Data", "Data"); 

            // controls    
            List<ViewControlDef> Controls = new List<ViewControlDef>();
            Controls.Add(ViewControlDef.TextBox, "DataType", "DataType", "SysData", new object { });
            Controls.Add(ViewControlDef.TextBox, "DataName", "DataName", "SysData", new object { });
            Controls.Add(ViewControlDef.TextBox, "TitleKey", "TitleKey", "SysData", new object { });
            Controls.Add(ViewControlDef.TextBox, "Owner", "Owner", "SysData", new object { });

            Controls.Add(ViewControlDef.TextBox, "Tag1", "Tag1", "SysData", new object { });
            Controls.Add(ViewControlDef.TextBox, "Tag2", "Tag2", "SysData", new object { });
            Controls.Add(ViewControlDef.TextBox, "Tag3", "Tag3", "SysData", new object { });
            Controls.Add(ViewControlDef.TextBox, "Tag4", "Tag4", "SysData", new object { });

            List<List<ViewControlDef>> ColumnControlLists = Controls.Split(4); 

            ViewRowDef ControlsRow = ControlsPanel.AddRow("");
            foreach (var ControlList in ColumnControlLists)
            {
                ViewColumnDef Column = ControlsRow.AddColumn(ControlsRow.TableName);

                foreach (var Control in ControlList)
                    Column.AddControl(Control);
            }

            return View;

        }
 
        static void RegisterViews()
        {
            CreateSysDataViewDef("Table", "Tables");
            CreateSysDataViewDef("Broker", "Brokers");
        }
    }
}
