/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
 

namespace Tripous
{

    /// <summary>
    /// EventArgs for sending notification about changes happened to the property of
    /// an object or any of its items.
    /// </summary>
    public class ChangedEventArgs : EventArgs
    {
        private int index = -1;
        private object item;
        private string propertyName;


        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        public ChangedEventArgs(object Item)
        {
            item = Item;
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        public ChangedEventArgs(object Item, int Index)
        {
            item = Item;
            index = Index;
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        public ChangedEventArgs(string PropertyName)
        {
            propertyName = PropertyName;
        }

        /* properties */
        /// <summary>
        /// Gets the Item that changed. It could be the sender itself though.
        /// </summary>
        public object Item { get { return item; } }
        /// <summary>
        /// Returns the Index of the Item.
        /// </summary>
        public int Index { get { return index; } }
        /// <summary>
        /// Returns the name of the property which has changed.
        /// </summary>
        public string PropertyName { get { return !string.IsNullOrWhiteSpace(propertyName) ? propertyName : string.Empty; } }
    }



    /// <summary>
    /// EventArgs for sending notification about changes happened to the property of
    /// an object or any of its items.
    /// </summary>
    public class ChangedEventArgs<T> : EventArgs
    {
        private int index = -1;
        private T item;
        private string propertyName;

        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        public ChangedEventArgs(T Item)
        {
            item = Item;
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        public ChangedEventArgs(T Item, int Index)
        {
            item = Item;
            index = Index;
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        public ChangedEventArgs(string PropertyName)
        {
            propertyName = PropertyName;
        }

        /* properties */
        /// <summary>
        /// Returns the Item.
        /// </summary>
        public T Item { get { return item; } }
        /// <summary>
        /// Returns the Index of the Item.
        /// </summary>
        public int Index { get { return index; } }
        /// <summary>
        /// Returns the name of the property which has changed.
        /// </summary>
        public string PropertyName { get { return !string.IsNullOrWhiteSpace(propertyName) ? propertyName : string.Empty; } }
    }
}
