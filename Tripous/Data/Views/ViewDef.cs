using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data
{


 
    /// <summary>
    /// Top level container. Represents a desktop form or a html page.
    /// <para>May contain: Tabs, Groups and Rows.</para>
    /// <para><see cref="Tabs"/>, <see cref="Groups"/> and <see cref="Rows"/> are checked in that order. If any is not empty the rest are ignored.</para>
    /// <para>Contains a single Pager (TabControl) when the <see cref="Tabs"/> are not empty. </para>
    /// <para>Contains a single Accordeon when the <see cref="Groups"/> is not empty. </para>
    /// <para>Contains a signle Panel (DIV) with one or more rows when the <see cref="Rows"/> is not empty. </para>
    /// </summary>
    public class ViewDef 
    {
        static List<ViewDef> RegistryList = new List<ViewDef>();

        string fBrokerName;

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

            this.JSClassType = "tp.DeskDataView";
            this.JSBrokerClass = "tp.Broker";

            this.AddDefaultDataViewButtons();

            // filters (search) tab
            ViewTabDef FilterTab = new ViewTabDef();
            Tabs.Add(FilterTab);
            FilterTab.TabId = "Filters";
            FilterTab.TitleKey = "Filters";

            // List (browse) tab
            ViewTabDef ListTab = new ViewTabDef();
            Tabs.Add(ListTab);
            ListTab.TabId = "List";
            ListTab.TitleKey = "List";

            // Edit tab (contains a tab pager, i.e. its tabs is not empty)
            ViewTabDef EditTab = new ViewTabDef();
            Tabs.Add(EditTab);
            EditTab.TabId = "Edit";
            EditTab.TitleKey = "Edit";

            // the single tab page of the Edit pager
            ViewTabDef DataTab = new ViewTabDef(); 
            EditTab.Tabs.Add(DataTab);
            DataTab.TabId = "Data";
            DataTab.TitleKey = "Data";
            DataTab.TableName = Broker.MainTableName;
 
            // the single row of the Data tab-page
            ViewRowDef Row = new ViewRowDef();  
            DataTab.Rows.Add(Row);
            Row.TableName = DataTab.TableName;

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
        /// Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return Title;
        }
    
        /// <summary>
        /// Assigns properties to a data-setup object
        /// </summary>
        public void AssignTo(Dictionary<string, object> DataSetup)
        {
            if (!string.IsNullOrWhiteSpace(JSClassType))
                DataSetup["ClassType"] = JSClassType;

            if (!string.IsNullOrWhiteSpace(JSBrokerClass))
                DataSetup["BrokerClass"] = JSBrokerClass;

            if (!string.IsNullOrWhiteSpace(BrokerName))
                DataSetup["BrokerName"] = BrokerName;

            if (AutocreateControls)
                DataSetup["AutocreateControls"] = AutocreateControls;
 
            if (JS != null & JS.Count > 0)
                DataSetup["JS"] = JS;

            if (CSS != null & CSS.Count > 0)
                DataSetup["CSS"] = JS;

            if (CssClasses != null & CssClasses.Count > 0)
                DataSetup["CssClasses"] = JS;
        }

        /// <summary>
        /// Returns a <see cref="ViewTabDef"/> found under a specified Id, if any, else null.
        /// </summary>
        public ViewTabDef FindTabById(string TabId)
        {
           return Tabs.Find(item => Sys.IsSameText(item.TabId, TabId));
        }
        /// <summary>
        /// Returns true if a <see cref="ViewTabDef"/> found under a specified Id.
        /// </summary>
        public bool ContainsTab(string TabId)
        {
            return FindTabById(TabId) != null;
        }
 
        /// <summary>
        /// Adds the buttons to the tool-bar for a default data-view
        /// </summary>
        public void AddDefaultDataViewButtons()
        {
            ToolBarButtons.Clear();

            ToolBarButtons.AddRange(new ViewToolBarButtonDef []
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
        /// Adds and returns a <see cref="ViewTabDef"/>
        /// </summary>
        public ViewTabDef AddTab(string TitleKey, string TabId = "")
        {
            ViewTabDef Result = new ViewTabDef()
            {
                TitleKey = TitleKey,
                TabId = TabId,
            };

            Tabs.Add(Result);

            return Result;
        }
        /// <summary>
        /// Adds and returns a <see cref="ViewGroupDef"/>
        /// </summary>
        public ViewGroupDef AddGroup(string TitleKey)
        {
            ViewGroupDef Result = new ViewGroupDef()
            {
                TitleKey = TitleKey,    
            };

            Groups.Add(Result);

            return Result;
        }
        /// <summary>
        /// Adds and returns a <see cref="ViewRowDef"/>
        /// </summary>
        public ViewRowDef AddRow(string TitleKey)
        {
            ViewRowDef Result = new ViewRowDef();
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
                ToolTip = !string.IsNullOrWhiteSpace(ToolTip)? ToolTip: Text,
            };

            ToolBarButtons.Add(Result);
            return Result;
        }

        /* properties */
        /// <summary>
        /// A unique name among all view containers. 
        /// <para>NOTE: For DataViews this is the BrokerName.</para>
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The broker name. When not set then it returns the <see cref="Name"/>.
        /// </summary>
        public string BrokerName
        {
            get { return !string.IsNullOrWhiteSpace(fBrokerName) ? fBrokerName : Name; }
            set { fBrokerName = value; }
        }
        /// <summary>
        /// The table name. Used only when there are no Tabs or Groups in this instance.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Gets or sets a resource Key used in returning a localized version of Title
        /// </summary>
        public string TitleKey { get; set; }
        /// <summary>
        /// Gets the Title of this instance, used for display purposes. 
        /// <para>NOTE: The setter is fake. Do NOT use it.</para>
        /// </summary>    
        public string Title
        {
            get { return !string.IsNullOrWhiteSpace(TitleKey) ? Res.GS(TitleKey, TitleKey) : Name; }
            set { }
        }
 
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

        /// <summary>
        /// A list of tabs. Could be empty.
        /// <para>A tab is a single tab page or a TabControl (Pager) with child tab pages.</para>
        /// </summary>
        public List<ViewTabDef> Tabs { get; } = new List<ViewTabDef>();
        /// <summary>
        /// A list of groups. Could be empty.
        /// <para>A group is a container such as a DIV or panel which groups controls under a specified text.</para>
        /// </summary>
        public List<ViewGroupDef> Groups { get; } = new List<ViewGroupDef>();
        /// <summary>
        /// A list of rows. Could be empty.
        /// <para>A row is a panel. It may contain a grid or rows with controls.</para>
        /// </summary>
        public List<ViewRowDef> Rows { get; } = new List<ViewRowDef>();

        /* data-setup properties */
        /// <summary>
        /// Returns the javascript class name of a view class to be used when creating the view object in javascript.
        /// </summary>
        public string JSClassType { get; set; }
        /// <summary>
        /// Returns the javascript class name of a broker class to be used when creating the broker object in javascript.
        /// </summary>
        public string JSBrokerClass { get; set; }

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
        public List<string> CssClasses { get; set; } = new List<string>() ;

        /// <summary>
        /// When true, controls are auto-created.
        /// <para>Defaults to false.</para>
        /// </summary>
        public bool AutocreateControls { get; set; } = false;


    } 

}
