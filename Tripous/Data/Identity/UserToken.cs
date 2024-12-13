namespace Tripous.Identity
{

    /// <summary>
    /// 
    /// </summary>
    public class UserToken : IdentityEntity
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public UserToken()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public UserToken(DataRow Row)
            : base(Row)
        {
        }

        /* public */
        /// <summary>
        /// Returns a string representation of this instance
        /// </summary>
        public override string ToString()
        {
            return $"{Provider}:{Name}";
        }

        /* properties */
        /// <summary>
        /// The master Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// The provider this token comes from.
        /// </summary>
        public string Provider { get; set; }
        /// <summary>
        /// Token name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// SEE: https://stackoverflow.com/questions/26033983/what-is-the-maximum-size-of-jwt-token
        /// </summary>
        public string Data { get; set; }
    }
}
