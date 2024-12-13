namespace WebLib
{
    /// <summary>
    /// Represents this library
    /// </summary>
    static public partial class Lib
    {
        /// <summary>
        /// Constant. For required model string properties without a value.
        /// </summary>
        public const string EmptyValue = "-";
        /// <summary>
        /// Constant
        /// </summary>
        public const string PolicyAuthorizationDefault = "Default";
        /// <summary>
        /// Constant
        /// </summary>
        public const string PolicyAuthenticated = "Authenticated";
        /// <summary>
        /// Phone number validation regex
        /// </summary>
        public const string SPhoneRegex = @"^\+{0,1}[\d|\s]*$";


        /// <summary>
        /// The name of the JWT authentication scheme
        /// </summary>
        public const string JwtAuthScheme = JwtBearerDefaults.AuthenticationScheme;
        /// <summary>
        /// The name of the Cookie authentication scheme
        /// </summary>
        public const string CookieAuthScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    }
}
