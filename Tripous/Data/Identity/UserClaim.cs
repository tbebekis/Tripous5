namespace Tripous.Identity
{
    /// <summary>
    /// A claim that belongs to a <see cref="User"/>
    /// </summary>
    public class UserClaim : BaseClaim
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public UserClaim()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public UserClaim(DataRow Row)
            : base(Row)
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public UserClaim(Claim Source)
            : base(Source)
        { 
        }


        /* properties */
        /// <summary>
        /// The master Id
        /// </summary>
        public string UserId { get; set; }
 
    }
}
