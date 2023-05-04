using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Linq;
using System.Data;
using System.Security.Cryptography;
using System.Security.Claims;

namespace Tripous.Identity
{
    /// <summary>
    /// Helper. Represents this library.
    /// </summary>
    static public class IdLib
    {
        static PasswordHasher fPasswordHasher;
  
        /// <summary>
        /// Creates an authenticated <see cref="ClaimsIdentity"/> identity and the <see cref="ClaimsPrincipal"/> principal and assigns the context's Principal property.
        /// <para>No claims are added to the identity.</para>
        /// <para>Returns the newly created identity for the client code to add any needed claims to it.</para>
        /// </summary>
        static public ClaimsPrincipal CreateAuthenticatedIdentity(string AuthenticationType)
        {
            // NOTE: setting the second parameter actually authenticates the identity (IsAuthenticated returns true)
            ClaimsIdentity Identity = new ClaimsIdentity(new Claim[] { }, AuthenticationType);
            ClaimsPrincipal Principal = new ClaimsPrincipal(Identity);
            return Principal;
        }

        /* properties */
        /// <summary>
        /// The object used in hashing passwords.
        /// </summary>
        static public PasswordHasher PasswordHasher
        {
            get 
            {
                if (fPasswordHasher == null)
                    fPasswordHasher = new PasswordHasher();
                return fPasswordHasher;             
            }
            set { fPasswordHasher = value; }
        }
    }
}
