using System;
using System.Collections.Generic;
using System.Text;

namespace Tripous.Identity
{
    /// <summary>
    /// Password hasher. It uses the SHA256 algorithm.
    /// </summary>
    public class PasswordHasher
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public PasswordHasher()
        {
        }


        /* public */
        /// <summary>
        /// Creates and returns a salt with length between 4 and 8 bytes.
        /// </summary>
        public virtual string GenerateSalt()
        {
            return Hasher.GenerateSalt();
        }
        /// <summary>
        /// Hashes a specified password given in plain text, using a specified salt and returns the hashed password
        /// </summary>
        public virtual string Hash(string PlainTextPassword, string Salt)
        {
            return Hasher.Hash(PlainTextPassword, Salt);
        }
        /// <summary>
        /// Returns true if a hashed password and a password given in plain text along with a salt, are identical.
        /// </summary>
        public virtual bool Verify(string PlainTextPassword, string Salt, string HashedPassword)
        {
            return Hasher.Validate(PlainTextPassword, Salt, HashedPassword);
        }
    }
}
