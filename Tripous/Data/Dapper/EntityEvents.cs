using System;
using System.Collections.Generic;
using System.Text;

namespace Tripous.Data 
{
    /// <summary>
    /// Delegates DomainEntity related events to registered listeners.
    /// </summary>
    static public class EntityEvents
    {
        /// <summary>
        /// Event name
        /// </summary>
        public const string SBeforeInsert = "BeforeInsert";
        /// <summary>
        /// Event name
        /// </summary>
        public const string SAfterInsert = "AfterInsert";
        /// <summary>
        /// Event name
        /// </summary>
        public const string SBeforeUpdate = "BeforeUpdate";
        /// <summary>
        /// Event name
        /// </summary>
        public const string SAfterUpdate = "AfterUpdate";
        /// <summary>
        /// Event name
        /// </summary>
        public const string SBeforeDelete = "BeforeDelete";
        /// <summary>
        /// Event name
        /// </summary>
        public const string SAfterDelete = "AfterDelete";

        /* private */
        static object syncLock = new LockObject();
        static List<IEntityListener> listeners = new List<IEntityListener>();

        static bool CanCallListener(IEntityListener Listener)
        {
            if (Sys.HasProperty(Listener, "IsDisposed"))
            {
                object V = Sys.GetProperty(Listener, "IsDisposed");
                if (V != null)
                    return !Convert.ToBoolean(V);
            }

            return true;
        }

        /* construction */
        /// <summary>
        /// constructor.
        /// </summary>
        static EntityEvents()
        {
        }

        /* public methods */
        /// <summary>
        /// Registers a Listener 
        /// </summary>
        static public void Add(IEntityListener Listener)
        {
            lock (syncLock)
            {
                try
                {
                    if (!listeners.Contains(Listener))
                    {
                        listeners.Add(Listener);
                    }
                }
                catch
                {
                }
            }
        }
        /// <summary>
        /// Unregisters a Listener 
        /// </summary>
        static public void Remove(IEntityListener Listener)
        {
            lock (syncLock)
            {
                try
                {
                    if (listeners.Contains(Listener))
                    {
                        listeners.Remove(Listener);
                    }
                }
                catch
                {
                }
            }
        }


        /// <summary>
        /// Calls all listeners synchronously. 
        /// </summary>
        /// <param name="Entity">The entity caused the event</param>
        /// <param name="Event">A string representing the event, e.g. BeforeInsert</param>
        /// <param name="Info">A special information provided by the event sender. Could be null.</param>
        static public void Send(DataEntity Entity, string Event, object Info = null)
        {
            lock (syncLock)
            {
                if (Active)
                {
                    foreach (IEntityListener Item in listeners)
                    {
                        if (CanCallListener(Item))
                        {
                            Item.HandleEntityEvent(Entity, Event, Info);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Calls all listeners asynchronously. 
        /// </summary>
        /// <param name="Entity">The entity caused the event</param>
        /// <param name="Event">A string representing the event, e.g. BeforeInsert</param>
        /// <param name="Info">A special information provided by the event sender. Could be null.</param>
        static public void Post(DataEntity Entity, string Event, object Info = null)
        {
            lock (syncLock)
            {
                if (Active)
                {
                    foreach (IEntityListener Item in listeners)
                    {
                        if (CanCallListener(Item))
                        {
                            Threads.Run(() => Item.HandleEntityEvent(Entity, Event, Info));
                        }
                    }
                }
            }
        }

        /* properties */
        /// <summary>
        /// Indicates whether this broacaster can send notifications. Defaults to true
        /// </summary>
        static public bool Active { get; set; } = true;
    }
}
