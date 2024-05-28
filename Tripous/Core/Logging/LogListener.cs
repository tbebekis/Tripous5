/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using Tripous.Data;

namespace Tripous.Logging
{

 

    /// <summary>
    /// A listener for <see cref="LogEntry"/> messages.
    /// <para>A listener registers itself automatically with <see cref="Logger"/>, upon construction.</para>
    /// </summary>
    public abstract class LogListener
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public LogListener()
        {
            Register();
        }


        /// <summary>
        /// Called by the Logger to pass <see cref="LogEntry"/> to a log listener.
        ///<para>
        /// CAUTION: The Logger calls its Listeners asynchronously, that is from inside a thread.
        /// Thus Listeners should synchronize the ProcessLog() call. Controls need to check if InvokeRequired.
        /// </para>
        /// </summary>
        public abstract void ProcessLog(LogEntry Info);


        /// <summary>
        /// Registers this listener to <see cref="Logger"/>
        /// </summary>
        public void Register()
        {
            Logger.Add(this);
        }
        /// <summary>
        /// Unregisters this listener to <see cref="Logger"/>
        /// </summary>
        public void Unregister()
        {
            Logger.Remove(this);
        }
    }
}