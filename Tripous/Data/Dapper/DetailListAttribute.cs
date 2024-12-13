namespace Tripous.Data
{

    /// <summary>
    /// Marks a list property of a master table/entity class in order to denote a detail table/entity collection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DetailListAttribute : Attribute
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public DetailListAttribute()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public DetailListAttribute(Type DetailEntityType, string DetailKeyPropertyName)
        {
            this.DetailEntityType = DetailEntityType;
            this.DetailKeyPropertyName = DetailKeyPropertyName;
        }

        /// <summary>
        /// The type of the detail entity
        /// </summary>
        public Type DetailEntityType { get; set; }
        /// <summary>
        /// The name of property in the detail entity. That property/field, matches the master entity/table primary key field/property.
        /// </summary>
        public string DetailKeyPropertyName { get; set; }
    }

}
