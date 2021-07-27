using System;
using System.Collections.Generic;
using System.Text;

namespace Tripous
{

    /// <summary>
    /// Represents an object that has subscribed to an Broadcaster object
    /// for getting event notifications.
    /// </summary>
    public interface IBroadcasterListener
    {
        /// <summary>
        /// It represents a general event handler method for events coming from an Broadcaster object.
        /// <para>If the Args contain a "Sender" argument, then the Value of that argument is the sender
        /// of the event.</para>
        /// <para>WARNING: Broadcaster Post() method is an asynchronous call. That is it is called from
        /// a thread other than the primary thread. Controls need to check if InvokeRequired.
        /// </para> 
        /// </summary>
        void HandleBroadcasterEvent(string EventName, IDictionary<string, object> Args);
    }
}
