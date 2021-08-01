/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Data;
using System.Data.Common;

namespace Tripous.Data
{
    /// <summary>
    /// Flags indicating the behavior of S statement generators
    /// </summary>
    [Flags]
    public enum BuildSqlFlags
    {
        /// <summary>
        /// Container flag
        /// </summary>
        None = 0,
        /// <summary>
        /// When is set indicates that the OIDs must be created before an INSERT statement execution.
        /// <para>This is the case with an Oracle sequencer or Firebird generator or when the OID is a Guid or anyother string.</para> 
        /// </summary>
        OidModeIsBefore = 1,
        /// <summary>
        /// When is set indicates that the OID is a Guid string.  
        /// </summary>
        GuidOids = 2,
        /// <summary>
        /// When is set indicates that blob fields must included into the SELECT of the Browse part.
        /// </summary>
        BrowseBlobFields = 4,
        /// <summary>
        /// When is set indicates that look up fields are included in the generated S with join clauses
        /// </summary>
        JoinLookUpFields = 8,
        /// <summary>
        /// When is set indicates that look up DataTables must created too, along with S generation
        /// </summary>
        CreateLookUpTables = 16
    }

}
