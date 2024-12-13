namespace Tripous
{



    /// <summary>
    /// Represents a list of <see cref="INamedItem"/> items.
    /// </summary>
    public interface INamedItems<T> : IList<T>, ICollection<T>, IEnumerable<T>, IList, ICollection, IEnumerable, IAssignable, IUniqueNamesList, IOwned where T : class, INamedItem, new()
    {
        /// <summary>
        /// Creates an Item with Name and adds it to the list.
        /// </summary>
        T Add(string Name);
        /// <summary>
        /// Removes an Item with Name from list, if any
        /// </summary>
        void Remove(string Name);
        /// <summary>
        /// Returns the index of an Item with Name in the list.
        /// </summary>
        int IndexOf(string Name);
        /// <summary>
        /// Returns true if an Item with Name exists in list.
        /// </summary>
        bool Contains(string Name);
        /// <summary>
        /// Finds an Item by Name, if any and if T has a Name property. Else returns null.
        /// </summary>
        T Find(string Name);

        /// <summary>
        /// String indexer.
        /// </summary>
        T this[string Name] { get; }
    }

}
