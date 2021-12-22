﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data
{

    /// <summary>
    /// Base class for all view component defs
    /// </summary>
    public class ViewDefComponent
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewDefComponent()
        {
        }


        /* public */
        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return !string.IsNullOrWhiteSpace(Name) ? Name : (!string.IsNullOrWhiteSpace(Title) ? Title : base.ToString());
        }

        /// <summary>
        /// Assigns properties to a data-setup object
        /// </summary>
        public virtual void AssignTo(Dictionary<string, object> DataSetup)
        {
            if (!string.IsNullOrWhiteSpace(Name))
                DataSetup["Name"] = Name;

            if (!string.IsNullOrWhiteSpace(Title))
                DataSetup["Title"] = Title;

            if (!string.IsNullOrWhiteSpace(TableName))
                DataSetup["TableName"] = TableName; 

            foreach (var Entry in Properties)
                DataSetup[Entry.Key] = Entry.Value;
        }
        /// <summary>
        /// Serializes this instance in order to properly used as a data-setup html attribute.
        /// </summary>
        public virtual string GetDataSetupText()
        {
            Dictionary<string, object> Temp = new Dictionary<string, object>();
            AssignTo(Temp);
            string JsonText = Json.Serialize(Temp);
            return JsonText;
        }

        /* properties */
        /// <summary>
        /// A unique name among all siblings. 
        /// </summary>
        public string Name { get; set; }

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
            get { return !string.IsNullOrWhiteSpace(TitleKey) ? Res.GS(TitleKey, TitleKey) : (!string.IsNullOrWhiteSpace(Name)? Name: Sys.None); }
            set { }
        }

        /// <summary>
        /// The data source name. When empty then it binds to its parent's source.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// Helper indexer for the Properties property.
        /// <para>Properties property is a Dictionary with properties of the data-setup attribute of this component. </para>
        /// <para>NOTE: The data-setup of a control row has the form <code>{Text: 'xxx', Control: {Prop1: value, PropN: value}}</code> </para>
        /// </summary>
        public object this[string Key]
        {
            get { return Properties.ContainsKey(Key) ? Properties[Key] : null; }
            set { Properties[Key] = value; }
        }
        /// <summary>
        /// Dictionary with properties of the data-setup attribute of this component. 
        /// <para>NOTE: The data-setup of a control row has the form <code>{Text: 'xxx', Control: {Prop1: value, PropN: value}}</code> </para>
        /// </summary>
        public Dictionary<string, object> Properties { get; } = new Dictionary<string, object>();
    }




    /// <summary>
    /// Represents a container of view def component items suchs as PanelList, TabControl and Accordeon.
    /// </summary>
    public class ViewDefContainer<T>: ViewDefComponent where T: ViewDefComponent, new()
    {
        /// <summary>
        /// Creates and returns a component item.
        /// </summary>
        protected virtual T CreateItem(string TitleKey, string Name = "")
        {
            T Result = new T()
            {
                TitleKey = TitleKey,
                Name = Name,
            };

            return Result;
        }

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewDefContainer()
        {
        }

        /// <summary>
        /// Returns a <see cref="ViewPanelListPanelDef"/> found under a specified name, if any, else null.
        /// </summary>
        public T FindTabByName(string ComponentName)
        {
            return Items.FirstOrDefault(item => Sys.IsSameText(item.Name, ComponentName));
        }
        /// <summary>
        /// Returns true if a <see cref="ViewPanelListPanelDef"/> found under a specified name.
        /// </summary>
        public bool Contains(string ComponentName)
        {
            return FindTabByName(ComponentName) != null;
        }
        /// <summary>
        /// Adds and returns a <see cref="ViewPanelListPanelDef"/>
        /// </summary>
        public T Add(string TitleKey, string Name = "")
        {
            T Result = CreateItem(TitleKey, Name);
            Items.Add(Result);
            return Result;
        }

        /* properties */
        /// <summary>
        /// A list of tabs. Could be empty. When not empty then this describes a TabControl (Pager) with child tab pages
        /// </summary>
        public List<T> Items { get; } = new List<T>();
    }



    /// <summary>
    /// Represents a panel in a container.
    /// <para>Container could be a <see cref="ViewPanelListDef"/>, a <see cref="ViewTabControlDef"/> or a <see cref="ViewAccordeonDef"/> </para>
    /// </summary>
    public class ViewDefContainerPanel : ViewDefComponent
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewDefContainerPanel()
        {
        }

        /* public */
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


        /* properties */
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
    }
}
