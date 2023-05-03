using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tripous;
using Tripous.Data;
 
using System.Security.Cryptography;
using System.Security.Authentication;

namespace WebLib
{

    /// <summary>
    /// Represents the database
    /// </summary>
    static public partial class DataStore
    {
        /// <summary>
        /// Generates and returns a random text of a specified size.
        /// <para>Use it to generate Salt Keys for hashing passwords.</para>
        /// </summary>
        static string GenerateRandomText(int Length)
        {
            string Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder SB = new StringBuilder();

            int Index;
            for (int i = 0; i < Length; i++)
            {
                Index = RandomNumberGenerator.GetInt32(Chars.Length);
                SB.Append(Chars[Index]);
            }

            return SB.ToString();
        }
        /// <summary>
        /// Generates and returns a hash of a Password specified in clear text, using the SHA1 algorithm. 
        /// <para>It appends a specified SaltKey to the password first.</para>
        /// </summary>
        static string GeneratePasswordHash(string PlainTextPassword, string PasswordSalt, HashAlgorithmType Type = HashAlgorithmType.Sha1)
        {
            string S = string.Concat(PlainTextPassword, PasswordSalt);
            //return ComputeHash(Encoding.UTF8.GetBytes(S), AlgorithmName);
            return Encryptor.ComputeHash(Type, S);
        }
        /*
        /// <summary>
        /// Computes the hash value of a specifed byte array
        /// </summary>
        static string ComputeHash(byte[] Buffer, string AlgorithmName = "SHA1")
        {
            if (string.IsNullOrWhiteSpace(AlgorithmName))
                AlgorithmName = "SHA1";

            HashAlgorithm Algorithm = HashAlgorithm.Create(AlgorithmName);
            if (Algorithm == null)
                throw new ArgumentException("Wrong Hash Algorith Name");

            Buffer = Algorithm.ComputeHash(Buffer);
            return BitConverter.ToString(Buffer).Replace("-", "");
        }
        */
        /// <summary>
        /// Validates the password of a user/requestor
        /// </summary>
        static bool ValidatePassword(string PlainTextPassword, string PasswordSalt, string EncryptedPassword)
        {
            if (string.IsNullOrWhiteSpace(PlainTextPassword) || string.IsNullOrWhiteSpace(PasswordSalt))
                return false;

            var Settings = GetSettings();
            string SuperUserPassword = Settings.General.SuperUserPassword;
            if (!string.IsNullOrWhiteSpace(SuperUserPassword) && (PlainTextPassword == SuperUserPassword))
                return true;

            string S = GeneratePasswordHash(PlainTextPassword, PasswordSalt);
            return S == EncryptedPassword;
        }
    
        static Command[] GetMainMenuDemo()
        {
            List<Command> Result = new List<Command>();

            Command BarItem = new Command() { TitleKey = "File Kai Ase Re File" };
            Result.Add(BarItem);
            BarItem.AddUi("New");
            BarItem.AddUi("Open");
            BarItem.AddUi("Exit");

            BarItem = new Command() { TitleKey = "Edit kai Edit" };
            Result.Add(BarItem);
            BarItem.AddUi("Cut");
            BarItem.AddUi("Copy");
            BarItem.AddUi("Paste");
            BarItem.AddUi("Delete");

            BarItem = new Command() { TitleKey = "View Ki Apanw Tourla" };
            Result.Add(BarItem);
            BarItem.AddUi("Document");
            BarItem.AddUi("Image");
            BarItem.AddUi("Table");
            BarItem.AddUi("Window");

            BarItem = new Command() { TitleKey = "Project Apisteyto" };
            Result.Add(BarItem);
            BarItem.AddUi("Run");
            BarItem.AddUi("Debug");

            BarItem = new Command() { TitleKey = "Extensions" };
            Result.Add(BarItem);
            BarItem.AddUi("One");
            BarItem.AddUi("Two");
            BarItem.AddUi("Three");

 

            return Result.ToArray();
        }
    }
}
