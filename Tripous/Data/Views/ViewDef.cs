﻿using System;
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
    public class ViewDef: ViewDefComponent
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
        public ViewDef(SqlBrokerDef Broker, UiSplit Split = null)
        {
            if (Split != null)
                this.ColumnSplit = Split;

            this.Name = Broker.Name;
            this.BrokerName = Broker.Name;

            Title = Broker.Title;

            this.ClassType = "tp.DeskDataView";
            this.BrokerClass = "tp.Broker";

            this.CssClasses.AddRange(new string[] { "tp-View", "tp-DeskView" });

            this.AddDefaultDataViewButtons();

            // PanelList  
            ViewPanelListPanelDef FilterPanel = AddPanelListPanel("Filters", "Filters");       // filters (search) panel
            ViewPanelListPanelDef ListPanel = AddPanelListPanel("List", "List");               // List (browse) panel
            ViewPanelListPanelDef EditPanel = AddPanelListPanel("Edit", "Edit");               // Edit panel (contains a tab pager, i.e. its tabs is not empty)

            // Edit TabControl pages
            ViewTabPageDef DataPage = EditPanel.AddTabPage("Data", "Data");       // the single tab page of the Edit pager    
            DataPage.TableName = Broker.MainTableName;

            // the single row of the Data tab-page
            ViewRowDef Row = DataPage.AddRow(DataPage.TableName);
 
            // columns in the row
            var MainTable = Broker.MainTable;
            List<List<SqlBrokerFieldDef>> ColumnFieldLists = MainTable.Fields.Split(this.ColumnSplit.Large);

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

        /// <summary>
        /// Returns a string with css classes that control the widths of controls/titles in control rows. Examples
        /// <para><c>tp-Ctrls lc-75 mc-70 sc-70</c></para>
        /// <para><c>tp-Ctrls tp-TextTop</c></para>
        /// </summary>
        static public string GetCssClassesForControlWidths(bool TextTop, int TextSplitPercent)
        {
            // tp-Ctrls tp-TextTop
            // tp-Ctrls lc-75 mc-70 sc-70
            StringBuilder SB = new StringBuilder();
            SB.Append("tp-Ctrls ");
            if (TextTop)
            {
                SB.Append("tp-TextTop ");
            }
            else
            {
                // control widths in control rows
                int LargePercent = 100 - TextSplitPercent;
                int MediumPercent = LargePercent - 5;
                int SmallPercent = MediumPercent - 3;

                SB.Append($"lc-{LargePercent} mc-{MediumPercent} sc-{SmallPercent} ");
            }

            return SB.ToString();
        }

        /* public */
        /// <summary>
        /// Adds and returns a <see cref="ViewPanelListPanelDef"/>
        /// <para>NOTE: Creates the container if is null.</para>
        /// </summary>
        public ViewPanelListPanelDef AddPanelListPanel(string TitleKey, string Name = "")
        {
            if (PanelList == null)
                PanelList = new ViewPanelListDef();

            ViewPanelListPanelDef Result = PanelList.Add(TitleKey, Name);
            return Result;
        }
        /// <summary>
        /// Adds and returns a <see cref="ViewTabPageDef"/>
        /// <para>NOTE: Creates the container if is null.</para>
        /// </summary>
        public ViewTabPageDef AddTabPage(string TitleKey, string Name = "")
        {
            if (TabControl == null)
                TabControl = new ViewTabControlDef();

            ViewTabPageDef Result = TabControl.Add(TitleKey, Name);
            return Result;
        }
        /// <summary>
        /// Adds and returns a <see cref="ViewAccordeonPanelDef"/>
        /// <para>NOTE: Creates the container if is null.</para>
        /// </summary>
        public ViewAccordeonPanelDef AddGroup(string TitleKey, string Name = "")
        {
            if (Accordeon == null)
                Accordeon = new ViewAccordeonDef();

            ViewAccordeonPanelDef Result = Accordeon.Add(TitleKey, Name);
            return Result;
        }
        /// <summary>
        /// Adds and returns a <see cref="ViewRowDef"/>
        /// <para>NOTE: Creates the rows list if is null.</para>
        /// </summary>
        public ViewRowDef AddRow(string TableName = "")
        {
            if (Rows == null)
                Rows = new List<ViewRowDef>();

            ViewRowDef Result = new ViewRowDef();
            Result.TableName = TableName;
            Rows.Add(Result);
            return Result;
        }

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

            if (CssClasses != null & CssClasses.Count > 0)
                DataSetup["CssClasses"] = string.Join(" ", CssClasses.ToArray());

            base.AssignTo(DataSetup);
        }

        /* properties */
        /// <summary>
        /// The broker name. When not set then it returns the Name.
        /// </summary>
        public string BrokerName { get; set; }
 

        /// <summary>
        /// When true, then <see cref="TextSplitPercent"/> is not applied. Control labels go on top of each control.
        /// </summary>
        public bool TextTop { get; set; }
        /// <summary>
        /// Width percent of text in rows.
        /// </summary>
        public int TextSplitPercent { get; set; } = 35;
        /// <summary>
        /// Columns per screen size
        /// </summary>
        public UiSplit ColumnSplit { get; set; } = new UiSplit();

        /// <summary>
        /// Tool-bar buttons.
        /// </summary>
        public List<ViewToolBarButtonDef> ToolBarButtons { get; } = new List<ViewToolBarButtonDef>();

        /* properties */
        /// <summary>
        /// A panel list control
        /// </summary>
        public ViewPanelListDef PanelList { get; set; }
        /// <summary>
        /// A tab control
        /// </summary>
        public ViewTabControlDef TabControl { get; set; }
        /// <summary>
        /// An accordeon control
        /// </summary>
        public ViewAccordeonDef Accordeon { get; set; }
        /// <summary>
        /// A list of rows. Could be empty.
        /// <para>A row is a panel. It may contain a grid or columns with controls (control rows).</para>
        /// </summary>
        public List<ViewRowDef> Rows { get; set; }  

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
        /// A list of css classes for the view
        /// </summary>
        public List<string> CssClasses { get; set; } = new List<string>();

        /// <summary>
        /// When true, controls are auto-created.
        /// <para>Defaults to false.</para>
        /// </summary>
        public bool AutocreateControls { get; set; } = false;
    }
}
