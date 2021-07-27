using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;

using Dapper;

namespace Tripous.Data
{

    /// <summary>
    /// Describes a non-list property which is a reference to another Entity.
    ///  <para>When an Entity contains a foreign key property/field, e.g. Customer.CountryId, this attribute is used in marking the backing reference property, Customer.Country.</para>
    /// </summary>
    public class RelationalDescriptor
    {
        EntityDescriptor fForeignTableDescriptor;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="RelationalProperty">A non-list property which is a reference to another Entity, e.g. Customer.Country. </param>
        /// <param name="KeyField">A foreign key property/field, e.g. Customer.CountryId, to be used when SELECTing the property value.</param>
        public RelationalDescriptor(PropertyInfo RelationalProperty, PropDescriptor KeyField)
        {
            this.RelationalProperty = RelationalProperty;
            this.KeyField = KeyField;
        }

        /// <summary>
        /// A non-list property which is a reference to another Entity, e.g. Customer.Country.
        /// </summary>
        public PropertyInfo RelationalProperty { get; }
        /// <summary>
        /// A foreign key property/field, e.g. Customer.CountryId, to be used when SELECTing the property value.
        /// </summary>
        public PropDescriptor KeyField { get; }
        /// <summary>
        /// Gets the TableDescriptor of the type the relational property represents.
        /// </summary>
        public EntityDescriptor ForeignTableDescriptor
        {
            get
            {
                if (fForeignTableDescriptor == null)
                {
                    fForeignTableDescriptor = EntityDescriptors.Find(RelationalProperty.PropertyType);
                }

                return fForeignTableDescriptor;
            }
        }
    }


}
