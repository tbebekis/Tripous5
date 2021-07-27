/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;

namespace Tripous
{

    /// <summary>
    /// Indicates the location object that a Locate() call uses
    /// </summary>
    [Flags]
    public enum LocateOptions
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Indicates a case insensitive operation
        /// </summary>
        CaseInsensitive = 1,
        /// <summary>
        /// Indicates a partial key operation
        /// </summary>
        PartialKey = 2,
        /// <summary>
        /// Indicates that both flags are used
        /// </summary>
        Both = CaseInsensitive | PartialKey,
    }
}