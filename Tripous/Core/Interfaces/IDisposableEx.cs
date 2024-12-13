namespace Tripous
{
    /// <summary>
    /// Extends the IDisposable
    /// </summary>
    public interface IDisposableEx : IDisposable
    {
        /// <summary>
        /// Returns true when this instance is disposed.
        /// </summary>
        bool IsDisposed { get; }

        /* events */
        /// <summary>
        /// Occurs when this intance is disposed
        /// </summary>
        event EventHandler Disposed;
    }
}
