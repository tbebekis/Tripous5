namespace Tripous
{


    /// <summary>
    /// A listener that synchronizes the <see cref="ProcessBroadcasterMessage(BroadcasterArgs)"/> call to any context.
    /// <para>That is, it is safe to attach to the <see cref="BroadcasterListener.OnMessage"/> a GUI element such as a Form or a TextBox.</para>
    /// <para>SEE: <see cref="SynchronizationContext.Post(SendOrPostCallback, object?)"/> which reverses the responsibility of thread synchronization. </para>
    /// <para>Client code has just to link to <see cref="BroadcasterListener.OnMessage"/> and then process the passed <see cref="BroadcasterArgs"/> safely.</para>
    /// <para>SEE: https://lostechies.com/gabrielschenker/2009/01/23/synchronizing-calls-to-the-ui-in-a-multi-threaded-application/</para>
    /// </summary>
    public class SyncedBroadcasterListener: BroadcasterListener
    {
        SynchronizationContext fSyncContext;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public SyncedBroadcasterListener(): base()
        {
            fSyncContext = AsyncOperationManager.SynchronizationContext;
        }

        /* public */
        /// <summary>
        /// Called by the Logger to pass LogInfo to a log listener.
        ///<para>
        /// CAUTION: The Logger calls its Listeners asynchronously, that is from inside a thread.
        /// Thus Listeners should synchronize the ProcessLogInfo() call. Controls need to check if InvokeRequired.
        /// <para>NOTE: This listener synchronizes this call to any context.</para>
        /// </para>
        /// </summary>
        public override void ProcessBroadcasterMessage(BroadcasterArgs Args)
        {
            if (Active)
                fSyncContext.Post(e => OnMessage(e as BroadcasterArgs), Args);
        }


    }
}
