using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tripous
{
    /// <summary>
    /// Represents an object that sends event notifications to its subscribed listeners.
    /// <para>A Broadcaster resembles a mailing list. A message, an event notification actually, is propagated to
    /// all of the subscribed listeners.</para>
    /// <para>Any code can use the Broadcaster to send a message to its subscribers.</para>
    /// <para>The Send() methods, send the event sychronously while the Post() 
    /// send the event asychronously.</para>
    /// </summary>
    static public class Broadcaster
    {

        /* private */
        static object syncLock = new LockObject();
        static List<IBroadcasterListener> listeners = new List<IBroadcasterListener>();

        static bool CanCallListener(IBroadcasterListener Listener)
        {
            if (Sys.HasProperty(Listener, "IsDisposed"))
            {
                object V = Sys.GetProperty(Listener, "IsDisposed");
                if (V != null)
                    return !Convert.ToBoolean(V);
            }

            return true;
        }

        /* constructors */
        /// <summary>
        /// constructor.
        /// </summary>
        static Broadcaster()
        {
        }

        /* public methods */
        /// <summary>
        /// Registers Listener to the Broadcaster.
        /// </summary>
        static public void Add(IBroadcasterListener Listener)
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
        /// Unregisters Listener from the Broadcaster.
        /// </summary>
        static public void Remove(IBroadcasterListener Listener)
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
        /// <para>The Args may contain an Event element which will be the "name" of the event.</para>
        /// <para>If the Args contain a "Sender" argument, then the Value of that argument is the sender
        /// of the event.</para>
        /// </summary> 
        static public void Send(string EventName, IDictionary<string, object> Args = null)
        {
            lock (syncLock)
            {
                if (Active)
                {
                    if (Args == null)
                    {
                        Args = new Dictionary<string, object>();
                        Args["EventName"] = EventName;
                    }

                    object Sender = Args.ContainsKey("Sender") ? Args["Sender"] : null;
                    IBroadcasterListener[] Items = listeners.ToArray();

                    foreach (IBroadcasterListener Item in Items)
                    {
                        if (!CanCallListener(Item))
                            continue;

                        if (Sender != Item)
                        {
                            Item.HandleBroadcasterEvent(EventName, Args);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Calls all listeners synchronously. 
        /// <para>Sender is the sender object, while EventName is the "name" of the event.</para>
        /// </summary>
        static public void Send(object Sender, string EventName)
        {
            Dictionary<string, object> Args = new Dictionary<string, object>();
            Args["Sender"] = Sender;
            Args["EventName"] = EventName;
 
            Send(EventName, Args);
        }

        /// <summary>
        /// Calls all listeners asynchronously. 
        /// <para>The Args may contain an Event element which will be the "name" of the event.</para>
        /// <para>If the Args contain a "Sender" argument, then the Value of that argument is the sender
        /// of the event.</para>
        /// </summary> 
        static public void Post(string EventName, IDictionary<string, object> Args = null)
        {
            lock (syncLock)
            {
                if (Active)
                {
                    if (Args == null)
                    {
                        Args = new Dictionary<string, object>();
                        Args["EventName"] = EventName;
                    }

                    object Sender = Args.ContainsKey("Sender") ? Args["Sender"] : null;
                    IBroadcasterListener[] Items = listeners.ToArray();

                    foreach (IBroadcasterListener Item in Items)
                    {
                        if (!CanCallListener(Item))
                            continue;

                        if (Sender != Item)
                        {
                            Task.Run(() => {
                                Item.HandleBroadcasterEvent(EventName, Args);
                            }); 
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Calls all listeners asynchronously. 
        /// <para>Sender is the sender object, while EventName is the "name" of the event.</para>
        /// </summary>
        static public void Post(object Sender, string EventName)
        {
            Dictionary<string, object> Args = new Dictionary<string, object>();
            Args["Sender"] = Sender;
            Args["EventName"] = EventName;

            Post(EventName, Args);
        }

        /* properties */
        /// <summary>
        /// Indicates whether this broacaster can send notifications. Defaults to true
        /// </summary>
        static public bool Active { get; set; } = true;

    }
}
