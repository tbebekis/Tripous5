using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data
{

    /// <summary>
    /// Represents a single tab page or a TabControl (Pager) with child tab pages. 
    /// <para>May contain: Tabs, Groups and Rows.</para>
    /// <para>When <see cref="Tabs"/> is empty then this instance is a signle tab page.</para>
    /// <para>When <see cref="Tabs"/> is NOT empty then this instance is a TabControl (Pager) with child tab pages.</para>
    /// <para><see cref="Tabs"/>, <see cref="Groups"/> and <see cref="Rows"/> are checked in that order. If any is not empty the rest are ignored.</para>
    /// <para>Contains a single Pager (TabControl) when the <see cref="Tabs"/> are not empty. </para>
    /// <para>Contains a single Accordeon when the <see cref="Groups"/> is not empty. </para>
    /// <para>Contains a signle Panel (DIV) with one or more rows when the <see cref="Rows"/> is not empty. </para>
    /// </summary>
    public class ViewTabDef  
    {


        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewTabDef()
        {
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

        /* properties */
        /// <summary>
        /// A unique Id among sibling tabs, e.g. List, Edit, Filters,  etc
        /// </summary>
        public string TabId { get; set; }

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
            get { return !string.IsNullOrWhiteSpace(TitleKey) ? Res.GS(TitleKey, TitleKey) : TabId; }
            set { }
        }

        /// <summary>
        /// The data source name. When empty then it binds to its parent's source.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// A list of tabs. Could be empty. When not empty then this describes a TabControl (Pager) with child tab pages
        /// </summary>
        public List<ViewTabDef> Tabs { get; } = new List<ViewTabDef>();
        /// <summary>
        /// A list of groups. Could be empty.
        /// </summary>
        public List<ViewGroupDef> Groups { get; } = new List<ViewGroupDef>();
        /// <summary>
        /// A list of rows. Could be empty.
        /// </summary>
        public List<ViewRowDef> Rows { get; } = new List<ViewRowDef>();
 
    }

    /// <summary>
    /// Extensions
    /// </summary>
    static public class ViewTabDefExtensions
    {
        /// <summary>
        /// Returns a <see cref="ViewTabDef"/> found under a specified Id, if any, else null.
        /// </summary>
        static public ViewTabDef FindTabById(this IEnumerable<ViewTabDef> Tabs, string TabId)
        {
            return Tabs.FirstOrDefault(item => Sys.IsSameText(item.TabId, TabId));
        }
        /// <summary>
        /// Returns true if a <see cref="ViewTabDef"/> found under a specified Id.
        /// </summary>
        static public bool Contains(this IEnumerable<ViewTabDef> Tabs, string TabId)
        {
            return Tabs.FindTabById(TabId) != null;
        }
    }
}
