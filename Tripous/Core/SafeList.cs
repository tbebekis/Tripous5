namespace Tripous
{

    /// <summary>
    /// A thread-safe generic list
    /// </summary>
    public class SafeList<T>
    {
        object syncLock = new LockObject();
        List<T> list = new List<T>();

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public SafeList()
        {
        }

        /* public */
        /// <summary>
        /// Removes and returns the object at the beginning (top) of the list
        /// </summary>
        public T Pop()
        {
            lock (syncLock)
            {
                T Result = default(T);

                if (list.Count > 0)
                {
                    Result = list[0];
                    list.RemoveAt(0);
                }

                return Result;
            }
        }
        /// <summary>
        /// Adds item to list
        /// </summary>
        public void Add(T item)
        {
            lock (syncLock)
                list.Add(item);
        }
        /// <summary>
        /// Adds collection to list
        /// </summary>
        public void AddRange(IEnumerable<T> collection)
        {
            lock (syncLock)
                list.AddRange(collection);
        }
        /// <summary>
        /// Removes all items from list
        /// </summary>
        public void Clear()
        {
            lock (syncLock)
                list.Clear();
        }
        /// <summary>
        /// True if item in list
        /// </summary>
        public bool Contains(T item)
        {
            lock (syncLock)
                return list.Contains(item);
        }
        /// <summary>
        /// Copies list content to array
        /// </summary>
        public void CopyTo(T[] array)
        {
            lock (syncLock)
                list.CopyTo(array);
        }
        /// <summary>
        /// Determines whether the list contains elements
        /// that match the conditions defined by the specified predicate.
        /// </summary>
        public bool Exists(Predicate<T> match)
        {
            lock (syncLock)
                return list.Exists(match);
        }
        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified
        /// predicate, and returns the first occurrence within the entire list
        /// </summary>
        public T Find(Predicate<T> match)
        {
            lock (syncLock)
                return list.Find(match);
        }
        /// <summary>
        /// Returns the first element of the sequence that satisfies a condition or a default value if no such element is found.
        /// </summary>
        public T FirstOrDefault(Func<T, bool> predicate)
        {
            lock (syncLock)
                return list.FirstOrDefault(predicate);
        }
        /// <summary>
        /// Retrieves all the elements that match the conditions defined by the specified
        /// predicate.
        /// </summary>
        public List<T> FindAll(Predicate<T> match)
        {
            lock (syncLock)
                return list.FindAll(match);
        }
        /// <summary>
        /// Searches for an element that matches the conditions defined by the specified
        /// predicate, and returns the zero-based index of the first occurrence within
        /// the entire list
        /// </summary>
        public int FindIndex(Predicate<T> match)
        {
            lock (syncLock)
                return list.FindIndex(match);
        }
        /// <summary>
        /// Performs the specified action on each element of the list
        /// </summary>
        public void ForEach(Action<T> action)
        {
            lock (syncLock)
                list.ForEach(action);
        }
        /// <summary>
        /// Returns the index of item
        /// </summary>
        public int IndexOf(T item)
        {
            lock (syncLock)
                return list.IndexOf(item);
        }
        /// <summary>
        /// Inserts item at index
        /// </summary>
        public void Insert(int index, T item)
        {
            lock (syncLock)
                list.Insert(index, item);
        }
        /// <summary>
        /// Removes item from list
        /// </summary>
        public bool Remove(T item)
        {
            lock (syncLock)
                return list.Remove(item);
        }
        /// <summary>
        /// Removes all the elements that match the conditions defined by the specified
        /// predicate.
        /// </summary>
        public int RemoveAll(Predicate<T> match)
        {
            lock (syncLock)
                return list.RemoveAll(match);
        }
        /// <summary>
        /// Removes item at index
        /// </summary>
        public void RemoveAt(int index)
        {
            lock (syncLock)
                list.RemoveAt(index);
        }
        /// <summary>
        /// Sorts the elements in the entire list using
        /// the default comparer.
        /// </summary>
        public void Sort()
        {
            lock (syncLock)
                list.Sort();
        }
        /// <summary>
        /// Sorts the elements in the entire list using
        /// the specified comparison function
        /// </summary>
        public void Sort(Comparison<T> comparison)
        {
            lock (syncLock)
                list.Sort(comparison);
        }
        /// <summary>
        /// Sorts the elements in the entire list using
        /// the specified comparer
        /// </summary>
        public void Sort(IComparer<T> comparer)
        {
            lock (syncLock)
                list.Sort(comparer);
        }
        /// <summary>
        /// Returns all elements in the list as an array
        /// </summary>
        public T[] ToArray()
        {
            lock (syncLock)
                return list.ToArray();
        }
        /// <summary>
        /// Determines whether every element in the list
        /// matches the conditions defined by the specified predicate.
        /// </summary>
        public bool TrueForAll(Predicate<T> match)
        {
            lock (syncLock)
                return list.TrueForAll(match);
        }


        /* properties */
        /// <summary>
        /// Gets the number of elements actually contained in the list
        /// </summary>
        public int Count
        {
            get
            {
                lock (syncLock)
                    return list.Count;
            }
        }
        /// <summary>
        /// True if list is empty
        /// </summary>
        public bool IsEmpty
        {
            get
            {
                lock (syncLock)
                    return list.Count == 0;
            }

        }
        /// <summary>
        /// Gets the lock object
        /// </summary>
        public object SyncLock { get { return syncLock; } }
    }
}
