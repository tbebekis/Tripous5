using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data
{

    /// <summary>
    /// Represents an accordeon control
    /// </summary>
    public class ViewAccordionDef : ViewDefContainer<ViewAccordionPanelDef>
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewAccordionDef()
        {
        }
    }


    /// <summary>
    /// Represents a panel in a <see cref="ViewAccordionDef"/>
    /// </summary>
    public class ViewAccordionPanelDef: ViewDefContainerPanel
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewAccordionPanelDef()
        {
        }

    }
}
