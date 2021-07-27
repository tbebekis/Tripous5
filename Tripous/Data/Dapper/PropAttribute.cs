using System;
using System.Collections.Generic;
using System.Text;

namespace Tripous.Data
{


    /// <summary>
    /// Marks a property of an entity class. Used in mapping properties to database table fields.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public class PropAttribute: Attribute
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public PropAttribute()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="Name">The field name in the database table</param>
        /// <param name="Nullable">True when the field is nullable.</param>
        /// <param name="TypeName">The type of the field in the database table, e.g. char, varchar, int, etc.</param>
        /// <param name="Length">The maximum length of a string field.</param>
        /// <param name="Description">The description of the field.</param>
        public PropAttribute(string Name, bool Nullable, string TypeName, int Length = 0, string Description = "")
        {
            this.Name = Name;
            this.Nullable = Nullable;
            this.TypeName = TypeName;            
            this.Length = Length;
            this.Description = Description;
        }


        /// <summary>
        /// Override. Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return !string.IsNullOrWhiteSpace(Name)? Name: base.ToString();
        }

        /// <summary>
        /// The field name in the database table
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// True when the field is nullable.
        /// </summary>
        public bool Nullable { get; set; }
        /// <summary>
        /// The type of the field in the database table, e.g. char, varchar, int, etc.
        /// </summary>
        public string TypeName { get; set; }
        /// <summary>
        /// The maximum length of a string field.
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// The description of the field.
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Not null when this is a foreign key field/property. Denotes the Type of the master (foreign) entity.
        /// </summary>
        public Type MasterEntityType { get; set; }

    }


}
