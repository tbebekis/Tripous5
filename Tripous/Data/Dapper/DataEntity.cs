namespace Tripous.Data
{

    
    /// <summary>
    /// The base data entity  
    /// </summary>
    public class DataEntity
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public DataEntity()
        {
        }

        /* public */
        /// <summary>
        /// A validity check to perform just before the entity is saved in the database.
        /// <para>Returns a StringBuilder with errors or an empty one.</para>
        /// </summary>
        public virtual StringBuilder BeforeSaveCheck(bool IsInsert)
        {
            StringBuilder Errors = new StringBuilder();
            EntityDescriptor Descriptor = EntityDescriptors.Find(this.GetType());
            if (Descriptor != null)
            {
                Descriptor.BeforeSaveCheck(this, Errors, IsInsert);
            }

            return Errors;
        }
    }
}
