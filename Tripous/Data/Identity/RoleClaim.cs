namespace Tripous.Identity
{
 
    /// <summary>
    /// A claim that belongs to a <see cref="Role"/>
    /// </summary>
    public class RoleClaim : BaseClaim
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public RoleClaim()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public RoleClaim(DataRow Row)
            : base(Row)
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public RoleClaim(Claim Source)
            : base(Source)
        {
        }

        /* properties */
        /// <summary>
        /// The master Id
        /// </summary>
        public string RoleId { get; set; }
    }
}
