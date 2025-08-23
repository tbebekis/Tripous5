namespace Tripous
{
    /// <summary>
    /// A simple application instance manager.
    /// <para>User a mutex  in order to check for any running application instance.</para>
    /// </summary>
    public class InstanceManager : Disposable
    {
        Mutex mutex;

        /// <summary>
        /// Disposes managed or un-managed resources, according to the Managed passed in flag.
        /// </summary>
        protected override void Dispose(bool Managed)
        {
            if (!IsDisposed && !Managed)
            {
                if (mutex != null)
                {
                    try
                    {
                        mutex.ReleaseMutex();
                    }
                    catch
                    {
                    }

                    mutex = null;
                }
            }
        }

        // ● construction 
        /// <summary>
        /// Constructor.
        /// <para>UniqueId should be a name that uniquely identifies the application, preferably a Guid stirng.</para>
        /// </summary>
        public InstanceManager(string UniqueId, int TimeoutMSecs = 1500)
        {
            try
            {
                mutex = new Mutex(false, GlobalMutexName(UniqueId));
                IsSingleInstance = mutex.WaitOne(TimeoutMSecs);
            }
            catch
            {
            }
        }

        /// <summary>
        /// For a really global mutex name, that is "shared" between users/sessions 
        /// we have to prefix the name of the mutex with "Global\"
        /// </summary>
        static public string GlobalMutexName(string MutexName)
        {
            return string.Format(@"Global\{0}", MutexName);  // http://stackoverflow.com/questions/7672596/system-threading-mutex-with-service-and-console
        }

        // ● properties 
        /// <summary>
        /// Returns true if this application runs just once.
        /// </summary>
        public bool IsSingleInstance { get; }
    }
}
