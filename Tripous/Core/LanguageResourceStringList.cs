using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Tripous
{
 
    /// <summary>
    /// String resources of a language
    /// </summary>
    public class LanguageResourceStringList
    {
        class ResourceItem
        {
            public ResourceItem(string Key, string Value)
            {
                if (string.IsNullOrWhiteSpace(Key))
                    throw new ExceptionEx("String resource item Key can not be null or empty.");

                this.Key = Key.ToUpperInvariant();
                this.Value = Value;
            }

            public override string ToString()
            {
                return $"{Key}|{Value}";
            }

            public string Key { get; }
            public string Value { get; }
        }

        List<ResourceItem> Items = new List<ResourceItem>();

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public LanguageResourceStringList()
        {
        }

        /* public */
        /// <summary>
        /// Clears the internal resource list
        /// </summary>
        public void Clear()
        {
            Items.Clear();
        }        
        /// <summary>
        /// Clears the internal list and loads string resources from a specified source
        /// </summary>
        public void LoadFrom(Dictionary<string, string> Source)
        {
            Items.Clear();
            foreach (var Entry in Source)
                Items.Add(new ResourceItem(Entry.Key, Entry.Value));
        }
        /// <summary>
        /// Returns a resource string based on a key, if any, or null. 
        /// <para>If key is not found, it may return the key as the result.</para>
        /// <para>Key is case-insensitive.</para>
        /// </summary>
        public string Find(string Key, bool DefaultToKey = true)
        {
            string sKey = Key.ToUpperInvariant();
            ResourceItem Item = Items.FirstOrDefault(item => item.Key == sKey);
            return Item != null ? Item.Value : (DefaultToKey ? Key : null);
        }

        /* properties */
        /// <summary>
        /// True when the internal resource list is empty
        /// </summary>
        public bool IsEmpty { get { return Items.Count == 0; } }
    }
}
