namespace Tripous
{

    /// <summary>
    /// Encrypts and decrypts a string value.
    /// <para>Uses the <see cref="Aes"/> a symmetric block cipher algorithm.</para>
    /// <para>Symmetric means that encrypted data can be decrypted.</para>
    /// <para>NOTE: adapted from https://code-maze.com/csharp-string-encryption-decryption/ </para>
    /// </summary>
    static public class Encryptor
    {

        static byte[] IV =
        {
            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08,
            0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
        }; 

        static byte[] DeriveKeyFromPassword(string password)
        {
            var emptySalt = Array.Empty<byte>();
            var iterations = 1000;
            var desiredKeyLength = 16; // 16 bytes equal 128 bits.
            var hashMethod = HashAlgorithmName.SHA384;
            return Rfc2898DeriveBytes.Pbkdf2(Encoding.Unicode.GetBytes(password),
                                             emptySalt,
                                             iterations,
                                             hashMethod,
                                             desiredKeyLength);
        }

        /// <summary>
        /// Encrypts a string and returns the result.
        /// <para>NOTE: The specified passphrase used in decryption should be the one used for encryption. </para>
        /// </summary>
        static public string Encrypt(string PlainTextPassword, string Passphrase)
        {
            if (string.IsNullOrWhiteSpace(PlainTextPassword))
                throw new ArgumentNullException(PlainTextPassword);

            if (string.IsNullOrWhiteSpace(Passphrase))
                throw new ArgumentNullException(Passphrase);

            using (Aes aes = Aes.Create())
            {
                aes.Key = DeriveKeyFromPassword(Passphrase);
                aes.IV = IV;

                using (MemoryStream OutMS = new MemoryStream())
                {
                    using (CryptoStream CryptoStream = new(OutMS, aes.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        CryptoStream.Write(Encoding.Unicode.GetBytes(PlainTextPassword));
                        CryptoStream.FlushFinalBlock();
                    }

                    return Convert.ToBase64String(OutMS.ToArray());
                }
            }
        }
        /// <summary>
        /// Decrypts a string and returns the result.
        /// <para>NOTE: The specified passphrase used in decryption should be the one used for encryption. </para>
        /// </summary>
        static public string Decrypt(string EncryptedPassword, string Passphrase)
        {
            if (string.IsNullOrWhiteSpace(EncryptedPassword))
                throw new ArgumentNullException(EncryptedPassword);

            if (string.IsNullOrWhiteSpace(Passphrase))
                throw new ArgumentNullException(Passphrase);

            byte[] EncryptedBuffer = Convert.FromBase64String(EncryptedPassword);

            using (Aes aes = Aes.Create())
            {
                aes.Key = DeriveKeyFromPassword(Passphrase);
                aes.IV = IV;

                using (MemoryStream InMS = new(EncryptedBuffer))
                {
                    using (CryptoStream CryptoStream = new(InMS, aes.CreateDecryptor(), CryptoStreamMode.Read))
                    {
                        using MemoryStream OutMS = new();
                        CryptoStream.CopyTo(OutMS);

                        byte[] Buffer = OutMS.ToArray();

                        return Encoding.Unicode.GetString(Buffer);
                    }
                }
            }
        }

        /// <summary>
        /// Encrypts a string and returns the result.
        /// </summary>
        static public string Encrypt(string PlainTextPassword)
        {
            string Passphrase = DefaultPassphrase;
            return Encrypt(PlainTextPassword, Passphrase);
        }
        /// <summary>
        /// Decrypts a string and returns the result.
        /// </summary>
        static public string Decrypt(string EncryptedPassword)
        {
            string Passphrase = DefaultPassphrase;
            return Decrypt(EncryptedPassword, Passphrase);
        }

        /// <summary>
        /// The default passphrase to be used when encrypting/decrypting without specifying a passphrase.
        /// </summary>
        static public string DefaultPassphrase { get; set; } = SysConfig.DefaultPassphrase; // @"8fDL@sHv#p0re-F0";
    }

}
 
