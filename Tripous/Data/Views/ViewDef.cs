using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data
{
 


    /// <summary>
    /// Top level container. Represents a desktop form or a html page
    /// </summary>
    public class ViewDef
    {
        static List<ViewDef> RegistryList = new List<ViewDef>();

 
        string fTitle;

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
                this.Split = Split;

            Title = Broker.Title; 

            // filters (search) tab
            ViewTabDef FilterTab = new ViewTabDef("Filters") { TitleKey = "Filters" };
            this.Tabs.Add(FilterTab);

            // list (browse) tab
            ViewTabDef ListTab = new ViewTabDef("List") { TitleKey = "List" };
            this.Tabs.Add(ListTab);

            // edit tab
            ViewTabDef EditTab = new ViewTabDef("Edit") { TitleKey = "Edit" };
            this.Tabs.Add(EditTab);

            ViewTabDef DataTab = new ViewTabDef("Data") { TitleKey = "Data" };
            EditTab.Tabs.Add(DataTab);
            DataTab.SourceName = Broker.MainTableName;

            var MainTable = Broker.MainTable;
            List<List<SqlBrokerFieldDef>> ColumnFieldLists = MainTable.Fields.Split(this.Split.Large);

            ViewColumnDef Column;
            foreach (var FieldList in ColumnFieldLists)
            {
                Column = new ViewColumnDef(FieldList);
                DataTab.Columns.Add(Column);
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
        /// A unique name among all view containers. 
        /// <para>NOTE: For DataViews this is the BrokerName.</para>
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets tha Title of this descriptor, used for display purposes.
        /// </summary>    
        public string Title
        {
            get { return !string.IsNullOrWhiteSpace(fTitle) ? fTitle : (!string.IsNullOrWhiteSpace(TitleKey) ? Res.GS(TitleKey, TitleKey) : Name); }
            set { fTitle = value; }
        }
        /// <summary>
        /// Gets or sets a resource Key used in returning a localized version of Title
        /// </summary>
        public string TitleKey { get; set; }
 

        /// <summary>
        /// Width percent of text in rows.
        /// </summary>
        public int TextSplit { get; set; } = 35;

        /// <summary>
        /// A list of tabs. Could be empty.
        /// </summary>
        public List<ViewTabDef> Tabs { get; } = new List<ViewTabDef>();
        /// <summary>
        /// A list of groups. Could be empty.
        /// </summary>
        public List<ViewGroupDef> Groups { get; } = new List<ViewGroupDef>();
        /// <summary>
        /// A list of columns. Could be empty.
        /// </summary>
        public List<ViewColumnDef> Columns { get; } = new List<ViewColumnDef>();

        /// <summary>
        /// Columns per screen size
        /// </summary>
        public UiSplit Split { get; set; } = new UiSplit();
    } 

}
