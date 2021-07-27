using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tripous.Data
{


    /// <summary>
    /// Info about a Relational property
    /// </summary>
    public class RelationalInfo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public RelationalInfo()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public RelationalInfo(RelationalDescriptor Descriptor)
        {
            this.RelationalProperty = Descriptor.RelationalProperty.Name;
            this.KeyProperty = Descriptor.KeyField.PropertyName;
        }

        /// <summary>
        /// A non-list property which is a reference to another Entity, e.g. Customer.Country.
        /// </summary>
        public string RelationalProperty { get; set; }
        /// <summary>
        /// A foreign key property/field, e.g. Customer.CountryId, to be used when SELECTing the property value.
        /// </summary>
        public string KeyProperty { get; set; }
    }


}
