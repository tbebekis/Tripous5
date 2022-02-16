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
 
        Dictionary<string, string> Entries = new Dictionary<string, string>();

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
            Entries.Clear();
        }        
        /// <summary>
        /// Clears the internal list and loads string resources from a specified source
        /// </summary>
        public void LoadFrom(Dictionary<string, string> Source)
        {
            Entries.Clear();
            foreach (var Entry in Source)
                Entries[Entry.Key.ToLowerInvariant()] = Entry.Value;
        }
        /// <summary>
        /// Returns a resource string based on a key, if any, or null. 
        /// <para>If key is not found, it may return the key as the result.</para>
        /// <para>Key is case-insensitive.</para>
        /// </summary>
        public string Find(string Key, bool DefaultToKey = true)
        {
            string sKey = Key.ToLowerInvariant();
            if (Entries.ContainsKey(sKey))
            {
                var Value = Entries[sKey];
                return !string.IsNullOrWhiteSpace(Value) ? Value : (DefaultToKey ? Key : null);
            }

            return DefaultToKey ? Key : null;
        }

        /// <summary>
        /// Returns the internal dictionary with resource strings.
        /// </summary>
        public Dictionary<string, string> GetResourceStringListDictionary()
        {
            return Entries;
        }

        /* properties */
        /// <summary>
        /// True when the internal resource list is empty
        /// </summary>
        public bool IsEmpty { get { return Entries.Count == 0; } }
    }
}
