namespace Tripous.Identity
{
    /// <summary>
    /// User account in an external identity provider, e.g. Facebook, Google
    /// </summary>
    public class UserLogin : IdentityEntity
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public UserLogin()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public UserLogin(DataRow Row)
            : base(Row)
        {
        }

        /* public */
        /// <summary>
        /// Returns a string representation of this instance
        /// </summary>
        public override string ToString()
        {
            return Provider;
        }

        /* properties */
        /// <summary>
        /// The master Id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// The external identity provider, e.g. Facebook, Google etc.
        /// </summary>
        public string Provider { get; set; }
        /// <summary>
        /// The provider key. It uniquelly identifies a user in the external identity provider.
        /// </summary>
        public string ProviderKey { get; set; }
        /// <summary>
        /// The provider display name
        /// </summary>
        public string ProviderDisplayName { get; set; }
 
    }
 
}
