using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tripous;
using Tripous.Data;


namespace WebLib.Models
{

    /// <summary>
    /// A model for a pager (TabControl)
    /// </summary>
    public class DataViewPagerModel
    {
        /* construction */
        /// <summary>
        /// Constructor.
        /// <para>Constructs a model for the Pager (TabControl) of the Edit part (DIV) of a standard data view.</para>
        /// <para>The other two parts of a standard data-view are the Browser (DIV) and the Filters (DIV) part.</para>
        /// </summary>
        public DataViewPagerModel(ViewDef ViewDef, ViewTabDef Pager = null)
        {
            this.ViewDef = ViewDef;

            // we always expect that the Edit tab represents a TabControl with tab pages.
            if (Pager == null)
                Pager = ViewDef.Tabs.Find(item => Sys.IsSameText(item.Id, "Edit"));

            this.Pager = Pager;
        }

        /* properties */
        /// <summary>
        /// Css classes for the pager, e.g. 'TabControl ViewDataPager'
        /// </summary>
        public string CssClasses { get; set; } = "TabControl ";
        /// <summary>
        /// Text of the data-setup attribute, e.g. '{ SelectedIndex: 0 }'
        /// </summary>
        public string DataSetupText { get; set; } = "{ SelectedIndex: 0 }";

        /// <summary>
        /// The definition of the view
        /// </summary>
        public ViewDef ViewDef { get; }
        /// <summary>
        /// The pager (TabControl)
        /// </summary>
        public ViewTabDef Pager { get; }
    }

}
