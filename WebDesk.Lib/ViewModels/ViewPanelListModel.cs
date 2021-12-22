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
    /// A model for an PanelList control
    /// </summary>
    public class ViewPanelListModel
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewPanelListModel(ViewDef ViewDef, ViewPanelListDef Def)
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
        /// The definition associated to this model
        /// </summary>
        public ViewPanelListDef Def { get; }
    }
}
