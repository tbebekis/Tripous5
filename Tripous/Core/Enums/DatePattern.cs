using System;
using System.Collections.Generic;
using System.Text;

namespace Tripous
{
    /// <summary>
    /// Indicates the pattern of a date format string.
    /// </summary>
    public enum DatePattern
    {
        /// <summary>
        /// MM-dd-yyyy. Middle-endian (month, day, year), e.g. 04/22/96  
        /// </summary>
        MDY,
        /// <summary>
        /// dd-MM-yyyy. Little-endian (day, month, year), e.g. 22.04.96 or 22/04/96
        /// </summary>
        DMY,
        /// <summary>
        /// yyyy-MM-dd. Big-endian (year, month, day), e.g. 1996-04-22
        /// </summary>
        YMD
    }
}
