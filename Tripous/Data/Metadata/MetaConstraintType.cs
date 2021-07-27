/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;


namespace Tripous.Data.Metadata
{

    /// <summary>
    /// Indicates the type of a constraint
    /// </summary>
    public enum MetaConstraintType
    {
        /// <summary>
        /// PrimaryKey
        /// </summary>
        PrimaryKey,
        /// <summary>
        /// ForeignKey
        /// </summary>
        ForeignKey,
        /// <summary>
        /// UniqueKey
        /// </summary>
        UniqueKey,
        /// <summary>
        /// Check constraint
        /// </summary>
        Check
    }
}
