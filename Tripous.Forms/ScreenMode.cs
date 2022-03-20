using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Forms
{
    /// <summary>
    /// Screen mode
    /// </summary>
    [Flags]
    public enum ScreenMode
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// XSmall
        /// </summary>
        XSmall = 1,
        /// <summary>
        /// Small
        /// </summary>
        Small = 2,
        /// <summary>
        /// Medium
        /// </summary>
        Medium = 4,
        /// <summary>
        /// Large
        /// </summary>
        Large = 8,
    }
}

/*
     None: 0,
    XSmall: 1,     //    0 ..  767
    Small: 2,      //  768 ..  991
    Medium: 4,     //  992 .. 1200
    Large: 8       // 1201 .. 
     */
