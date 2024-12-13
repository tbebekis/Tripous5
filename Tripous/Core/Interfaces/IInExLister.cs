namespace Tripous
{
    /// <summary>
    /// Represents an object containing an Include and an Exclude list
    /// for the user to rearrange items in the lists.
    /// </summary>
    public interface IInExLister
    {

        /* properties */
        /// <summary>
        /// Gets a title, for display purposes
        /// </summary>
        string ListerTitle { get; }
        /// <summary>
        /// Gets the Include list
        /// </summary>
        List<IInExListerItem> InList { get; }
        /// <summary>
        /// Gets the Exclude list
        /// </summary>
        List<IInExListerItem> ExList { get; }
    }
}
