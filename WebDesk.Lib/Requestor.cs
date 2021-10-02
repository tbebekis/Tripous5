using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

using Tripous;

namespace WebLib
{

    /// <summary>
    /// Represents the user or api client of a request
    /// </summary>
    public class Requestor: IClaimListProvider
    {
        /* constants */
        /// <summary>
        /// A claim type for a private claim. 
        /// Designates the level of a user, i.e. Admin, User, Guest, Service, etc.
        /// </summary>
        public const string SUserLevelClaimType = "UserLevel";
        /// <summary>
        /// A claim type for a private claim.
        /// Designates the scheme used in authentication, i.e. Cookies, JWT, etc.
        /// </summary>
        public const string SAuthenticationSchemeClaimType = "AuthenticationScheme";
        /// <summary>
        /// A claim type for a private claim. 
        /// Designates the culture code to be used for subsequent calls, e.g. en-US
        /// </summary>
        public const string SCultureClaimType = "CultureCode";

        /* public */
        /// <summary>
        /// Creates and returns a claim list regarding this instance
        /// </summary>
        public List<Claim> GetClaimList(string AuthenticationScheme)
        {
            if (string.IsNullOrWhiteSpace(Id))
                throw new ApplicationException("Cannot produce claims. No Id");

            if (string.IsNullOrWhiteSpace(UserId))
                throw new ApplicationException("Cannot produce claims. No UserId");
 
            List<Claim> ClaimList = new List<Claim>();

            ClaimList.Add(new Claim(ClaimTypes.NameIdentifier, Id));
            ClaimList.Add(new Claim(ClaimTypes.Name, !string.IsNullOrWhiteSpace(Name) ? Name : "no name"));
            ClaimList.Add(new Claim(ClaimTypes.Email, !string.IsNullOrWhiteSpace(Email) ? Email : "no email"));

            // private claims
            ClaimList.Add(new Claim(SUserLevelClaimType, Level.ToString()));
            ClaimList.Add(new Claim(SAuthenticationSchemeClaimType, AuthenticationScheme));

            return ClaimList;
        }

        /* properties */
        /// <summary>
        /// Required. Database Id
        /// </summary>
        public string Id { get; set; } = "";
        /// <summary>
        /// The level of a user, i.e. Guest, Admin, User, etc.
        /// </summary>
        public UserLevel Level { get; set; }
        /// <summary>
        /// Required. Email or User name or something
        /// </summary> 
        public string UserId { get; set; }
        /// <summary>
        /// Optional. The requestor name
        /// </summary> 
        public string Name { get; set; }
        /// <summary>
        /// Optional. The requestor email
        /// </summary> 
        public string Email { get; set; }
        /// <summary>
        /// True when requestor is blocked by admins
        /// </summary>
        public bool IsBlocked { get; set; }
    }
}
