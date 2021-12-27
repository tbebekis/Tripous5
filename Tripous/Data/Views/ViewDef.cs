using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class ViewDef: ViewDefContainerPanel  
    {
        static List<ViewDef> RegistryList = new List<ViewDef>();

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewDef()
        {
        }
        /// <summary>
        /// Constructor. Creates a default view based on a broker descriptor.
        /// </summary>
        public ViewDef(SqlBrokerDef Broker, int MaxFieldsInColumn = 5)
        {
 
            this.Name = Broker.Name;
            this.BrokerName = Broker.Name;

            Title = Broker.Title;

            this.ClassType = "tp.DeskDataView";
            this.BrokerClass = "tp.Broker";
 
            this.AddCssClasses("tp-View tp-DeskView");

            this.AddDefaultDataViewButtons();

            // main TabControl
            this.TabControl = new ViewTabControlDef();
            this.TabControl.CssClasses.AddRange(new string[] { "MainContainer" });

            ViewTabPageDef FilterPage = AddTabPage("Filters", "Filters");                       // filters (search) panel
            ViewTabPageDef ListPage = AddTabPage("List", "List");                               // List (browse) panel
            ViewTabPageDef EditPage = AddTabPage("Edit", "Edit");                               // Edit panel (contains a tab pager, i.e. its tabs is not empty)
 
            // ListPage
            ViewRowDef Row = ListPage.AddRow();
            Row.AddCssClasses("ListGridContainer");

            Row.SetGrid();
            Row.Grid["Height"] = "100%";

            // Edit TabControl pages
            ViewTabPageDef DataPage = EditPage.AddTabPage("Data", "Data");       // the single tab page of the Edit pager    
            DataPage.TableName = Broker.MainTableName;

            // the single row of the Data tab-page
            Row = DataPage.AddRow(DataPage.TableName);
 
            // columns in the row
            var MainTable = Broker.MainTable;

            List<SqlBrokerFieldDef> TableFieldList = new List<SqlBrokerFieldDef>();
            foreach (var FieldDef in MainTable.Fields)
                if (!FieldDef.IsHidden)
                    TableFieldList.Add(FieldDef); 

            List<List<SqlBrokerFieldDef>> ColumnFieldLists = TableFieldList.Split(MaxFieldsInColumn); 

            ViewColumnDef Column;
            ViewControlDef Control;
            foreach (var FieldList in ColumnFieldLists)
            {
                Column = new ViewColumnDef();
                Row.Columns.Add(Column);
                Column.TableName = Row.TableName;

                foreach (var BrokerFieldDef in FieldList)
                {
                    Control = new ViewControlDef(BrokerFieldDef);
                    Column.Controls.Add(Control);
                    Control.TableName = Column.TableName;
                }
            }
        }

        /* static */
        /// <summary>
        /// Returns a registered item, if any, else null.
        /// </summary>
        static public ViewDef Find(string Name)
        {
            return RegistryList.Find(item => Sys.IsSameText(item.Name, Name));
        }
        /// <summary>
        /// Returns true if an item is registered.
        /// </summary>
        static public bool Contains(string Name)
        {
            return Find(Name) != null;
        }

        /// <summary>
        /// Returns the index of an item in the internal registry list.
        /// </summary>
        static public int IndexOf(ViewDef Def)
        {
            return IndexOf(Def.Name);
        }
        /// <summary>
        /// Returns the index of an item in the internal registry list.
        /// </summary>
        static public int IndexOf(string Name)
        {
            for (int i = 0; i < RegistryList.Count; i++)
            {
                if (Sys.IsSameText(RegistryList[i].Name, Name))
                    return i;
            }

            return -1;
        }

        /// <summary>
        /// Registers an item.
        /// <para>NOTE: If an item with the same name is already registered, the specified item replaces the existing item.</para>
        /// </summary>
        static public ViewDef Register(ViewDef Def)
        {
            int Index = IndexOf(Def);
            if (Index != -1)
            {
                RegistryList[Index] = Def;
                return RegistryList[Index];
            }
            else
            {
                RegistryList.Add(Def);
                return Def;
            }
        }
        /// <summary>
        /// Adds a broker to the list
        /// </summary>
        static public ViewDef Register(string Name, string TitleKey)
        {
            ViewDef Def = new ViewDef();
            Def.Name = Name;
            Def.TitleKey = TitleKey;
            return Register(Def);
        }
 
        /* public */ 
        /// <summary>
        /// Adds and returns a <see cref="ViewToolBarButtonDef"/>
        /// </summary>
        public ViewToolBarButtonDef AddButton(string Command, string Text, string IcoClasses, string ToolTip = "")
        {
            ViewToolBarButtonDef Result = new ViewToolBarButtonDef()
            {
                Command = Command,
                Text = Text,
                IcoClasses = IcoClasses,
                ToolTip = !string.IsNullOrWhiteSpace(ToolTip) ? ToolTip : Text,
            };

            ToolBarButtons.Add(Result);
            return Result;
        }
        /// <summary>
        /// Adds the buttons to the tool-bar for a default data-view
        /// </summary>
        public void AddDefaultDataViewButtons()
        {
            ToolBarButtons.Clear();

            ToolBarButtons.AddRange(new ViewToolBarButtonDef[]
            {
                ViewToolBarButtonDef.ButtonHome,

                ViewToolBarButtonDef.ButtonList,
                ViewToolBarButtonDef.ButtonFilters,

                ViewToolBarButtonDef.ButtonEdit,
                ViewToolBarButtonDef.ButtonInsert,
                ViewToolBarButtonDef.ButtonDelete,
                ViewToolBarButtonDef.ButtonSave,

                ViewToolBarButtonDef.ButtonCancel,
                ViewToolBarButtonDef.ButtonClose,
            });
        }

        /// <summary>
        /// Assigns properties to a data-setup object
        /// </summary>
        public override void AssignTo(Dictionary<string, object> DataSetup)
        {
            if (!string.IsNullOrWhiteSpace(ClassType))
                DataSetup["ClassType"] = ClassType;

            if (!string.IsNullOrWhiteSpace(BrokerClass))
                DataSetup["BrokerClass"] = BrokerClass;

            if (!string.IsNullOrWhiteSpace(BrokerName))
                DataSetup["BrokerName"] = BrokerName;

            if (AutocreateControls)
                DataSetup["AutocreateControls"] = AutocreateControls;

            if (JS != null & JS.Count > 0)
                DataSetup["JS"] = JS;

            if (CSS != null & CSS.Count > 0)
                DataSetup["CSS"] = CSS; 

            base.AssignTo(DataSetup);
        }

        /* properties */
        /// <summary>
        /// The broker name. When not set then it returns the Name.
        /// </summary>
        public string BrokerName { get; set; } 
 

        /// <summary>
        /// Tool-bar buttons.
        /// </summary>
        public List<ViewToolBarButtonDef> ToolBarButtons { get; } = new List<ViewToolBarButtonDef>();

        /* data-setup properties */
        /// <summary>
        /// Returns the javascript class name of a view class to be used when creating the view object in javascript.
        /// </summary>
        public string ClassType { get; set; }
        /// <summary>
        /// Returns the javascript class name of a broker class to be used when creating the broker object in javascript.
        /// </summary>
        public string BrokerClass { get; set; }

        /// <summary>
        /// A list of javascript files this view needs in order to function properly.
        /// </summary>
        public List<string> JS { get; set; } = new List<string>();
        /// <summary>
        /// A list of css files this view needs in order to function properly.
        /// </summary>
        public List<string> CSS { get; set; } = new List<string>();

        /// <summary>
        /// When true, controls are auto-created.
        /// <para>Defaults to false.</para>
        /// </summary>
        public bool AutocreateControls { get; set; } = false;
    }
}
