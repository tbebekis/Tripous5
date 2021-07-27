/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System; 

namespace Tripous.Data
{
    /// <summary>
    /// Flags for the SqlBroker batch commit operation
    /// </summary>
    [Flags]
    public enum BatchCommitFlags
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Trigger any broker script event
        /// </summary>
        TriggerScriptEvents = 1,
        /// <summary>
        /// Call any custom action (SysActions)
        /// </summary>
        CallCustomActions = 2,
    }
}
