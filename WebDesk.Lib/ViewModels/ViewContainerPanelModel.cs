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
    /// A model for a panel (TabPage, Accordion panel, Panel) in a container
    ///  <para>Container could be a <see cref="ViewPanelListDef"/>, a <see cref="ViewTabControlDef"/> or a <see cref="ViewAccordionDef"/> </para>
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
