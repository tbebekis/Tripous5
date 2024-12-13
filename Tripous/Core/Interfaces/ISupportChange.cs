namespace Tripous
{
    /// <summary>
    /// Represents an object which provides a Change() method which is called when any of its properties or items changes.
    /// </summary>
    public interface ISupportChange
    {
        /// <summary>
        /// Notifies about a change in an item.
        /// <para>Item could be this instance or an internal list element.</para>
        /// </summary>
        void Change(object Item);


        /// <summary>
        /// Occures when the Change() method is called.
        /// </summary>
        event EventHandler<ChangedEventArgs> Changed;
    }
}
