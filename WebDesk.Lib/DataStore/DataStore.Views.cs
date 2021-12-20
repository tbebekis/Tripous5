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

            View.JSClassType = "tp.SysDataView";

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

            // tabs
            ViewTabDef ListTab = View.AddTab("List", "List");  
            ViewTabDef EditTab = View.AddTab("Edit", "Edit");

            // accordeon

            // grid accordeon item
            ViewGroupDef Group = EditTab.AddGroup("Fields");
            ViewRowDef Row = Group.AddRow();
            ViewControlDef Grid = Row.SetGrid();

            // controls accordeon item
            Group = EditTab.AddGroup("Data");
            Row = Group.AddRow();
 

        }

        static void RegisterViews()
        {
            RegisterView_SysData_Table();
        }
    }
}
