using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Tripous
{
    /// <summary>
    /// Password hasher. It uses the SHA256 algorithm.
    /// </summary>
    static public class SHA256Hasher
    {
        /// <summary>
        /// Creates and returns a salt with length between 4 and 8 bytes.
        /// </summary>
        static public string GenerateSalt()
        {
            int MinSaltSize = 4;
            int MaxSaltSize = 8;

            Random Random = new Random();
            int SaltSize = Random.Next(MinSaltSize, MaxSaltSize);
            byte[] SaltBuffer = new byte[SaltSize];

            RNGCryptoServiceProvider RandomNumberGenerator = new RNGCryptoServiceProvider();
            RandomNumberGenerator.GetNonZeroBytes(SaltBuffer);
            return Convert.ToBase64String(SaltBuffer);
        }
        /// <summary>
        /// Hashes a specified password given in plain text, using a specified salt and returns the hashed password
        /// </summary>
        static public string Hash(string PlainTextPassword, string Salt)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(PlainTextPassword + Salt);
            HashAlgorithm hash = new SHA256Managed();
            byte[] hashBytes = hash.ComputeHash(plainTextBytes);
            return Convert.ToBase64String(hashBytes);
        }
        /// <summary>
        /// Returns true if a hashed password and a password given in plain text along with a salt, are identical.
        /// </summary>
        static public bool Verify(string PlainTextPassword, string Salt,  string HashedPassword)
        {
            string ExpectedHashedPassword = Hash(Salt, PlainTextPassword);
            return HashedPassword == ExpectedHashedPassword;
        }

    }
}
