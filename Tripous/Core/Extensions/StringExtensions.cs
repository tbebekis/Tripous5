/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Tripous
{


    /// <summary>
    /// Extensions
    /// </summary>
    static public class StringExtensions
    {
        const string SValidEmailPattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|"
           + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)"
           + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

        static Regex ValidEmailRegex = new Regex(SValidEmailPattern, RegexOptions.IgnoreCase);

        /// <summary>
        /// Case insensitive string equality.
        /// <para>Returns true if 1. both are null, 2. both are empty string or 3. they are the same string </para>
        /// </summary>
        static public bool IsSameText(this string A, string B)
        {
            //return (!string.IsNullOrWhiteSpace(A) && !string.IsNullOrWhiteSpace(B))&& (string.Compare(A, B, StringComparison.InvariantCultureIgnoreCase) == 0);

            // Compare() returns true if 1. both are null, 2. both are empty string or 3. they are the same string
            return string.Compare(A, B, StringComparison.InvariantCultureIgnoreCase) == 0;
        }
        /// <summary>
        /// Case sensitive string equality.
        /// <para>Returns true if 1. both are null, 2. both are empty string or 3. they are the same string </para>
        /// </summary>
        public static bool IsSameStr(this string A, string B)
        {
            //return (!string.IsNullOrWhiteSpace(A) && !string.IsNullOrWhiteSpace(B)) && string.Compare(A, B, false, CultureInfo.InvariantCulture) == 0;

            // Compare() returns true if 1. both are null, 2. both are empty string or 3. they are the same string
            return string.Compare(A, B, false, CultureInfo.InvariantCulture) == 0;
        }
        /// <summary>
        /// Returns a string array that contains the substrings in this instance that are delimited by the Separator string.
        /// </summary>
        static public string[] Split(this string Text, string Separator)
        {
            if (string.IsNullOrEmpty(Text))
                return new string[0];

            return Text.Split(new string[] { Separator }, StringSplitOptions.RemoveEmptyEntries);             
        }
        /// <summary>
        /// Quotes S, that is returns S surrounded by ' (single quotes)
        /// </summary>
        static public string Quote(this string S)
        {
            StringBuilder SB = new StringBuilder(S);
            SB.Replace('\'', ' ');
            SB.Insert(0, "'");
            SB.Append('\'');

            return SB.ToString();
        }
        /// <summary>
        /// Quotes S, that is returns S surrounded by ' (single quotes)
        /// </summary>
        static public string QS(this string S)
        {
            return Quote(S);
        }
        /// <summary>
        /// Returns S removing any surrounding QuoteChar 
        /// </summary>
        static public string Dequote(this string S, char QuoteChar)
        {
            if (String.IsNullOrEmpty(S))
                return string.Empty;

            int length = S.Length;
            if (length > 1 && S[0] == QuoteChar && S[length - 1] == QuoteChar)
                S = S.Substring(1, length - 2);

            return S;
        }
        /// <summary>
        /// Trims S and if the last character is the comma character it removes it.
        /// </summary>
        static public string RemoveLastComma(this string S)
        {
            if (string.IsNullOrEmpty(S))
                return string.Empty;
            else
            {
                S = S.Trim();
                if ((S.Length > 0) && S.EndsWith(","))
                    S = S.Remove(S.Length - 1, 1);

                return S;
            }
        }
        /// <summary>
        /// Returns true if Value is contained in the Instance.
        /// Performs a case-insensitive check using the invariant culture.
        /// </summary>
        static public bool ContainsText(this string Instance, string Value)
        {
            if ((Instance != null) && !string.IsNullOrEmpty(Value))
            {
                return Instance.IndexOf(Value, StringComparison.InvariantCultureIgnoreCase) != -1;
            }

            return false;
        }
        /// <summary>
        /// Returns true if Instance starts with Value.
        /// Performs a case-insensitive check using the invariant culture.
        /// </summary>
        static public bool StartsWithText(this string Instance, string Value)
        {
            if ((Instance != null) && !string.IsNullOrEmpty(Value))
            {
                return Instance.StartsWith(Value, StringComparison.InvariantCultureIgnoreCase);
            }

            return false;
        }
        /// <summary>
        /// Splits the specified Text into lines, taking the Environment.NewLine as separator.
        /// </summary>
        static public string[] ToLines(this string Text)
        {
            if (string.IsNullOrEmpty(Text))
                return new string[0];

            Regex rx = new Regex(Environment.NewLine);
            return rx.Split(Text);

        }
        /// <summary>
        /// A Replace method with case-sensitivity configuration
        /// <para>NOTE: from comments in http://www.codeproject.com/Articles/10890/Fastest-C-Case-Insenstive-String-Replace </para>
        /// </summary>
        static public string Replace(this string original, string oldValue, string newValue, StringComparison comparisonType)
        {
            return Replace(original, oldValue, newValue, comparisonType, -1);
        }
        /// <summary>
        /// A Replace method with case-sensitivity configuration
        /// <para>NOTE: from comments in http://www.codeproject.com/Articles/10890/Fastest-C-Case-Insenstive-String-Replace </para>
        /// </summary>
        static public string Replace(this string original, string oldValue, string newValue, StringComparison comparisonType, int stringBuilderInitialSize)
        {
            if (original == null)
            {
                return null;
            }

            if (String.IsNullOrEmpty(oldValue))
            {
                return original;
            }

            int posCurrent = 0;
            int lenPattern = oldValue.Length;
            int idxNext = original.IndexOf(oldValue, comparisonType);
            StringBuilder result = new StringBuilder(stringBuilderInitialSize < 0 ? Math.Min(4096, original.Length) : stringBuilderInitialSize);

            while (idxNext >= 0)
            {
                result.Append(original, posCurrent, idxNext - posCurrent);
                result.Append(newValue);

                posCurrent = idxNext + lenPattern;

                idxNext = original.IndexOf(oldValue, posCurrent, comparisonType);
            }

            result.Append(original, posCurrent, original.Length - posCurrent);

            return result.ToString();
        }
        /// <summary>
        /// The string is returned truncated if exceeds Len.
        /// <para>If the Value is null or empty, Value is returned.</para>
        /// </summary>
        static public string MaxLen(this string Value, int Len)
        {
            if (string.IsNullOrEmpty(Value))
                return Value;

            if (Value.Length > Len)
                Value = Value.Remove(Len - 1);

            return Value;
        }
        /// <summary>
        /// Converts accented characters of the specified Text into non-accented characters
        /// <para>From: http://stackoverflow.com/questions/359827/ignoring-accented-letters-in-string-comparison</para>
        /// </summary>
        static public string RemoveDiacritics(this string Text)
        {
            if (Text != null)
            {
                return string.Concat(
                    Text.Normalize(NormalizationForm.FormD)
                    .Where(ch => CharUnicodeInfo.GetUnicodeCategory(ch) != UnicodeCategory.NonSpacingMark)
                  ).Normalize(NormalizationForm.FormC);
            }

            return Text;
        }

        /// <summary>
        /// Splits a camel case string by adding spaces between words.
        /// <para>It handles acronyms too, i.e. ABCamelDECase</para>
        /// </summary>
        static public string SplitCamelCase(this string Text)
        {
            return string.IsNullOrWhiteSpace(Text)? string.Empty: Regex.Replace(Text, "(?<=[a-z])([A-Z])", " $1", RegexOptions.Compiled).Trim();
        }

        /// <summary>
        /// Converts a string to an HTML-encoded string.
        /// </summary>
        static public string HtmlEncode(this string Text)
        {
            return string.IsNullOrWhiteSpace(Text) ? Text : HttpUtility.HtmlEncode(Text);
        }
        /// <summary>
        /// Converts a string that has been HTML-encoded for HTTP transmission into a decoded string
        /// </summary>
        static public string HtmlDecode(this string Text)
        {
            return string.IsNullOrWhiteSpace(Text) ? Text : HttpUtility.HtmlDecode(Text);
        }
        /// <summary>
        /// Converts a string to an Url-encoded string.
        /// </summary>
        static public string UrlEncode(this string Text)
        {
            return string.IsNullOrWhiteSpace(Text) ? Text : HttpUtility.UrlEncode(Text);
        }
        /// <summary>
        /// Converts a string that has been Url-encoded into a decoded string
        /// </summary>
        static public string UrlDecode(this string Text)
        {
            return string.IsNullOrWhiteSpace(Text) ? Text : HttpUtility.UrlDecode(Text);
        }

        /// <summary>
        /// Returns true if a specified text is a valid email address
        /// </summary>
        static public bool IsValidEmail(this string Text)
        {
            return !string.IsNullOrWhiteSpace(Text) && ValidEmailRegex.IsMatch(Text);
        }
        /// <summary>
        /// Returns true if a specified text is a valid mobile (cell) phone number.
        /// </summary>
        /// <param name="Text">The phone number</param>
        /// <param name="ValidLengths">Array with valid lengths the phone number may have, after removing spaces 
        /// and the + prefix of the international calling code, if there. Defaults to {10, 12}</param>
        static public bool IsValidMobilePhone(this string Text, int[] ValidLengths = null)
        {
            if (!string.IsNullOrWhiteSpace(Text))
            {
                Text = Text.Replace(" ", "").Trim();
                if (Text.StartsWith("+"))
                    Text = Text.Remove(0, 1);

                if (ValidLengths == null)
                    ValidLengths = new int[] { 10, 12 };

                if (ValidLengths.Contains(Text.Length))
                {
                    foreach (var C in Text)
                    {
                        if (!char.IsDigit(C))
                            return false;
                    }

                    return true;
                }

            }

            return false;
        }
    }
}
