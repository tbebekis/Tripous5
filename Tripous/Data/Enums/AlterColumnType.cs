using System;
using System.Collections.Generic;
using System.Text;

namespace Tripous.Data
{

    /// <summary>
    /// Indicates the type of alteration to be done in a column
    /// </summary>
    public enum AlterColumnType
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Add
        /// </summary>
        Add = 1,
        /// <summary>
        /// Alter
        /// </summary>
        Alter = 2,
        /// <summary>
        /// Drop
        /// </summary>
        Drop = 3,
        /// <summary>
        /// Rename
        /// </summary>
        Rename = 4,
    }
}
