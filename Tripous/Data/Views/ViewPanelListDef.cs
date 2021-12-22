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
    public class ViewPanelListDef : ViewDefContainer<ViewPanelListPanelDef>
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewPanelListDef()
        {
        }
    }
 

    /// <summary>
    /// Represents a page in a <see cref="ViewPanelListDef"/>
    /// </summary>
    public class ViewPanelListPanelDef : ViewDefContainerPanel
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewPanelListPanelDef()
        {
        }


    }

}
