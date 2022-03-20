using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Forms
{
    /// <summary>
    /// Indicates the type of message box 
    /// </summary>
    [Flags]
    public enum MessageBoxType
    {
        /// <summary>
        /// Indicates an information message box
        /// </summary>
        Info = 1,
        /// <summary>
        /// Indicates an error message box
        /// </summary>
        Error = 2,
        /// <summary>
        /// Indicates a message box with Yes/No buttons
        /// </summary>
        YesNo = 4,
        /// <summary>
        /// Indicates a message box with Yes/No/Cancel buttons
        /// </summary>
        YesNoCancel = 8
    }
}
