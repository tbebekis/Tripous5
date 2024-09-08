using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous
{
    /// <summary>
    /// Generates passwords and validates passwords
    /// </summary>
    static public class Passwords
    {
        /// <summary>
        /// LowerChars
        /// </summary>
        static public readonly string LowerChars = @"abcdefghijklmnopqrstuvwxyz";
        /// <summary>
        /// UpperChars
        /// </summary>
        static public readonly string UpperChars = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        /// <summary>
        /// DigitChars
        /// </summary>
        static public readonly string DigitChars = @"0123456789";
        /// <summary>
        /// SpecialChars
        /// </summary>
        static public readonly string SpecialChars = @"~!@#$%^*()_-+=[{]};:>|./?";   // exclude & and < 


        /// <summary>
        /// Generates and returns a password of a specified length.
        /// </summary>
        static public string Generate(int Length = 10, bool UseLowerChars = true, bool UseUpperChars = true, bool UseDigitChars = true, bool UseSpecialChars = true)
        {
            StringBuilder sbResult = new StringBuilder();
            StringBuilder SB = new StringBuilder();

            List<string> List = new List<string>();
            if (UseLowerChars) SB.Append(LowerChars);
            if (UseUpperChars) SB.Append(UpperChars);

            Length = Length < 6 || Length > 64 ? 32 : Length;

            int Index;

            Random Random = new Random();
            while (sbResult.Length < Length)
            {
                Index = Random.Next(SB.Length - 1);
                sbResult.Append(SB[Index]);
            }

            int Index2 = -1;
            if (UseDigitChars)
            {
                Index = Random.Next(DigitChars.Length - 1);
                Index2 = Random.Next(sbResult.Length - 1);
                sbResult[Index2] = DigitChars[Index];
            }

            int Index3;
            if (UseSpecialChars)
            {
                Index = Random.Next(SpecialChars.Length - 1);
                Index3 = Random.Next(sbResult.Length - 1);
                while (Index3 == Index2)
                    Index3 = Random.Next(sbResult.Length - 1);
                sbResult[Index3] = SpecialChars[Index];
            }

            return sbResult.ToString();
        }

        /// <summary>
        /// Validates a password, according to specified requirements regarding its content and length, and returns the result.
        /// </summary>
        static public bool IsValid(string Password, int MinLength = 8, int MaxLength = 12, bool UseLowerChars = true, bool UseUpperChars = true, bool UseDigitChars = true, bool UseSpecialChars = true)
        {

            if (MinLength < 6)
                MinLength = 6;
            if (MaxLength < 8)
                MaxLength = 8;
            if (MaxLength < MinLength)
            {
                MinLength = 8;
                MaxLength = 12;
            }

            if (string.IsNullOrWhiteSpace(Password))
                return false;
            if (Password.Length < MinLength || Password.Length > MaxLength)
                return false;
            if (UseLowerChars && !Password.Any(char.IsLower))
                return false;
            if (UseUpperChars && !Password.Any(char.IsUpper))
                return false;
            if (UseDigitChars && !Password.Any(char.IsDigit))
                return false;
            if (UseSpecialChars && !Password.Any(SpecialChars.Contains))
                return false;

            return true;
        }
    }
}
