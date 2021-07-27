using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;

namespace Tripous.Data
{
    /// <summary>
    /// Describes a database table field
    /// </summary>
    public class PropDescriptor
    { 

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public PropDescriptor(EntityDescriptor Table, PropertyInfo Property, PropAttribute Source)
        {
            this.Table = Table;
            this.Property = Property;
            this.PropertyType = Property.PropertyType;
            this.PropertyName = Property.Name;
            this.FieldName = Source.Name;
            this.FieldType = Source.TypeName;
            this.Nullable = Source.Nullable;            
            this.Length = Source.Length;
            this.Description = Source.Description;
            this.MasterEntityType = Source.MasterEntityType;
 
            this.TrimRequired = "char".IsSameText(this.FieldType);
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public PropDescriptor(EntityDescriptor Table, string PropertyName, string FieldName, bool Nullable, string TypeName, int Length = 0, string Description = "")
        {
            this.Table = Table;
            this.PropertyName = PropertyName;
            this.Property = Table.EntityType.GetProperty(PropertyName);
            this.PropertyType = Property.PropertyType;
            this.FieldName = FieldName;
            this.FieldType = TypeName;
            this.Nullable = Nullable;            
            this.Length = Length;
            this.Description = Description;
 
            this.TrimRequired = "char".IsSameText(this.FieldType);
        }


        /* public */
        /// <summary>
        /// Override. Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return $"{PropertyName}:{FieldName}";
        }


        /// <summary>
        /// Gets the value of the property from a specified entity
        /// </summary>
        public object GetValue(object Entity)
        {
            return Property.GetValue(Entity);
        }
        /// <summary>
        /// Sets the value of the property in a specified entity
        /// </summary>
        public void SetValue(object Entity, object Value)
        {
            Property.SetValue(Entity, Value);
        }

        /// <summary>
        /// Returns true if this field is one of the primary keys of the table.
        /// </summary>
        public bool IsPrimaryKey()
        {
            return this.Table == null ? false : this.Table.IsPrimaryKey(this);
        }

        /* properties */
        /// <summary>
        /// The owner table
        /// </summary>
        public EntityDescriptor Table { get; private set; }
        /// <summary>
        /// Returns the reflection Property instance
        /// </summary>
        public PropertyInfo Property { get; set; }
        /// <summary>
        /// The property type
        /// </summary>
        public Type PropertyType { get; set; }
        /// <summary>
        /// The property name in the Entity this field represents.
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// The field name in the database table
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// The type of the field in the database table, e.g. char, varchar, int, etc.
        /// </summary>
        public string FieldType { get; set; }
        /// <summary>
        /// True when the field is nullable.
        /// </summary>
        public bool Nullable { get; set; }
        /// <summary>
        /// The maximum length of a string field.
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// True when is a char field which requires trimming of ending spaces
        /// </summary>
        public bool TrimRequired { get; set; }
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
