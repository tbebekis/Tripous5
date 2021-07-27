using System;
using System.Collections.Generic;
using System.Text;

namespace Tripous.Data
{
    /// <summary>
    /// Marks a non-list property which is a reference to another Entity.
    /// <para>When an Entity contains a foreign key property/field, e.g. Customer.CountryId, 
    /// this attribute is used in marking the backing reference property, Customer.Country.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class RelationalAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RelationalAttribute()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="KeyPropertyName">The name of a foreign key property/field to be used when selecting the relational property, e.g. Customer.CountryId.</param>
        public RelationalAttribute(string KeyPropertyName)
        {
            this.KeyPropertyName = KeyPropertyName;
        }

        /// <summary>
        /// The name of a foreign key property/field to be used when selecting the relational property.
        /// </summary>
        public string KeyPropertyName { get; set; }

    }
}
