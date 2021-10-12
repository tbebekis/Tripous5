/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;


namespace Tripous
{
    /// <summary>
    /// Indicates the kind of the aggregate function to be used 
    /// </summary>
    [Flags]
    public enum AggregateFunctionType
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Count
        /// </summary>
        Count = 0x1,
        /// <summary>
        /// Average
        /// </summary>
        Avg = 0x2,
        /// <summary>
        /// Sum
        /// </summary>
        Sum = 0x4,
        /// <summary>
        /// Max
        /// </summary>
        Max = 0x8,
        /// <summary>
        /// Min
        /// </summary>
        Min = 0x10,
    }

}