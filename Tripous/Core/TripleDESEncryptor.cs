namespace Tripous
{
    /// <summary>
    /// A string encryption class that uses the <see cref="TripleDES"/> algorithm.
    /// <para>From: https://stackoverflow.com/questions/11413576/how-to-implement-triple-des-in-c-sharp-complete-example</para>
    /// </summary>
    static public class TripleDESEncryptor
    {

        /// <summary>
        /// Creates and returns a <see cref="TripleDES"/> instance using a specified encryption key
        /// </summary>
        static TripleDES CreateTripleDES(string EncryptionKey)
        {
            MD5 MD5 = new MD5CryptoServiceProvider();
            TripleDES TripleDES = new TripleDESCryptoServiceProvider();
            byte[] HashCodeBuffer = MD5.ComputeHash(Encoding.UTF8.GetBytes(EncryptionKey));
            TripleDES.Key = HashCodeBuffer;
            TripleDES.IV = new byte[TripleDES.BlockSize / 8];
            TripleDES.Padding = PaddingMode.PKCS7;
            TripleDES.Mode = CipherMode.ECB;
            return TripleDES;
        }
        /// <summary>
        /// Encrypts a specified plain text using  a specified encryption key 
        /// </summary>
        static public string Encrypt(string PlainText, string EncryptionKey)
        {
            TripleDES TripleDES = CreateTripleDES(EncryptionKey);
            ICryptoTransform Encryptor = TripleDES.CreateEncryptor();
            byte[] InputBuffer = Encoding.UTF8.GetBytes(PlainText);
            byte[] OutputBuffer = Encryptor.TransformFinalBlock(InputBuffer, 0, InputBuffer.Length);
            return Convert.ToBase64String(OutputBuffer);
        }
        /// <summary>
        /// Decrypts a specified encrypted text using a specified encryption key 
        /// </summary>
        static public string Decrypt(string CypherText, string EncryptionKey)
        {
            TripleDES TripleDES = CreateTripleDES(EncryptionKey);
            ICryptoTransform Decryptor = TripleDES.CreateDecryptor();
            byte[] InputBuffer = Convert.FromBase64String(CypherText);
            byte[] OutputBuffer = Decryptor.TransformFinalBlock(InputBuffer, 0, InputBuffer.Length);
            return Encoding.UTF8.GetString(OutputBuffer);
        } 

    }
}
