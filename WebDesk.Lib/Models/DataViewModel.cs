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
    /// A model for a standard data-view.
    /// <para>A standard data-view has three parts: Brower, Edit and Filters.</para>
    /// <para>The Browser part contains/is the browser grid.</para>
    /// <para>The Filters part contains the filter controls.</para>
    /// <para>The Edit part contains a pager (TabControl) with one or more tab-pages for editing/inserting data.</para>
    /// </summary>
    public class DataViewModel 
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public DataViewModel(ViewDef ViewDef)
        {
            this.ViewDef = ViewDef;
            this.Setup.BrokerName = ViewDef.BrokerName;
        }

        /* properties */
        /// <summary>
        /// The definition of the view
        /// </summary>
        public ViewDef ViewDef { get; }
        /// <summary>
        /// Used as the generator of the data-setup attribute value for a default DataView razor view.
        /// </summary>
        public DataViewSetup Setup { get; } = new DataViewSetup();
    }


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


    /// <summary>
    /// A model for a row
    /// </summary>
    public class DataViewRowModel 
    {
        /// <summary>
        /// Constant. Default column and width classes
        /// </summary>
        public const string DefaultColumnCssClasses = "tp-Col l-75 m-70 s-70 xs-100 ";

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public DataViewRowModel(ViewDef ViewDef, ViewRowDef RowDef)
        {
            this.ViewDef = ViewDef;
            this.RowDef = RowDef;

            this.ColumnCssClasses = ViewDef.ColumnSplit.GetColumnCssClasses();
        }
 

        /* properties */
        /// <summary>
        /// Column and width classes. Not used when columns is empty.
        /// </summary>
        public string ColumnCssClasses { get; set; }

        /// <summary>
        /// The definition of the view
        /// </summary>
        public ViewDef ViewDef { get; }
        /// <summary>
        /// The definition of the row.
        /// </summary>
        public ViewRowDef RowDef { get; }
    }


}
