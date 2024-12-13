namespace Tripous
{
    /// <summary>
    /// Represents a list where each of its items may have a unique Name.
    /// </summary>
    public interface IUniqueNamesList
    {
        /// <summary>
        /// Throws an exception of Name is not unique in the list.
        /// </summary>
        void CheckUniqueName(object Item, string Name);
        /// <summary>
        /// Indicates whether items in the list should have or not unique names.
        /// </summary>
        bool UniqueNames { get; set; }
    }
}
