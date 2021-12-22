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
}
