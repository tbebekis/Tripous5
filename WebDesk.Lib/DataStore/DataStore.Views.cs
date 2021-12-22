using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using Tripous;
using Tripous.Data;

 

namespace WebLib
{
    static public partial class DataStore
    {
        static void RegisterView_SysData_Table()
        {
            ViewDef View = ViewDef.Register("SysData.Table", "Tables");

            View.ClassType = "tp.SysDataView";
            View.AddCssClasses("tp-View tp-DeskView");   

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
            View.TabControl.CssClasses.AddRange(new string[] { "MainContainer" });
 
            ViewTabPageDef ListPage = View.AddTabPage("List", "List");                               // List (browse) panel
            ViewTabPageDef EditPage = View.AddTabPage("Edit", "Edit");                               // Edit panel (contains a tab pager, i.e. its tabs is not empty)

            ViewRowDef Row = ListPage.AddRow();
            Row.AddCssClasses("ListGridContainer");

            Row.SetGrid();
            Row.Grid["Height"] = "100%";

            // EditPage controls
            EditPage.Accordion = new ViewAccordionDef();
            EditPage.Accordion["AllowMultiExpand"] = true;
            ViewAccordionPanelDef ControlsPanel = EditPage.Accordion.Add("Data", "Data");
            ViewAccordionPanelDef FieldsPanel = EditPage.Accordion.Add("Fields", "Fields");
 
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

            List<List<ViewControlDef>> ColumnControlLists = Controls.Split(View.ColumnSplit.Large);

            ViewRowDef ControlsRow = ControlsPanel.AddRow("");
            foreach (var ControlList in ColumnControlLists)
            {
                ViewColumnDef Column = ControlsRow.AddColumn(ControlsRow.TableName); 

                foreach (var Control in ControlList) 
                    Column.AddControl(Control);
            }


            ViewRowDef GridRow = FieldsPanel.AddRow("");
            GridRow.SetGrid();

        }

        static void RegisterViews()
        {
            RegisterView_SysData_Table();
        }
    }
}
