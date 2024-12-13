namespace Tripous.Data
{
    /// <summary>
    /// Field information
    /// </summary>
    public class PropInfo
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public PropInfo()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public PropInfo(PropDescriptor Descriptor)
        {
            PropertyName = Descriptor.PropertyName;
            PropertyType = !Sys.IsNullable(Descriptor.PropertyType) ? Descriptor.PropertyType.Name : System.Nullable.GetUnderlyingType(Descriptor.PropertyType).Name + "?" ;  
            FieldName = Descriptor.FieldName;
            FieldType = Descriptor.FieldType;
            Nullable = Descriptor.Nullable;
            Length = Descriptor.Length;
            TrimRequired = Descriptor.TrimRequired;
            Description = Descriptor.Description;
        }

        /// <summary>
        /// Override. Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return $"{PropertyName}:{FieldName}";
        }

        /// <summary>
        /// The property name in the Entity this field represents.
        /// </summary>
        public string PropertyName { get; set; }
        /// <summary>
        /// The type name of the property.
        /// </summary>
        public string PropertyType { get; set; }
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
    }
}
