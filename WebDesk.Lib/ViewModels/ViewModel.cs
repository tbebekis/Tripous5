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
    /// A model for a view.
    /// </summary>
    public class ViewModel
    {
         /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewModel(ViewDef Def)
        {
            this.Def = Def; 
        }
 
        /* properties */
        /// <summary>
        /// The definition of the view
        /// </summary>
        public ViewDef Def { get; } 
    }

    public class ViewPanelListModel
    {
        public ViewPanelListModel(ViewDef ViewDef, ViewPanelListDef Def)
        {
            this.ViewDef = ViewDef;
            this.Def = Def;
        }

        public ViewDef ViewDef { get; }
        public ViewPanelListDef Def { get; }
    }


    public class ViewAccordeonModel
    {
        public ViewAccordeonModel(ViewDef ViewDef, ViewAccordeonDef Def)
        {
            this.ViewDef = ViewDef;
            this.Def = Def;
        }

        public ViewDef ViewDef { get; }
        public ViewAccordeonDef Def { get; }
    }

    /// <summary>
    /// A model for a pager (TabControl)
    /// </summary>
    public class ViewTabControlModel
    {
        /* construction */
        /// <summary>
        /// Constructor.
        /// <para>Constructs a model for the Pager (TabControl) of the Edit part (DIV) of a standard data view.</para>
        /// <para>The other two parts of a standard data-view are the Browser (DIV) and the Filters (DIV) part.</para>
        /// </summary>
        public ViewTabControlModel(ViewDef ViewDef, ViewTabControlDef Def)
        {
            this.ViewDef = ViewDef;
            this.Def = Def;
        }

        /* properties */ 
        /// <summary>
        /// The definition of the view
        /// </summary>
        public ViewDef ViewDef { get; }
        /// <summary>
        /// The pager (TabControl)
        /// </summary>
        public ViewTabControlDef Def { get; }
    }



    /// <summary>
    /// A model for a row
    /// </summary>
    public class ViewRowModel
    {
 
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewRowModel(ViewDef ViewDef, ViewRowDef Def)
        {
            this.ViewDef = ViewDef;
            this.Def = Def;
            this.Def.ColumnCssClasses = ViewDef.ColumnSplit.GetColumnCssClasses();
        }

        /* properties */
        /// <summary>
        /// The definition of the view
        /// </summary>
        public ViewDef ViewDef { get; }
        /// <summary>
        /// The definition of the row.
        /// </summary>
        public ViewRowDef Def { get; }
    }

    /// <summary>
    /// A model for a row
    /// </summary>
    public class ViewContainerPanelModel
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewContainerPanelModel(ViewDef ViewDef, ViewDefContainerPanel Def)
        {
            this.ViewDef = ViewDef;
            this.Def = Def; 
        }

        /* properties */
        /// <summary>
        /// The definition of the view
        /// </summary>
        public ViewDef ViewDef { get; }
        /// <summary>
        /// The definition of the container.
        /// </summary>
        public ViewDefContainerPanel Def { get; }
    }

     
}
