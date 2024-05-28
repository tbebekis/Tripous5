using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tripous;
using Tripous.Logging;
using Tripous.Data;

namespace WebLib
{
    /// <summary>
    /// LogListener, saves log information to database
    /// </summary>
    internal class DataLogListener : ILogListener
    {
        /// <summary>
        /// Called by the Logger to pass LogInfo to a log listener.
        ///<para>
        /// CAUTION: The Logger calls its Listeners asynchronously, that is from inside a thread.
        /// Thus Listeners should synchronize the ProcessLogInfo() call. Controls need to check if InvokeRequired.
        /// </para>
        /// </summary>
        public void ProcessLog(LogEntry Info)
        {
            // TODO: DataLogListener
        }
    }
}
