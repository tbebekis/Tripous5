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
            View.CssClasses.AddRange(new string[] { "tp-View", "tp-DeskView" });

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

            // PanelList   
            ViewPanelListPanelDef ListPanel = View.AddPanelListPanel("List", "List");               // List (browse) panel
            ViewPanelListPanelDef EditPanel = View.AddPanelListPanel("Edit", "Edit");               // Edit panel (contains a tab pager, i.e. its tabs is not empty)

            // Edit TabControl pages
            ViewTabPageDef DataPage = EditPanel.AddTabPage("Data", "Data");                     // the single tab page of the Edit pager    
 


            // the single row of the Data tab-page
           
            List<ViewControlDef> Controls = new List<ViewControlDef>();
            Controls.Add(ViewControlDef.TextBox, "DataType", "DataType", "", new object { });
            Controls.Add(ViewControlDef.TextBox, "DataName", "DataName", "", new object { });
            Controls.Add(ViewControlDef.TextBox, "TitleKey", "TitleKey", "", new object { });
            Controls.Add(ViewControlDef.TextBox, "Owner", "Owner", "", new object { });

            Controls.Add(ViewControlDef.TextBox, "Tag1", "Tag1", "", new object { });
            Controls.Add(ViewControlDef.TextBox, "Tag2", "Tag2", "", new object { });
            Controls.Add(ViewControlDef.TextBox, "Tag3", "Tag3", "", new object { });
            Controls.Add(ViewControlDef.TextBox, "Tag4", "Tag4", "", new object { });

            List<List<ViewControlDef>> ColumnControlLists = Controls.Split(View.ColumnSplit.Large);

            ViewRowDef ControlsRow = DataPage.AddRow();
            foreach (var ControlList in ColumnControlLists)
            {
                ViewColumnDef Column = ControlsRow.AddColumn(ControlsRow.TableName); 

                foreach (var Control in ControlList) 
                    Column.AddControl(Control);
            }


            ViewRowDef GridRow = DataPage.AddRow();
            GridRow.SetGrid();

        }

        static void RegisterViews()
        {
            RegisterView_SysData_Table();
        }
    }
}
