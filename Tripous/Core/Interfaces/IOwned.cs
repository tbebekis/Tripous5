namespace Tripous
{
    /// <summary>
    /// Represents an object that has an owner.
    /// </summary>
    public interface IOwned
    {
        /// <summary>
        /// Gets or sets the owner of this instance.
        /// </summary>
        object Owner { get; set; }
    }
}
