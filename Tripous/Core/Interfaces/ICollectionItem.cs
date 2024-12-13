namespace Tripous
{
    /// <summary>
    /// Represenents an object that belongs to a collection.
    /// </summary>
    public interface ICollectionItem
    {
        /// <summary>
        /// Gets or sets the owner collection of this instance.
        /// </summary>
        IList Collection { get; set; }
    }
}
