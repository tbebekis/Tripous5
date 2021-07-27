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
    /// Indicates the stage (phase) of a transaction operation
    /// </summary>
    public enum TransactionStage
    {
        /// <summary>
        /// At the start transaction
        /// </summary>
        Start,
        /// <summary>
        /// At the commit transaction
        /// </summary>
        Commit,
        /// <summary>
        /// 
        /// </summary>
        Rollback,
    }
}
