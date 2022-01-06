/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Globalization;
using System.Reflection;

[assembly: CLSCompliant(true)]

//using Newtonsoft.Json;

namespace Tripous
{
    /// <summary>
    /// Helper
    /// </summary>
    static public partial class Sys
    {

        static DateTime today = DateTime.MinValue;



        /* public fields */
        /// <summary>
        /// Empty object
        /// </summary>
        static public readonly object EmptyObject = new object();
        /// <summary>
        /// The id of the primary thread
        /// </summary>
        static public readonly int PrimaryThreadId = Thread.CurrentThread.ManagedThreadId;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        static Sys()
        {

        }


        /* Exceptions */
        /// <summary>
        /// Returns the text of the Exception and any inner Exception 
        /// </summary>
        static public string ExceptionText(Exception E)
        {

            return ExceptionEx.GetExceptionText(E);

        }
        /// <summary>
        /// Throws an Exception
        /// </summary>
        static public void Throw(string Text)
        {
            if (string.IsNullOrWhiteSpace(Text))
                Text = "Unknown error";

            throw (new ExceptionEx(Text));
        }
        /// <summary>
        /// Throws an Exception
        /// </summary>
        static public void Throw(string Text, params object[] Args)
        {
            if ((Args != null) && (Args.Length > 0))
                Text = string.Format(Text, Args);
            throw (new ExceptionEx(Text));
        }
        /// <summary>
        /// Throws a "Not yet implemented" exception.
        /// </summary>
        static public void NotYet(string Feature)
        {
            if (Feature == null)
                Feature = string.Empty;

            Throw("Not yet implemented: " + Feature);
        }
        /// <summary>
        /// Throws an exception if Index is out of range
        /// </summary>
        static public void CheckListIndex(IList List, int Index)
        {
            if ((Index < 0) || (Index > List.Count - 1))
                Throw("Index out of range: {0}", Index);
        }

        /* Log */
        /// <summary>
        /// Log method, produces an information log entry
        /// </summary>
        static public void LogInfo(string Text, string Source = "", int EventId = 0)
        {
            Tripous.Logging.Logger.Info(Source, EventId.ToString(), Text);
        }
        /// <summary>
        /// Log method, produces a warning log entry
        /// </summary>
        static public void LogWarn(string Text, string Source = "", int EventId = 0)
        {
            Tripous.Logging.Logger.Warn(Source, EventId.ToString(), Text);
        }
        /// <summary>
        /// Log method, produces an error log entry
        /// </summary>
        static public void LogError(Exception Exception, string Source = "", int EventId = 0)
        {
            Tripous.Logging.Logger.Error(Source, EventId.ToString(), Exception);
        }
        /// <summary>
        /// Log method, produces an error log entry
        /// </summary>
        static public void LogError(string Text, string Source = "", int EventId = 0)
        {
            Tripous.Logging.Logger.Error(Source, EventId.ToString(), Text);
        }

        /* Strings */
        /// <summary>
        /// Case insensitive string equality.
        /// </summary>
        public static bool IsSameText(string A, string B)
        {
            return A.IsSameText(B);
        }
        /// <summary>
        /// Case sensitive string equality.
        /// </summary>
        public static bool IsSameStr(string A, string B)
        {
            return A.IsSameStr(B);
        }
        /// <summary>
        /// Returns a format string for float values, based on the specified Decimals
        /// </summary>
        static public string FormatStringFor(int Decimals)
        {
            string Result = "#,#";
            if (Decimals > 0)
                Result = Result + "0." + new string('0', Decimals);
            return Result;
        }
        /// <summary>
        /// Converts S to a valid indentifier name
        /// </summary>
        static public string ToValidIdentifier(string S)
        {
            string SLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            string SNumbers = "0123456789";
            string SValidChars = SLetters + SNumbers;

            StringBuilder SB = new StringBuilder();
            char c;
            for (int i = 0; i < S.Length; i++)
            {
                c = S[i];
                if ((i == 0) && char.IsDigit(c))
                    SB.Append("_");
                else if (SValidChars.IndexOf(c) == -1)
                    SB.Append("_");
                else
                    SB.Append(c);
            }

            return SB.ToString();
        }
        /// <summary>
        /// Splits a camel case string by adding spaces between words.
        /// <para>It handles acronyms too, i.e. ABCamelDECase</para>
        /// </summary>
        static public string SplitCamelCase(string Text)
        {
            return System.Text.RegularExpressions.Regex.Replace(Text, "(?<=[a-z])([A-Z])", " $1", System.Text.RegularExpressions.RegexOptions.Compiled).Trim();
        }
        /// <summary>
        /// Indicates whether a specified string is null, empty, or consists only of
        /// white-space characters.
        /// </summary>
        static public bool IsNullOrWhiteSpace(string S)
        {
            return string.IsNullOrWhiteSpace(S);
        }

        /// <summary>
        /// Combines the url base and the relative url into one, consolidating the '/' between them
        /// </summary>
        /// <param name="baseUrl"></param>
        /// <param name="relativeUrl">The relative path to combine</param>
        /// <returns>The merged url</returns>
        static public string UrlCombine(string baseUrl, string relativeUrl)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentNullException(nameof(baseUrl));

            if (string.IsNullOrWhiteSpace(relativeUrl))
                return baseUrl;

            baseUrl = baseUrl.TrimEnd('/');
            relativeUrl = relativeUrl.TrimStart('/');

            return $"{baseUrl}/{relativeUrl}";
        }
        /// <summary>
        /// Combines the url base and the array of relatives urls into one, consolidating the '/' between them
        /// </summary>
        /// <returns>The merged url</returns>
        static public string UrlCombine(string baseUrl, params string[] relativePaths)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentNullException(nameof(baseUrl));

            if (relativePaths.Length == 0)
                return baseUrl;

            var currentUrl = UrlCombine(baseUrl, relativePaths[0]);

