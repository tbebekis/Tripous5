using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data
{

    /// <summary>
    /// Represents a tab control
    /// </summary>
    public class ViewTabControlDef : ViewDefContainer<ViewTabPageDef>
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewTabControlDef()
        {
        }
    }



    /// <summary>
    /// Represents a page in a <see cref="ViewTabControlDef"/>
    /// </summary>
    public class ViewTabPageDef: ViewDefContainerPanel
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewTabPageDef()
        {
        }

    }
}
