namespace Tripous.Identity
{

    /// <summary>
    /// Role
    /// </summary>
    public class Role : IdentityEntity
    {
        /* construction */
        /// <summary>
        /// Construction
        /// </summary>
        public Role()
        {
        }
        /// <summary>
        /// Construction
        /// </summary>
        public Role(DataRow Row)
            : base(Row)
        {
        }

        /* public */
        /// <summary>
        /// Returns a string representation of this instance
        /// </summary>
        public override string ToString()
        {
            return Name;
        }
        /// <summary>
        /// It is called just before this instance is save to the database
        /// </summary>
        public override void BeforeSave(bool IsInsert)
        {
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new ApplicationException("Role Name not defined");
            }

            if (string.IsNullOrWhiteSpace(NormalizedName))
            {
                NormalizedName = Name.Normalize().ToUpperInvariant();
                IdDb.CheckUniqueStringField(this.TableName, "NormalizedName", NormalizedName, this.Id);
            } 

        }

        /* properties */
        /// <summary>
        /// The name of a role
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The "normalized" name of a role
        /// </summary>
        public string NormalizedName { get; set; }
    }
}
