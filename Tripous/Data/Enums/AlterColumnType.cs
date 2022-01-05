using System;
using System.Collections.Generic;
using System.Text;

namespace Tripous.Data
{

    /// <summary>
    /// Indicates the type of alteration to be done in a table column
    /// </summary>
    [Flags]
    public enum AlterColumnType
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Add Column
        /// </summary>
        Add = 1,
        /// <summary>
        /// Drop Column
        /// </summary>
        Drop = 2,
        /// <summary>
        /// Rename Column
        /// </summary>
        Rename = 4,
        /// <summary>
        /// Set Column Length
        /// </summary>
        Length = 8,
        /// <summary>
        /// Set Column to not null
        /// </summary>
        NotNull = 0x10,
        /// <summary>
        /// Drop not null from Column
        /// </summary>
        DropNotNull = 0x20,
        /// <summary>
        /// Set Column default value
        /// </summary>
        Default = 0x40,
        /// <summary>
        /// Drop Column default value
        /// </summary>
        DropDefault = 0x80,

    }
}