            return UrlCombine(currentUrl, relativePaths.Skip(1).ToArray());
        }

        /// <summary>
        /// Αλγόριθμος ελέγχου εγκυρότητας ΑΦΜ (Τα ΑΦΜ είναι εννεαψήφιοι αριθμοί)
        /// Εστω ότι ο δοσμένος αριθμός είναι
        ///   Α1 Α2 Α3 Α4 Α5 Α6 Α7 Α8 Α9
        /// 
        /// 1. Υπολογίζουμε το άθροισμα Σ 
        ///    Σ = 256 * Α1 + 128 * Α2 + 64 * Α3 + 32 * Α4 + 16 * Α5 + 8 * Α6 + 4 * Α7 + 2 * Α8
        /// 2. Υπολογίζουμε το υπόλοιπο Υ της διαίρεσης Σ με τον αριθμό 11
        /// 3. Αν το υπόλοιπο Υ είναι 10 τότε Α9 = 0
        ///    Διαφορετικά, αν το Υ είναι μονοψήφιος αριθμός, τότε Α9 = Υ
        /// </summary>
        static public bool IsValidAFM(string AFM)
        {
            if (!string.IsNullOrWhiteSpace(AFM) && AFM.Length == 9 && AFM.All(c => char.IsNumber(c)))
            {
                //for (int i = 0; i < 9; i++)
                //    if (!char.IsNumber(AFM, i))
                //        return false;

                // 1
                int Sum = (256 * Convert.ToInt32(AFM[0].ToString()))
                        + (128 * Convert.ToInt32(AFM[1].ToString()))
                        + (64 * Convert.ToInt32(AFM[2].ToString()))
                        + (32 * Convert.ToInt32(AFM[3].ToString()))
                        + (16 * Convert.ToInt32(AFM[4].ToString()))
                        + (8 * Convert.ToInt32(AFM[5].ToString()))
                        + (4 * Convert.ToInt32(AFM[6].ToString()))
                        + (2 * Convert.ToInt32(AFM[7].ToString()));


                // 2
                Sum = Sum % 11;

                // 3
                if (Sum == 10)
                    return Convert.ToInt32(AFM[8].ToString()) == 0;
                else if ((Sum >= 0) && (Sum <= 9))
                    return Convert.ToInt32(AFM[8].ToString()) == Sum;
            }


            return false;

        }

        /// <summary>
        /// Returns true if a specified text is a valid email address
        /// </summary>
        static public bool IsValidEmail(string Text)
        {
            return Text.IsValidEmail();
        }
        /// <summary>
        /// Returns true if a specified text is a valid mobile (cell) phone number.
        /// </summary>
        /// <param name="Text">The phone number</param>
        /// <param name="ValidLengths">Array with valid lengths the phone number may have, after removing spaces 
        /// and the + prefix of the international calling code, if there. Defaults to {10, 12}</param>
        static public bool IsValidMobilePhone(string Text, int[] ValidLengths = null)
        {
            return Text.IsValidMobilePhone(ValidLengths);
        }

        /// <summary>
        /// Creates and returns a random string of a specified length, picking characters from a specified set of characters.
        /// </summary>
        static public string GenerateRandomString(int Length, string CharSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789")
        {
            if (string.IsNullOrWhiteSpace(CharSet))
                CharSet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            char[] Buffer = new char[Length];
            Random R = new Random();

            for (int i = 0; i < Buffer.Length; i++)
            {
                Buffer[i] = CharSet[R.Next(CharSet.Length)];
            }

            string Result = new string(Buffer);
            return Result;
        }

        /* passwords */
        /// <summary>
        /// Generates and returns a password of a specified length.
        /// <para>Adapted from: https://codeshare.co.uk/blog/how-to-create-a-random-password-generator-in-c/ </para>
        /// </summary>
        static public string GeneratePassword(int Length, string PasswordCharSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#$%^*()_-+=[{]};:>|./?")
        {
            // exclude & and < 

            if (Length < 6 || Length > 20)
                throw new ApplicationException("Password length must be between 6 and 20");

            if (string.IsNullOrWhiteSpace(PasswordCharSet))
                throw new ApplicationException("Password character set not specified");

            const int MaxIdenticalConsecutiveChars = 2;
            string Data = PasswordCharSet;

            char[] Password = new char[Length];
            int DataLength = Data.Length;

            bool Flag;
            Random Random = new Random();
            for (int CharPos = 0; CharPos < Length; CharPos++)
            {
                Password[CharPos] = Data[Random.Next(DataLength - 1)];

                Flag = CharPos > MaxIdenticalConsecutiveChars && Password[CharPos] == Password[CharPos - 1] && Password[CharPos - 1] == Password[CharPos - 2];

                if (Flag)
                {
                    CharPos--;
                }
            }

            return string.Join(null, Password);
        }
        /// <summary>
        /// Generates and returns a password of a specified length.
        /// <para>Adapted from: https://codeshare.co.uk/blog/how-to-create-a-random-password-generator-in-c/ </para>
        /// </summary>
        static public string GeneratePassword(int Length, bool LowerChars, bool UpperChars, bool Digits, string SpecialChars = @"~!@#$%^*()_-+=[{]};:>|./?")
        {
            // exclude & and < 

            StringBuilder SB = new StringBuilder();

            if (LowerChars)
                SB.Append("abcdefghijklmnopqrstuvwxyz");
            if (UpperChars)
                SB.Append("ABCDEFGHIJKLMNOPQRSTUVWXYZ");
            if (Digits)
                SB.Append("0123456789");
            if (!string.IsNullOrWhiteSpace(SpecialChars))
                SB.Append(SpecialChars);

            string PasswordCharSet = SB.ToString();

            return GeneratePassword(Length, PasswordCharSet);
        }
        /// <summary>
        /// Validates a password, according to specified requirements regarding its content and length, and returns the result.
        /// </summary>
        static public bool IsValidPassword(string Password, bool LowerChars = true, bool UpperChars = true, bool Digits = true, string SpecialChars = @"~!@#$%^*()_-+=[{]};:>|./?", int MinLength = 8, int MaxLength = 12)
        {
            //string SpecialCharacters = "@"!@#$%^*()_-+=[{]};:>|./?""; // exclude & and < 

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
            if (LowerChars && !Password.Any(char.IsLower))
                return false;
            if (UpperChars && !Password.Any(char.IsUpper))
                return false;
            if (Digits && !Password.Any(char.IsDigit))
                return false;
            if (SpecialChars != null && SpecialChars.Trim().Length > 0 && !Password.Any(SpecialChars.Contains))
                return false;

            /*
            if (string.IsNullOrWhiteSpace(Password)
            || (LowerChars && !Password.Any(char.IsLower))
            || (UpperChars && !Password.Any(char.IsUpper))
            || (Digits && !Password.Any(char.IsDigit))
            || (SpecialChars != null && SpecialChars.Trim().Length > 0 && !Password.Any(SpecialChars.Contains))
            || Password.Length < MinLength
            || Password.Length > MaxLength
            )
            return false; 
             */

            return true;
        }


        /* Paths and Files */
        /// <summary>
        /// Removes a trailing slash mark (e.g. c:\Temp\ ) from a file path.
        /// </summary>
        static public string RemoveTrailingSlash(string FilePath)
        {
            return string.IsNullOrWhiteSpace(FilePath)? string.Empty: FilePath.TrimEnd(new[] { '\\', '/',  });
        }

        /// <summary>
        /// Returns an array containing the characters that are not allowed in file names.
        /// </summary>
        static public char[] GetInvalidFileNameChars()
        {
            char[] InvalidFileChars = Path.GetInvalidFileNameChars();
            return InvalidFileChars;
        }
        /// <summary>
        /// Returns true if FileName is a valid file name, that is it just contains
        /// characters that are allowed in file names.
        /// </summary>
        static public bool IsValidFileName(string FileName)
        {
            char[] InvalidFileChars = GetInvalidFileNameChars();
            return FileName.IndexOfAny(InvalidFileChars) == -1;
        }
        /// <summary>
        /// Replaces any invalid file name characters from Source with spaces.
        /// </summary>
        static public string StrToValidFileName(string Source)
        {
            char[] InvalidFileChars = GetInvalidFileNameChars();
            StringBuilder SB = new StringBuilder(Source);
            foreach (char C in InvalidFileChars)
                SB.Replace(C, ' ');
            return SB.ToString();

        }

        /// <summary>
        /// Returns file names from the specified Folder that match the specified Filters.
        /// <para>Filters is a list of filters delimited by semicolon, i.e. *.gif;*.jpg;*.png;*.bmp; </para>
        /// <para> The returned file names include the full path.</para>
        /// </summary>
        static public string[] GetFiles(string Folder, string Filters, bool SearchSubFolders = false)
        {
            List<string> List = new List<string>();

            SearchOption SearchOption = SearchSubFolders ? System.IO.SearchOption.AllDirectories : System.IO.SearchOption.TopDirectoryOnly;
            string[] FilterItems = Filters.Split(';');

            foreach (string Filter in FilterItems)
            {
                List.AddRange(Directory.GetFiles(Folder, Filter, SearchOption));
            }

            return List.ToArray();
        }
        /// <summary>
        /// Copies Source folder and its contents to Destination folder.
        /// </summary>
        static public void CopyFolder(DirectoryInfo Source, DirectoryInfo Destination, bool Overwrite = true)
        {
            if (!Source.Exists)
                return;

            DirectoryInfo[] SourceSubFolders = Source.GetDirectories();
            FileInfo[] SourceFiles = Source.GetFiles();

            if (!Destination.Exists)
                Destination.Create();

            foreach (DirectoryInfo SourceSubFolder in SourceSubFolders)
                CopyFolder(SourceSubFolder, new DirectoryInfo(Path.Combine(Destination.FullName, SourceSubFolder.Name)), Overwrite);

            foreach (FileInfo SourceFile in SourceFiles)
                SourceFile.CopyTo(Path.Combine(Destination.FullName, SourceFile.Name), Overwrite);
        }
        /// <summary>
        /// Copies Source folder and its contents to Destination folder.
        /// </summary>
        static public void CopyFolder(string Source, string Destination, bool Overwrite = true)
        {
            DirectoryInfo SourceDI = new DirectoryInfo(Source.Trim());
            if (!SourceDI.Exists)
                return;

            DirectoryInfo DestinationDI = new DirectoryInfo(Destination.Trim());

            CopyFolder(SourceDI, DestinationDI, Overwrite);
        }
        /// <summary>
        /// Returns an array with folders where each folder is a storage card installed in the machine
        /// </summary>
        static public string[] GetStorageCardFolders()
        {
            FileAttributes AttrStorageCard = FileAttributes.Directory | FileAttributes.Temporary;
            List<string> List = new List<string>();

            DirectoryInfo FootFolder = new DirectoryInfo(@"\");

            foreach (DirectoryInfo DI in FootFolder.GetDirectories())
            {
                if ((DI.Attributes & AttrStorageCard) == AttrStorageCard)
                    List.Add(DI.FullName);
            }

            return List.ToArray();
        }

        /// <summary>
        /// Ensures that all the directories of the a file path exist.
        /// <para>WARNING: The specified file path MUST contain path and file name (with or without extension) information.
        /// The file name is ignored, but should be there. The rest of the path is created if it does not exist.</para>
        /// </summary>
        static public void EnsureDirectories(string FilePath)
        {
            string sPath = Path.GetDirectoryName(FilePath);
            if (!Directory.Exists(sPath))
                Directory.CreateDirectory(sPath);
        }

        /// <summary>
        /// Waits for a file to become available
        /// </summary>
        static public bool WaitForFile(string FilePath, int IntervalMSecs, int Retries)
        {
            for (int i = 0; i < Retries; i++)
            {
                try
                {
                    if (File.Exists(FilePath))
                    {
                        using (FileStream FS = File.Open(FilePath, FileMode.Open, FileAccess.Read, FileShare.None))
                        {
                            if (FS.Length > 0)
                            {
                                return true;
                            }
                        }
                    }

                    System.Threading.Thread.Sleep(IntervalMSecs);

                }
                catch
                {
                }
            }

            return false;
        }


        /// <summary>
        /// Saves an exception to a file
        /// </summary>
        static public void SaveToFile(Exception Ex)
        {
            string FileName = $"ERROR {DateTime.Now.ToFileName()}.log";
            File.WriteAllText(FileName, Ex.ToString());
        }
        /* to/from Base64 */
        /// <summary>
        /// Encodes Value into a Base64 string using the specified Enc.
        /// If End is null, the Encoding.Unicode is used.
        /// </summary>
        static public string StringToBase64(string Value, Encoding Enc)
        {
            if (Enc == null)
                Enc = Encoding.Unicode;

            byte[] Data = Enc.GetBytes(Value);
            return Convert.ToBase64String(Data);
        }
        /// <summary>
        /// Decodes the Base64 string Value into a string using the specified Enc.
        /// If End is null, the Encoding.Unicode is used.
        /// </summary>
        static public string Base64ToString(string Value, Encoding Enc)
        {
            if (Enc == null)
                Enc = Encoding.Unicode;

            byte[] Data = Convert.FromBase64String(Value);
            return Enc.GetString(Data);
        }
        /// <summary>
        /// Converts an image to a base64 string
        /// </summary>
        static public string ImageToBase64(Image Image, bool InsertLineBreaks = true)
        {
            if (Image != null)
            {
                Base64FormattingOptions Options = InsertLineBreaks ? Base64FormattingOptions.InsertLineBreaks : Base64FormattingOptions.None;

                using (MemoryStream MS = new MemoryStream())
                {
                    Image.Save(MS, Image.RawFormat);
                    byte[] Bytes = MS.ToArray();
                    return Convert.ToBase64String(Bytes, Options);            
                }   
            }

            return null;
        }
        /// <summary>
        /// Converts a base64 string back to an image
        /// </summary>
        static public Image Base64ToImage(string Text)
        {
            if (!string.IsNullOrWhiteSpace(Text))
            {
                byte[] Bytes = Convert.FromBase64String(Text);

                if ((Bytes != null) && (Bytes.Length > 0))
                {
                    MemoryStream MS = new MemoryStream(Bytes, 0, Bytes.Length);
                    MS.Write(Bytes, 0, Bytes.Length);
                    return Image.FromStream(MS, true);
                }
            }

            return null;
        }


        /* Convertions */
        /// <summary>
        /// Returns true if Value is null or DBNull
        /// </summary>
        static public bool IsNull(object Value)
        {
            return (Value == null) || (DBNull.Value == Value);
        }

        /// <summary>
        /// Formats and returns a double value
        /// </summary>
        static public string DoubleToStr(double Value, int Digits = 4)
        {
            return Value.ToString("0." + new string('0', Digits));
        }
        /// <summary>
        /// Formats and returns a double value
        /// </summary>
        static public string DecimalToStr(decimal Value, int Digits = 4)
        {
            return Value.ToString("0." + new string('0', Digits));
        }
        /// <summary>
        /// Converts a datetime into a string
        /// </summary>
        static public string DateTimeToStr(DateTime Value, bool UseMSecs = false)
        {
            return UseMSecs ? Value.ToString("yyyy-MM-dd HH:mm:ss.fff") : Value.ToString("yyyy-MM-dd HH:mm:ss");
        }
        /// <summary>
        /// Converts a date into a string
        /// </summary>
        static public string DateToStr(DateTime Value)
        {
            return Value.ToString("yyyy-MM-dd");
        }

        /// <summary>
        /// Returns the Value as a value of T. If is null returns Default.
        /// </summary>
        static public T AsValue<T>(object Value, T Default)
        {
 
            try
            {
                if (IsNull(Value))
                    return Default;

                if (Default != null && Default.GetType().IsValueType)
                {
                    Type DefaultType = Default.GetType();

                    if (Value.GetType() == DefaultType)
                        return (T)Value;

                    if (DefaultType.ImplementsInterface(typeof(IConvertible)))
                        return (T)System.Convert.ChangeType(Value, DefaultType, CultureInfo.InvariantCulture);                    
                }

                return (T)Value;
 
            }
            catch
            {
                return Default;
            }
        }

        /// <summary>
        /// Converts a value to string, if possible, else returns Default
        /// </summary>
        static public string AsString(object Value, string Default = "")
        {
            return !IsNull(Value) ? Value.ToString() : Default;
        }
        /// <summary>
        /// Converts a value to int, if possible, else returns Default
        /// </summary>
        static public int AsInteger(object Value, int Default = 0)
        {
            try
            {
                return !IsNull(Value) ? Convert.ToInt32(Value) : Default;
            }
            catch
            {
            }

            return Default;
        }
        /// <summary>
        /// Converts a value to bool, if possible, else returns Default
        /// </summary>
        static public bool AsBoolean(object Value, bool Default = false)
        {
            try
            {
                return !IsNull(Value) ? Convert.ToBoolean(Value) : Default;
            }
            catch
            {
            }

            return Default;

        }
        /// <summary>
        /// Converts a value to double, if possible, else returns Default
        /// </summary>
        static public double AsDouble(object Value, double Default = 0)
        {
            try
            {
                if (IsNull(Value))
                    return 0;

                if (Value.GetType() == typeof(string))
                    return Convert.ToDouble(Value.ToString(), CultureInfo.InvariantCulture);

                return Convert.ToDouble(Value);
            }
            catch
            {
            }


            return Default;
        }
        /// <summary>
        /// Converts a value to decimal, if possible, else returns Default
        /// </summary>
        static public decimal AsDecimal(object Value, decimal Default = 0)
        {
            try
            {
                if (IsNull(Value))
                    return Default;

                if (Value.GetType() == typeof(string))
                    return Convert.ToDecimal(Value.ToString(), CultureInfo.InvariantCulture);

                return Convert.ToDecimal(Value);
            }
            catch
            {
            }

            return Default;
        }
        /// <summary>
        /// Converts a value to DateTime, if possible, else returns Default
        /// </summary>
        static public DateTime AsDateTime(object Value, DateTime Default)
        {
            try
            {
                return !IsNull(Value) ? StrToDateTime(Value.ToString()): Default ;
            }
            catch
            {                
            }

            return Default;
        }

        /// <summary>
        /// Converts a string into integer, if possible, else returns default.
        /// </summary>
        public static int StrToInt(string S, int Default = 0)
        {
            int Result = 0;
            if (TryStrToInt(S, out Result))
                return Result;
            return Default;
        }
        /// <summary>
        /// Converts a string into double, if possible, else returns default.
        /// </summary>
        public static double StrToDouble(string S, double Default = 0)
        {
            double Result = 0;
            if (TryStrToDouble(S, out Result))
                return Result;
            return Default;
        }
        /// <summary>
        /// Converts a string into decimal, if possible, else returns default.
        /// </summary>
        public static decimal StrToDecimal(string S, decimal Default = 0)
        {
            decimal Result = 0;
            if (TryStrToDecimal(S, out Result))
                return Result;
            return Default;
        }
        /// <summary> 
        /// Converts a string to a DateTime value. The string must be defined in one of the ISODateTimeFormats
        /// </summary>
        public static DateTime StrToDateTime(string S)
        {
            try
            {
                return DateTime.Parse(S);
            }
            catch
            {
                return DateTime.ParseExact(S, Sys.ISODateTimeFormats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None);
            }
        }
        /// <summary> 
        /// Converts a string to a DateTime value. The string must be defined in one of the ISODateTimeFormats
        /// </summary>
        public static DateTime StrToDateTime(string S, DateTime Default)
        {
            DateTime Result = DateTime.Now;
            if (TryStrToDateTime(S, out Result))
                return Result;
            return Default;
        }
        /// <summary> 
        /// Converts a string to a DateTime.Date value. The string must be defined in one of the ISODateTimeFormats
        /// </summary>
        public static DateTime StrToDate(string S, DateTime Default)
        {
            return StrToDateTime(S, Default).Date;
        }
        /// <summary>
        /// Converts a string into an enum value, if possible, else returns a default value.
        /// </summary>
        public static object StrToEnum(string S, Type EnumType, object Default)
        {
            object Result = null;
            if (TryStrToEnum(S, out Result, EnumType))
                return Result;
            return Default;
        }

        /// <summary>
        /// Converts a string into an integer. Returns true on success.
        /// </summary>
        public static bool TryStrToInt(string S, out int Value)
        {
            return int.TryParse(S, out Value);
        }
        /// <summary>
        /// Converts a string into a double. Returns true on success.
        /// </summary>
        public static bool TryStrToDouble(string S, out double Value)
        {
            Value = 0;
            bool Result = double.TryParse(S, out Value);

            if (!Result)
            {
                try
                {
                    Value = double.Parse(S, CultureInfo.InvariantCulture);
                    Result = true;
                }
                catch
                {
                }
            }
 
            return Result;
        }
        /// <summary>
        /// Converts a string into a decimal. Returns true on success.
        /// </summary>
        public static bool TryStrToDecimal(string S, out decimal Value)
        {
            Value = 0;
            bool Result = decimal.TryParse(S, out Value);

            if (!Result)
            {
                try
                {
                    Value = decimal.Parse(S, CultureInfo.InvariantCulture);
                    Result = true;
                }
                catch
                {
                }
            }

            return Result;
        }
        /// <summary>
        /// Converts a string into a DateTime. Returns true on success.
        /// </summary>
        public static bool TryStrToDateTime(string S, out DateTime Value)
        {
            Value = DateTime.MinValue;

            bool Result = DateTime.TryParse(S, out Value);
 
            if (!Result)
            {
                try
                {
                    Value = DateTime.ParseExact(S, Sys.ISODateTimeFormats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.None);
                    Result = true;
                }
                catch
                {
                }
            }

            return Result;
        }
        /// <summary>
        /// Converts a string into an enum value. Returns true on success.
        /// </summary>
        public static bool TryStrToEnum(string S, out object Value, Type EnumType)
        {
            Value = null;
            bool Result = true;
            try
            {
                Value = Enum.Parse(EnumType, S, true);
            }
            catch
            {
                Result = false;
            }

            return Result;
        }
        /// <summary>
        /// Formats a TimeSpan returning a string as hh:mm:ss 
        /// </summary>
        static public string FormatTimeSpan(TimeSpan TS)
        {
            return string.Format("{0:00}:{1:00}:{2:00}", Convert.ToInt32(TS.TotalHours), TS.Minutes, TS.Seconds);
        }

        /* numerics */
        /// <summary>
        /// Returns the decimal separator character of the TagInfo culture
        /// CAUTION: TagInfo should be a Specific culture, not a Neutral one
        /// </summary>
        /// <param name="Info">A Specific Culture object</param>
        static public char GetDecimalSeparator(CultureInfo Info)
        {
            return Info.NumberFormat.NumberDecimalSeparator.Trim()[0];
        }
        /// <summary>
        /// Calculates and returns, in a safe manner (i.e. avoiding division by zero errors)
        /// the percent of the specifie values: (Numerator / Denominator) * 100.0
        /// </summary>
        static public double Percent(double Numerator, double Denominator)
        {
            if ((Numerator != 0) && (Denominator != 0))
                return (Numerator / Denominator) * 100.0;
            else
                return 0;
        }
        /// <summary>
        /// Calculates and returns, in a safe manner (i.e. avoiding division by zero errors)
        /// the percent of the specifie values: (Numerator / Denominator) * 100.0
        /// </summary>
        static public decimal Percent(decimal Numerator, decimal Denominator)
        {
            return (Numerator == 0) || (Denominator == 0) ? 0 : (Numerator / Denominator) * 100;
        }

        /* DateTime */
        /// <summary>
        /// Returns the date separator character of the Info culture
        /// CAUTION: Info should be a Specific culture, not a Neutral one
        /// </summary>
        public static char GetDateSeparator(CultureInfo CultureInfo)
        {
            return CultureInfo.DateTimeFormat.DateSeparator.Trim()[0];
        }
        /// <summary>
        /// Returns the date separator character of the current UI culture
        /// </summary>
        public static char GetDateSeparator()
        {
            return GetDateSeparator(GetCurrentCulture());
        }
        /// <summary>
        /// Returns the time separator character of the Info culture
        /// CAUTION: Info should be a Specific culture, not a Neutral one
        /// </summary>
        /// <param name="Info">A Specific Culture object</param>
        public static char GetTimeSeparator(CultureInfo Info)
        {
            return Info.DateTimeFormat.TimeSeparator.Trim()[0];
        }
        /// <summary>
        /// Returns the time separator character of the current UI culture
        /// </summary>
        public static char GetTimeSeparator()
        {
            return GetTimeSeparator(GetCurrentCulture());
        }
        /// <summary>
        /// Returns a <see cref="DatePattern"/> value by analyzing the DateFormat.
        /// </summary>
        public static DatePattern GetDatePattern(string DateFormat)
        {
            return Tripous.DateFormat.GetDatePattern(DateFormat);
        }
        /// <summary>
        /// Returns a <see cref="DatePattern"/> value by analyzing the ShortDatePattern of the current culture.
        /// </summary>
        public static DatePattern GetDatePattern()
        {
            return GetDatePattern(GetCurrentCulture().DateTimeFormat.ShortDatePattern);
        }
        /// <summary>
        /// Returns a unified date format string usefull in formatting dates by using the DateTime.ToString() method
        /// i.e DT.ToString(Mask.GetDateMaskFormat(true, Info))
        /// CAUTION: Info should be a Specific culture, not a Neutral one
        /// </summary>
        /// <param name="TwoDigitYear">true for two digit year formatting</param>
        /// <param name="Info">A Specific Culture object</param>
        public static string GetDateMaskFormat(bool TwoDigitYear, CultureInfo Info)
        {
            return Tripous.DateFormat.GetDateMaskFormat(TwoDigitYear, Info);
        }
        /// <summary>
        /// Returns a unified date format string usefull in formatting dates by using the DateTime.ToString() method
        /// i.e DT.ToString(Mask.GetDateMaskFormat(true, Info))
        /// </summary>
        public static string GetDateMaskFormat(bool TwoDigitYear)
        {
            return GetDateMaskFormat(TwoDigitYear, GetCurrentCulture());
        }
        /// <summary>
        /// Normalizes a date string according to the date mask format the GetDateMaskFormat() returns.
        /// </summary>
        static public string NormalizeDateTime(string Text, CultureInfo CultureInfo)
        {
            return Tripous.DateFormat.NormalizeDateTime(Text, CultureInfo);
        }
        /// <summary>
        /// Normalizes a date string according to the date mask format the GetDateMaskFormat() returns.
        /// </summary>
        static public string NormalizeDateTime(string Text)
        {
            return NormalizeDateTime(Text, GetCurrentCulture());
        }

        /* object extensions */
        /// <summary>
        /// Clones an object.
        /// <para>The specified instance should be marked with the SerializableAttribute and provide a default constructor.</para>
        /// </summary>
        static public T CloneObject<T>(this T Source) where T: new()
        { 
            if (ReferenceEquals(Source, null))
                return default(T);

            string JsonText = Json.Serialize(Source);
            T Dest = Json.Deserialize<T>(JsonText);
            return Dest; 
        }
        /// <summary>
        /// Assigns a source object properties to a dest object
        /// </summary>
        static public void AssignObject<T, T2>(T Source, T2 Dest)
        {
            string JsonText = Json.Serialize(Source);
            Json.PopulateObject(Dest, JsonText);
        }

        /* Culture */
        /// <summary>
        /// Returns the current UI culture in a safe manner
        /// </summary>
        static public CultureInfo GetCurrentCulture()
        {
            CultureInfo Info = CultureInfo.CurrentUICulture;
            if (Info.IsNeutralCulture)
                Info = GetFirstSpecificCulture(Info);
            return Info;
        }
        /// <summary>
        /// Returns the first specific culture of the specified neutral ParentCulture.
        /// </summary>
        static public CultureInfo GetFirstSpecificCulture(CultureInfo ParentCulture)
        {
            if (!ParentCulture.IsNeutralCulture)
                return (CultureInfo)ParentCulture.Clone();

            CultureInfo Res = null;
#if !COMPACT
            foreach (CultureInfo CI in CultureInfo.GetCultures(CultureTypes.SpecificCultures))
                if (CI.Name.StartsWith(ParentCulture.Name))
#else
            foreach (CultureInfo CI in CultureInfoHelper.GetCultures())
                if (!CI.IsNeutralCulture &&  CI.Name.StartsWith(ParentCulture.Name))
#endif
                {
                    Res = CI;
                    break;
                }

            if (Res == null)
                throw new Exception(string.Format("Can not find a specific culture for neutral culture {0}", ParentCulture.Name));

            return Res;
        }
        /// <summary>
        /// Sets the CurrentUICulture and CurrentCulture of the Thread.CurrentThread
        /// to a culture passed as a command line argument. Use it as the very first
        /// call in the Main() method of the application.
        /// <para>The argument must have the format lang:CultureName </para>
        /// <para>For example lang:en-US, lang:el-GR  </para>
        /// <para>Example command line for a windows desktop shortcut</para>
        /// <para><c>"C:\My files\MyApp.exe" lang:el-GR</c></para>
        /// </summary>
        static public void SetApplicationCulture(string[] Args)
        {
#if !COMPACT
            if ((Args != null) && (Args.Length > 0))
            {

                string CultureName = string.Empty;

                foreach (string Arg in Args)
                {
                    if (Arg.StartsWith("lang"))
                    {
                        int Index = Arg.IndexOf(':');
                        if (Index != -1)
                        {
                            Index++;
                            CultureName = Arg.Substring(Index, Arg.Length - Index);
                            break;
                        }

                    }
                }
                // lang:el-GR
                // lang:en-US
                if (!string.IsNullOrWhiteSpace(CultureName))
                {
                    CultureInfo Culture = CultureInfo.GetCultureInfo(CultureName);
                    if ((Culture != null) && !Culture.IsNeutralCulture)
                    {
                        System.Threading.Thread.CurrentThread.CurrentUICulture = Culture;
                        System.Threading.Thread.CurrentThread.CurrentCulture = Culture;
                    }
                }

            }
#endif
        }

        /* Reflection */
        /// <summary>
        /// Tries to infer Type-s of Args. If an Arg is null, it infers the typeof(object)
        /// </summary>
        static public Type[] GetArgTypes(object[] Args)
        {
            if ((Args == null) || (Args.Length == 0))
                return new Type[0];

            Type[] Result = new Type[Args.Length];

            for (int i = 0; i < Args.Length; i++)
            {
                if (Args[i] == null)
                    Result[i] = typeof(object);
                else
                    Result[i] = Args[i].GetType();
            }

            return Result;
        }
        /// <summary>
        /// Returns true if a specified type is a nullable type
        /// </summary>
        static public bool IsNullable(Type T)
        {
            return Nullable.GetUnderlyingType(T) != null;
        }

        /// <summary>
        /// Returns true if the DestProp is an array property
        /// </summary>
        static public bool IsArrayProperty(PropertyInfo PropInfo)
        {
            return (PropInfo != null) && (PropInfo.PropertyType.IsArray) && (PropInfo.GetIndexParameters().GetLength(0) > 0);
        }
        /// <summary>
        /// Returns true if the DestProp is an indexer property
        /// </summary>
        static public bool IsIndexer(PropertyInfo PropInfo)
        {
            return (PropInfo != null) && (string.Compare("Item", PropInfo.Name, true) == 0) && (PropInfo.GetIndexParameters().GetLength(0) > 0);
        }
        /// <summary>
        /// Returns true if the DestProp is an one-dimensional integer indexer property 
        /// </summary>
        static public bool IsIntegerIndexer(PropertyInfo PropInfo)
        {
            return (string.Compare("Item", PropInfo.Name, true) == 0)
                    && (PropInfo.GetIndexParameters().GetLength(0) == 1)
                    && (PropInfo.GetIndexParameters()[0].ParameterType == typeof(int));
        }
        /// <summary>
        /// Returns Type.Name without any of the `0123456789+ characters it may contain.
        /// </summary>
        static public string NormalizeTypeName(Type Type)
        {
            StringBuilder SB = new StringBuilder();

            foreach (char c in Type.Name)
                if ("`0123456789+".IndexOf(c) == -1)
                    SB.Append(c);

            return SB.ToString();

        }
        /// <summary>
        /// Returns the Type specified by TypeName. The Type must be registered with the TypeStore.
        /// </summary>
        static public Type GetType(string TypeName, bool CheckExists)
        {
            if (string.IsNullOrWhiteSpace(TypeName))
                throw new ApplicationException("Can not get a type. TypeName is null or empty");

            Type Result = TypeStore.Find(TypeName);
            if ((Result == null) && CheckExists)
                throw new ApplicationException(string.Format("Type not found: {0}", TypeName));

            return Result;
        }

        /// <summary>
        /// Returns a string that contains the public interface of the Type T
        /// </summary>
        static public string GetTypePublicInterface(Type T)
        {
            return GetTypeInterface(T, false);
        }
        /// <summary>
        /// Returns a string that contains the public interface of the Type T
        /// </summary>
        static public string GetTypeInterface(Type T, bool PrivateToo)
        {
            if (T == null)
                return string.Empty;

            StringBuilder SB = new StringBuilder();
            BindingFlags Flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
            if (PrivateToo)
                Flags |= BindingFlags.NonPublic;

            FieldInfo[] FIs = T.GetFields(Flags);
            if ((FIs != null) && (FIs.Length > 0))
            {
                Array.Sort(FIs, (A, B) => { return string.Compare(A.Name, B.Name); });
                SB.AppendLine("// Fields ----------------------------- ");
                foreach (FieldInfo Item in FIs)
                    SB.AppendLine(Item.ToString());

                SB.AppendLine();
            }

            MethodInfo[] MIs = T.GetMethods(Flags);

            if ((MIs != null) && (MIs.Length > 0))
            {
                Array.Sort(MIs, (A, B) => { return string.Compare(A.Name, B.Name); });
                SB.AppendLine("// Methods ----------------------------- ");
                foreach (MethodInfo Item in MIs)
                    if (!Item.Name.StartsWith("get_") && !Item.Name.StartsWith("set_") && !Item.Name.StartsWith("add_") && !Item.Name.StartsWith("remove_"))
                        SB.AppendLine(Item.ToString());

                SB.AppendLine();
            }


            PropertyInfo[] PIs = T.GetProperties(Flags);
            if ((PIs != null) && (PIs.Length > 0))
            {
                Array.Sort(PIs, (A, B) => { return string.Compare(A.Name, B.Name); });
                SB.AppendLine("// Properties ----------------------------- ");
                foreach (PropertyInfo Item in PIs)
                    SB.AppendLine(Item.ToString());

                SB.AppendLine();
            }

            EventInfo[] EIs = T.GetEvents(Flags);
            if ((EIs != null) && (EIs.Length > 0))
            {
                Array.Sort(PIs, (A, B) => { return string.Compare(A.Name, B.Name); });
                SB.AppendLine("// Events ----------------------------- ");
                foreach (EventInfo Item in EIs)
                    SB.AppendLine(Item.ToString());

                SB.AppendLine();
            }

            return SB.ToString();
        }

        /// <summary>
        /// Returns the invocation list of the event specified by EventName of the specified Instance 
        /// using Reflection.
        /// <para>It may return an empty array when there is no event with EventName.</para>
        /// </summary>
        static public Delegate[] GetEventInvocationList(object Instance, string EventName)
        {
            if (Instance != null)
            {
                Type T = Instance.GetType();
                IList<FieldInfo> Fields = T.GetFields(typeof(object));

                if (Fields != null)
                {
                    foreach (FieldInfo FI in Fields)
                    {
                        if (EventName.IsSameText(FI.Name))
                        {
                            Delegate Delegate = FI.GetValue(Instance) as Delegate;
                            if (Delegate != null)
                            {
                                return Delegate.GetInvocationList();
                            }

                            break;
                        }
                    }
                }
            }

            return new List<Delegate>().ToArray();
        }
        /// <summary>
        /// Returns true if the event specified by EventName of the specified Instance is assigned at least once.
        /// That is it has at least one listener.
        /// </summary>
        static public bool IsEventAssigned(object Instance, string EventName)
        {
            Delegate[] InvocationList = Sys.GetEventInvocationList(Instance, EventName);
            return (InvocationList != null) && (InvocationList.Length > 0);
        }

        /// <summary>
        /// Invokes the MethodName method of the Instance passing it the specified Arguments.
        /// <para>That method must not be a constructor or a type initializer.</para>
        /// </summary>
        static public object Invoke(object Instance, string MethodName, object[] Arguments)
        {
            Type T = Instance.GetType();
            BindingFlags Flags = BindingFlags.Instance | BindingFlags.InvokeMethod | BindingFlags.Public | BindingFlags.NonPublic;
            return T.InvokeMember(MethodName, Flags, null, Instance, Arguments);
        }
        /// <summary>
        /// Invokes the MethodName method of the Instance.
        /// <para>That method must not be a constructor or a type initializer.</para>
        /// </summary>
        static public object Invoke(object Instance, string MethodName)
        {
            return Invoke(Instance, MethodName, new object[] { });
        }

        /// <summary>
        /// Returns the value of the indexer property of the Instance.
        /// </summary>
        static public object GetProperty(object Instance, object Index)
        {
            BindingFlags Flags = BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            Type T = Instance.GetType();
            try
            {
                return T.InvokeMember("Item", Flags, null, Instance, new object[] { Index }); 
            }
            catch
            {
            }

            return null;

        }
        /// <summary>
        /// Sets the value of the indexer property of the Instance.
        /// </summary>
        static public void SetProperty(object Instance, object Index, object Value)
        {
            BindingFlags Flags = BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            Type T = Instance.GetType();
            try
            {
                T.InvokeMember("Item", Flags, null, Instance, new object[] { Index, Value });  
            }
            catch
            {
            }
        }

        /// <summary>
        /// Returns the value property or field of the Instance.
        /// </summary>
        static public object GetProperty(object Instance, string Name)
        {
            BindingFlags Flags = BindingFlags.GetProperty | BindingFlags.GetField | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            Type T = Instance.GetType();
            try
            {
                return T.InvokeMember(Name, Flags, null, Instance, new object[] { });  
            }
            catch
            {
            } 

            return null;

        }
        /// <summary>
        /// Sets the Value as the value of a property or field of the Instance.
        /// </summary>
        static public void SetProperty(object Instance, string Name, object Value)
        {
            BindingFlags Flags = BindingFlags.SetProperty | BindingFlags.SetField | BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            Type T = Instance.GetType();

            try
            {
                T.InvokeMember(Name, Flags, null, Instance, new object[] { Value });
                return;
            }
            catch
            {
            }

        }

        /// <summary>
        /// Returns the value of static property or static field of a type.
        /// </summary>
        static public object GetStaticProperty(Type T, string Name)
        {
            BindingFlags Flags = BindingFlags.GetProperty | BindingFlags.GetField | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
  
            try
            {
                return T.InvokeMember(Name, Flags, null, T, new object[] { });
            }
            catch
            {
            }

            return null;

        }
        /// <summary>
        /// Sets the value of static property or static field of a type.
        /// </summary>
        static public void SetStaticProperty(Type T, string Name, object Value)
        {
            BindingFlags Flags = BindingFlags.SetProperty | BindingFlags.SetField | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
 

            try
            {
                T.InvokeMember(Name, Flags, null, T, new object[] { Value });
                return;
            }
            catch
            {
            }

        }
 
        /// <summary>
        /// True if type T contains a property with PropertyName
        /// </summary>
        static public bool HasProperty(Type T, string PropertyName)
        {
            try
            {
                return T.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                        .Any(p => p.Name == PropertyName);
            }
            catch
            {
            }

            return false;
        }
        /// <summary>
        /// True if the type of the Instance contains a property with PropertyName
        /// </summary>
        static public bool HasProperty(object Instance, string PropertyName)
        {
            return (Instance != null) && HasProperty(Instance.GetType(), PropertyName);
        }

        /* miscs */
        /// <summary>
        /// Creates and returns a new Guid.
        /// <para>If UseBrackets is true, the new guid is surrounded by {}</para>
        /// </summary>
        static public string GenId(bool UseBrackets)
        {
            string format = UseBrackets ? "B" : "D";
            return Guid.NewGuid().ToString(format).ToUpper();
        }
        /// <summary>
        /// Creates and returns a new Guid WITHOUT surrounding brackets, i.e. {}
        /// </summary>
        static public string GenId()
        {
            return GenId(false);
        }
        /// <summary>
        /// For a really global mutex name, that is "shared" between users/sessions 
        /// we have to prefix the name of the mutex with "Global\"
        /// </summary>
        static public string GlobalMutexName(string MutexName)
        {
            return string.Format(@"Global\{0}", MutexName);  // http://stackoverflow.com/questions/7672596/system-threading-mutex-with-service-and-console
        }
        /// <summary>
        /// Returns a string of the form TableName.FieldName
        /// </summary>
        static public string FieldPath(string TableName, string FieldName)
        {
            return (string.IsNullOrWhiteSpace(TableName) ? string.Empty : TableName + ".") + FieldName;
        }
        /// <summary>
        /// Constructs a field at table string. By default is TableName__FieldName.
        /// </summary>
        static public string FieldAlias(string TableName, string FieldName)
        {
            return (string.IsNullOrWhiteSpace(TableName) ? string.Empty : TableName + Sys.FieldAliasSep) + FieldName;
        }

        /// <summary>
        /// Returns a valid Id value in case where Id is null or empty
        /// </summary>
        static public object SafeId(object Id)
        {
            if (Sys.IsNull(Id))
                Id = SysConfig.OidDataType == DataFieldType.String ? (object)Sys.InvalidId : (object)int.MinValue;
            return Id;
        }
        /// <summary>
        /// Returns a string representation of the Id, suitable for sql statement formatting,
        /// according to SysConfig.OidDataType setting
        /// </summary>
        static public string IdStr(object Id)
        {
            return SysConfig.OidDataType == DataFieldType.String ? Id.ToString().QS() : Id.ToString();
        }
        /// <summary>
        ///  Used in constructing SQL statements that contain a WHERE clause of the type
        ///  <para><c>  where FIELD_NAME in (...)</c></para>
        ///  This method limits the number of elements inside the in (...) according to the passed in ModValue, in order
        ///  to avoid problems with database servers that have such a limit.
        ///  <para>It returns a string array where each element contains no more than ModValue of the FieldName values from Table.</para>
        /// </summary>
        static public string[] GetKeyValuesList(IList<object> SourceList, string FieldName, int ModValue, bool DiscardBelowZeroes)
        {
            List<string> List = new List<string>();

            if ((SourceList != null) && (SourceList.Count > 0))
            {
                bool IsString = false;

                foreach (var v in SourceList)
                {
                    if (v != null)
                    {
                        IsString = v.GetType() == typeof(string);
                        break;
                    }
                }

 
                int Counter = 0;
                string S = "";
                object Value;

                for (int i = 0; i < SourceList.Count; i++)
                {
                    if (SourceList[i] != null)
                    {
                        Value = SourceList[i];
                        if (IsString)
                        {
                            S = S + string.Format("'{0}', ", Value.ToString());
                            Counter++;
                        }
                        else if ((!DiscardBelowZeroes) || (DiscardBelowZeroes && (AsInteger(Value, -1) > 0)))
                        {
                            S = S + string.Format("{0}, ", Value);
                            Counter++;
                        }
                    }

                    if (Counter % ModValue == 0)
                    {
                        List.Add(S);
                        S = "";
                    }

                }


                if (S != "")
                    List.Add(S);


                // remove last comma
                for (int i = 0; i < List.Count; i++)
                {
                    S = List[i].Trim();
                    if (S.Length > 1)
                        S = S.Remove(S.Length - 1, 1);

                    List[i] = string.Format(" ({0}) ", S);
                }

            }

            return List.ToArray();
        }


        /* properties */
        /// <summary>
        /// To be used by user code
        /// </summary>
        static public Dictionary<string, object> Variables { get; private set; } = new Dictionary<string, object>();
        /// <summary>
        /// Gets or sets the global Today DateTime value.
        /// </summary>
        static public DateTime Today
        {
            get { return today == DateTime.MinValue ? DateTime.Now : today; }
            set { today = value; }
        }



        /// <summary>
        /// The username of the current user of this application, if any, else null
        /// </summary>
        static public string AppUserName { get; set; }

 

        /// <summary>
        /// The username of the current user of the local computer
        /// </summary>
        static public string NetUserName { get; private set; } = Environment.UserName;
        /// <summary>
        /// The name of the local computer
        /// </summary>
        static public string HostName { get; private set; } = System.Net.Dns.GetHostName();
 

    }
}
