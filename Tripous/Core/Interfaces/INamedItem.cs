namespace Tripous
{


    /// <summary>
    /// Represents an object which provides a Name property
    /// </summary>
    public interface INamedItem : ICollectionItem
    {
        /// <summary>
        /// Gets or sets the Name of this instance.
        /// </summary>
        string Name { get; set; }
    }

}
