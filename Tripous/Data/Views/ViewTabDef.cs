using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data
{

    /// <summary>
    /// Represents a tab page or a TabControl (Pager) with child tab pages.
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
        /// Returns a <see cref="ViewTabDef"/> found under a specified Id, if any, else null.
        /// </summary>
        public ViewTabDef GetTabById(string Id)
        {
            return Tabs.Find(item => Sys.IsSameText(item.Id, Id));
        }




        /* properties */
        /// <summary>
        /// A unique Id among sibling tabs, e.g. Filters, List, Edit, etc
        /// </summary>
        public string Id { get; set; }

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
            get { return !string.IsNullOrWhiteSpace(TitleKey) ? Res.GS(TitleKey, TitleKey) : Id; }
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
}
