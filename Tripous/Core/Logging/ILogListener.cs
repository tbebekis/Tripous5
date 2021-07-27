/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;


namespace Tripous.Logging
{

    /// <summary>
    /// A listener for LogInfo messages.
    /// </summary>
    public interface ILogListener
    {
        /// <summary>
        /// Called by the Logger to pass LogInfo to a log listener.
        ///<para>
        /// CAUTION: The Logger calls its Listeners asynchronously, that is from inside a thread.
        /// Thus Listeners should synchronize the ProcessLogInfo() call. Controls need to check if InvokeRequired.
        /// </para>
        /// </summary>
        void ProcessLog(LogInfo Info);
    }
}