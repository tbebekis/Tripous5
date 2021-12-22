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
    public class ViewAccordeonDef : ViewDefContainer<ViewAccordeonPanelDef>
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewAccordeonDef()
        {
        }
    }


    /// <summary>
    /// Represents a panel in a <see cref="ViewAccordeonDef"/>
    /// </summary>
    public class ViewAccordeonPanelDef: ViewDefContainerPanel
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewAccordeonPanelDef()
        {
        }

    }
}
