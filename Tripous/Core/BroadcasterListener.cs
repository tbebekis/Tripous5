using System;
using System.Collections.Generic;
using System.Text;
using Tripous.Core;
using Tripous.Logging;

namespace Tripous
{


    /// <summary>
    /// Represents an object that has subscribed to the <see cref="Broadcaster"/> static object
    /// for getting event notifications.
    /// </summary>
    public abstract class BroadcasterListener
    {
        protected void OnMessage(BroadcasterArgs Args)
        {
            if (Active && MessageEvent != null)
            {
                MessageEvent(this, Args);
            }
        }

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public BroadcasterListener()
        {
            Register();
        }

        /* public */
        /// <summary>
        /// Registers this listener to <see cref="Broadcaster"/>
        /// </summary>
        public void Register()
        {
            Broadcaster.Add(this);
        }
        /// <summary>
        /// Unregisters this listener to <see cref="Broadcaster"/>
        /// </summary>
        public void Unregister()
        {
            Broadcaster.Remove(this);
        }

        /// <summary>
        /// It represents a general event handler method for events coming from the Broadcaster.
        /// <para>WARNING: Broadcaster Post() method is an asynchronous call. That is it is called from
        /// a thread other than the primary thread. Controls need to check if InvokeRequired.
        /// </para> 
        /// </summary>
        public abstract void ProcessBroadcasterMessage(BroadcasterArgs Args);

        /* properties */
        /// <summary>
        /// When false then it should not process incoming events.
        /// </summary>
        static public bool Active { get; set; } = true;
        /// <summary>
        /// Occurs when a new <see cref="BroadcasterArgs"/> is available.>
        /// </summary>
        public event EventHandler<BroadcasterArgs> MessageEvent;
    }
}
