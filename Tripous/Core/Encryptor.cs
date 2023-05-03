/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;

namespace Tripous
{


    /// <summary>
    /// Encrypts a string Value
    /// <para>SEE: https://stackoverflow.com/questions/10168240/encrypting-decrypting-a-string-in-c-sharp </para>
    /// <para>SEE: https://github.com/nopara73/DotNetEssentials/blob/master/DotNetEssentials/Crypto/StringCipher.cs </para>
    /// </summary>
    static public class Encryptor
    {
        // This constant is used to determine the keysize of the encryption algorithm in bits.
        // We divide this by 8 within the code below to get the equivalent number of bytes.
        const int Keysize = 128;
        /// <summary>
        /// This constant determines the number of iterations for the password bytes generation function.
        /// </summary>
        const int DerivationIterations = 1000;

        /* private */
        static string fDefaultEncryptionKey;
        /// <summary>
        /// Generate128BitsOfRandomEntropy
        /// </summary>
        static byte[] Generate128BitsOfRandomEntropy()
        {
            var randomBytes = new byte[16]; // 16 Bytes will give us 128 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }

        /* public */
        /// <summary>
        /// Encrypts a string and returns the result.
        /// <para>EXAMPLE: Encrypt(Passphrase, TextToEncrypt) </para>
        /// </summary>
        static public string Encrypt(string TextToEncrypt, string EncryptionKey = "")
        {
            if (string.IsNullOrWhiteSpace(EncryptionKey))
                EncryptionKey = DefaultEncryptionKey;

            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            var saltStringBytes = Generate128BitsOfRandomEntropy();
            var ivStringBytes = Generate128BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(TextToEncrypt);
            using (var password = new Rfc2898DeriveBytes(EncryptionKey, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 128;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Decrypts a string and returns the result.
        /// <para>EXAMPLE: Decrypt(Passphrase, TextToDecrypt) </para>
        /// </summary>
        static public string Decrypt(string TextToDecrypt, string EncryptionKey = "")
        {
            if (string.IsNullOrWhiteSpace(EncryptionKey))
                EncryptionKey = DefaultEncryptionKey;

            // Get the complete stream of bytes that represent:
            // [32 bytes of Salt] + [16 bytes of IV] + [n bytes of CipherText]
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(TextToDecrypt);
            // Get the saltbytes by extracting the first 16 bytes from the supplied cipher Text bytes.
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            // Get the IV bytes by extracting the next 16 bytes from the supplied cipher Text bytes.
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Get the actual cipher text bytes by removing the first 64 bytes from the cipher Text string.
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            using (var password = new Rfc2898DeriveBytes(EncryptionKey, saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 128;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream(cipherTextBytes))
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                var plainTextBytes = new byte[cipherTextBytes.Length];
                                var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                            }
                        }
                    }
                }
            }
        }

        /* properties */
        /// <summary>
        /// The encryption key used by Encrypt()/Decrypt() when no encryption key is passed to those calls.
        /// </summary>
        static public string DefaultEncryptionKey
        {
            get { return !string.IsNullOrWhiteSpace(fDefaultEncryptionKey) ? fDefaultEncryptionKey : "8fDL@sHv#p0re-F0"; }
            set { fDefaultEncryptionKey = value; }
        }


        /// <summary>
        /// Computes the hash value of a specified Text using a specified hash algorithm
        /// </summary>
        static public string ComputeHash(HashAlgorithmType Type, string Text)
        {
            byte[] Buffer = Encoding.UTF8.GetBytes(Text);
            Buffer = ComputeHash(Type, Buffer);
            string Result = BitConverter.ToString(Buffer).Replace("-", "");
            return Result;
        }
        /// <summary>
        /// Computes the hash value of a specified byte array using a specified hash algorithm
        /// </summary>
        static public byte[] ComputeHash(HashAlgorithmType Type, byte[] Buffer)
        {
            switch (Type)
            {
                case HashAlgorithmType.Sha1: return SHA1.HashData(Buffer);
                case HashAlgorithmType.Sha256: return SHA256.HashData(Buffer);
                case HashAlgorithmType.Sha384: return SHA384.HashData(Buffer);
                case HashAlgorithmType.Sha512: return SHA512.HashData(Buffer);
                case HashAlgorithmType.Md5: return MD5.HashData(Buffer);
            }

            return Buffer;

        }

    }
}
 
