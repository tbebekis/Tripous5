namespace Tripous
{

    /// <summary>
    /// A default implementation of the IDisposable
    /// </summary>
    public abstract class Disposable : IDisposableEx, IDisposable
    {
    

        /// <summary>
        /// The disposing parameter is true when this method is called
        /// by the public Dispose() method, that is by application code.
        /// The disposing parameter is false when this method is called 
        /// by the destructor, that is by the GC.    
        /// </summary>
        void DisposeInstance(bool disposing)
        {
            if (!this.IsDisposed)
            {
                if (disposing)
                {
                    // dispose managed resources here 
                    Dispose(true);
                }

                // dispose unmanaged resources here
                Dispose(false);

                IsDisposed = true;

                if (Disposed != null)
                {
                    try
                    {
                        Disposed(this, EventArgs.Empty);
                    }
                    catch
                    {
                    }
                }
            }
        }
        /// <summary>
        /// Disposes managed or un-managed resources, according to the Managed passed in flag.
        /// </summary>
        protected abstract void Dispose(bool Managed);
  

        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        public Disposable()
        {
        }
        /// <summary>
        /// Destructor.
        /// </summary>
        ~Disposable()
        {
            DisposeInstance(false);
        }

        /* public */
        /// <summary>
        /// Disposes this instance
        /// </summary>
        public void Dispose()
        {
            if (!this.IsDisposed)
            {
                DisposeInstance(true);
                GC.SuppressFinalize(this);  // instructs GC not bother to call the destructor
            }
        }

        /* properties */
        /// <summary>
        /// Returns true when this instance is disposed.
        /// </summary>
        [Browsable(false)]
        public bool IsDisposed { get; protected set; }

        /* events */
        /// <summary>
        /// Occurs when this intance is disposed
        /// </summary>
        public event EventHandler Disposed;
    }

}
