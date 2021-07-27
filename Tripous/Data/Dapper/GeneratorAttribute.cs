using System;
using System.Collections.Generic;
using System.Text;

namespace Tripous.Data
{
    /// <summary>
    /// Marks a static method that is going to be used as a generator function.
    /// <para>NOTE: A generator function is responsible in creating and returning primary key values for databases tables.</para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class GeneratorAttribute: Attribute
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public GeneratorAttribute(Type EntityType)
        {
            this.EntityType = EntityType;
        }

        /// <summary>
        /// The entity type for which this generator function can create primary key values.
        /// </summary>
        public Type EntityType { get; private set; }
    }
}
