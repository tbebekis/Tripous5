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
        static List<BroadcasterListener> listeners = new List<BroadcasterListener>();

        static bool CanCallListener(BroadcasterListener Listener)
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
        static internal void Add(BroadcasterListener Listener)
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
        static internal void Remove(BroadcasterListener Listener)
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
        /// <para>Sender is the sender object, while EventName is the "name" of the event.</para>
        /// </summary> 
        static public void Send(string EventName, object Sender, IDictionary<string, object> Params)
        {
            lock (syncLock)
            {
                if (Active)
                {
                    BroadcasterArgs Args = new BroadcasterArgs(EventName, Sender, Params);
                    BroadcasterListener[] Items = listeners.ToArray();

                    foreach (BroadcasterListener Item in Items)
                    {
                        if (!CanCallListener(Item))
                            continue;

                        if (Sender != Item)
                        {
                            Item.ProcessBroadcasterMessage(Args);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Calls all listeners synchronously. 
        /// <para>Sender is the sender object, while EventName is the "name" of the event.</para>
        /// </summary>
        static public void Send(string EventName, object Sender)
        {
            Send(EventName, Sender, new Dictionary<string, object>());
        }

        /// <summary>
        /// Calls all listeners asynchronously. 
        /// <para>Sender is the sender object, while EventName is the "name" of the event.</para>
        /// </summary> 
        static public void Post(string EventName, object Sender, IDictionary<string, object> Params)
        {
            lock (syncLock)
            {
                if (Active)
                {
                    BroadcasterArgs Args = new BroadcasterArgs(EventName, Sender, Params);
                    BroadcasterListener[] Items = listeners.ToArray();

                    foreach (BroadcasterListener Item in Items)
                    {
                        if (!CanCallListener(Item))
                            continue;

                        if (Sender != Item)
                        {
                            Task.Run(() => {
                                Item.ProcessBroadcasterMessage(Args);
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
        static public void Post(string EventName, object Sender)
        {
            Post(EventName, Sender, new Dictionary<string, object>());
        }

        /* properties */
        /// <summary>
        /// Indicates whether this broacaster can send notifications. Defaults to true
        /// </summary>
        static public bool Active { get; set; } = true;

    }
}
