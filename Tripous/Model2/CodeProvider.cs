using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.Common;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Tripous.Data;

namespace Tripous.Model2
{
    /// <summary>
    /// Produces unique Codes. The decriptor of this class describes the form of the Code.
    /// <para>See the <see cref="CodeProviderPartType"/> enum for more information. </para>
    /// </summary>
    public class CodeProvider
    {
        /// <summary>
        /// Valid separators for code part characters
        /// </summary>
        static public readonly string ValidSeparators = @".-\/ ";
        /// <summary>
        /// Valid separators for code part characters
        /// </summary>
        static public readonly char[] ValidSeparatorsChars = ValidSeparators.ToCharArray();

        static List<CodeProviderDef> Descriptors = new List<CodeProviderDef>();

        CodeProviderDef Descriptor;
        string TableName;
        List<CodeProviderPart> Parts = new List<CodeProviderPart>();
        DataRow CurrentRow;
        SqlStore Store;
        DbTransaction Transaction;



        /* execution */
        /// <summary>
        /// Parses the text specified in the descriptor and creates the parts.
        /// </summary>
        protected void ParseDescriptor()
        {
            string[] TextParts = Descriptor.Text.Split('|', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            foreach (string TextPart in TextParts)
                Parts.Add(new CodeProviderPart(TextPart));

            if (Parts.Count == 0)
                Sys.Throw($"No {this.GetType().Name} Code Parts.");

            if (Parts.Count(item => item.Type == CodeProviderPartType.Pivot) > 1)
                Sys.Throw($"{this.GetType().Name} contains more than one Pivot Code Parts.");

            if (!(Parts.Last().Type == CodeProviderPartType.Pivot || Parts.Last().Type == CodeProviderPartType.Sequencer || Parts.Last().Type == CodeProviderPartType.NumericSelect))
                Sys.Throw($"The last part of a {this.GetType().Name} must be a {CodeProviderPartType.Pivot}, a {CodeProviderPartType.Sequencer}  or a {CodeProviderPartType.NumericSelect} part.");
        }
        /// <summary>
        /// Checks if this instance is efficiently initialized and so it is valid for a client to
        /// call the Execute() method. Throws exceptions if Executor or the CurrentRow is null
        /// and if the format of the various parts is not valid.
        /// </summary>
        protected void CheckExecute(CodeProviderPart Part)
        {
            if ((Part.Type == CodeProviderPartType.NumericSelect || Part.Type == CodeProviderPartType.Sequencer) && Store == null)
                Sys.Throw($"{this.GetType().Name}. SqlStore is null");

            if (Part.Type == CodeProviderPartType.NumericSelect && CurrentRow == null)
                Sys.Throw($"{this.GetType().Name}. CurrentRow is null");

            if (Part.Type == CodeProviderPartType.NumericSelect && string.IsNullOrWhiteSpace(TableName))
                Sys.Throw($"{this.GetType().Name}. TableName is empty");

        }
        /// <summary>
        /// Executes a SELECT statement and returns a signle value.
        /// </summary>
        protected virtual string Select(string SqlText)
        {
            DataTable Table = Transaction != null ? Store.Select(Transaction, SqlText, CurrentRow) : Store.Select(SqlText, CurrentRow);
            if (Table.Rows.Count > 0)
                return Table.Rows[0].IsNull(0) ? "" : Table.Rows[0][0].ToString();
            return string.Empty;
        }
        /// <summary>
        /// Formats a part value according to the format of the specified part.
        /// <para>Handles parts having a format only.</para>
        /// <para>The passed value should be a string with numeric characters only.</para>
        /// <para>If the type of the part is <see cref="CodeProviderPartType.Pivot"/>, then it increments the value too.</para>
        /// </summary>
        protected virtual string FormatPart(CodeProviderPart Part, string sValue)
        {
            string Result = sValue;
 

            if (!string.IsNullOrWhiteSpace(Part.Format) && !string.IsNullOrWhiteSpace(Result))
            {
                // set the length of the Result string to be equal to the length of the Format
                if (Result.Length > Part.Format.Length)
                    Result = Result.Remove(0, Result.Length - Part.Format.Length);

                int iValue = int.Parse(Result);

                // if it is the Pivot, then increase by one
                if (Part.Type == CodeProviderPartType.Pivot)
                    iValue++;

                Result = iValue.ToString();
 
                // length of the part, not counting characters other than ValidCodeFormatDigit, X by default
                int ValueLen = 0;
                foreach (char C in Part.Format)
                    if (C == ValidCodeFormatDigit)
                        ValueLen++;

                // pad
                if (Result.Length < ValueLen)
                    Result = Result.PadLeft(ValueLen, '0');


                // format
                StringBuilder SB = new StringBuilder(Part.Format);

                for (int i = 0; i < SB.Length; i++)
                {
                    if (SB[i] == ValidCodeFormatDigit)
                    {
                        SB[i] = Result[0];
                        Result = Result.Remove(0, 1);
                        if (Result.Length == 0)
                            break;
                    }
                }

                Result = SB.ToString();
            }

            return Result;
        }
        /// <summary>
        /// Replaces those placeholders by using the value of the <see cref=" DateTime.UtcNow"/> property.
        /// </summary>
        protected virtual string ReplaceDateDigits(string S)
        {
            if (S.IndexOf("YYYY", 0, StringComparison.InvariantCultureIgnoreCase) != -1)
                S = S.Replace("YYYY", DateTime.UtcNow.Year.ToString());
            else if (S.IndexOf("YY", 0, StringComparison.InvariantCultureIgnoreCase) != -1)
                S = S.Replace("YY", DateTime.UtcNow.Year.ToString().Remove(0, 2));

            if (S.IndexOf("MM", 0, StringComparison.InvariantCultureIgnoreCase) != -1)
                S = S.Replace("MM", DateTime.UtcNow.Month.ToString().PadLeft(2, '0'));

            if (S.IndexOf("DD", 0, StringComparison.InvariantCultureIgnoreCase) != -1)
                S = S.Replace("DD", DateTime.UtcNow.Day.ToString().PadLeft(2, '0'));

            return S;
        }
        /// <summary>
        /// Returns the value the Part represents by executing code according to the <see cref="CodeProviderPart.Type"/> of the Part.
        /// </summary>
        protected virtual string GetPartValue(CodeProviderPart Part)
        {
 
            CheckExecute(Part);

            string Result = string.Empty;
            string SqlText;

            switch (Part.Type)
            {
                case CodeProviderPartType.Literal:
                    Result = Part.Text;
                    break; 
                case CodeProviderPartType.NumericSelect:                     
                    SqlText = Part.Text.Replace("@TABLE_NAME", TableName);
                    Result = Select(SqlText);
                    break;
                case CodeProviderPartType.Date:
                    Result = ReplaceDateDigits(Part.Text);
                    break;
                case CodeProviderPartType.Sequencer:
                    Result = Store.NextIdByGenerator(Part.Text).ToString();
                    break;
                case CodeProviderPartType.Pivot:
                    SqlText = $"select max({Descriptor.CodeFieldName}) from {TableName}";
                    Result = Select(SqlText);

                    if (string.IsNullOrWhiteSpace(Result))
                    {
                        Result = "0";
                    }
                    else
                    {
                        if (Result.Length > Part.Format.Length)
                            Result = Result.Remove(0, Result.Length - Part.Format.Length);
                    }

                    // remove any non-digit characters, such as delimiters
                    StringBuilder SB = new StringBuilder();
                    foreach (char C in Result)
                        if (char.IsDigit(C))
                            SB.Append(C);

                    Result = SB.ToString();

                    break; 
            }

            return Result;
        }
        /// <summary>
        /// Processes a part and returns its string value.
        /// </summary>
        protected virtual string Execute(CodeProviderPart Part)
        {
            string Result = GetPartValue(Part);
            if (!string.IsNullOrWhiteSpace(Part.Format))
                Result = FormatPart(Part, Result);
            return Result;
        }

        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        private CodeProvider()
        {
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        public CodeProvider(string DescriptorName, string TableName)
            : this(FindDescriptor(DescriptorName), TableName)
        {
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        public CodeProvider(CodeProviderDef Descriptor, string TableName)
        {
            if (Descriptor == null)
                Sys.Throw($"{this.GetType().Name}: Descriptor is null");
            this.TableName = TableName;
            this.Descriptor = Descriptor;
            ParseDescriptor();
        }


        /* static */
        /// <summary>
        /// Returns a descriptor by a specified name if any, else, null
        /// </summary>
        static public CodeProviderDef FindDescriptor(string Name)
        {
            return Descriptors.Find(item => item.Name.IsSameText(Name));
        }
        /// <summary>
        /// Returns true if a descriptor is already registered under a specified name.
        /// </summary>
        static public bool DescriptorExists(string Name)
        {
            return FindDescriptor(Name) != null;
        }
        /// <summary>
        /// Registers a descriptor.
        /// </summary>
        static public void RegisterDescriptor(string Name, string Text)
        {
            CodeProviderDef Des = FindDescriptor(Name);
            if (Des != null)
            {
                Des.Text = Text;
            }                
            else
            {
                Descriptors.Add(new CodeProviderDef() { Name = Name, Text = Text });
            }
        }


        /* public */
        /// <summary>
        /// Constructs and returns the Code.
        /// <para>The code producer must be properly initialized.</para>
        /// </summary>
        public virtual string Execute(DataRow CurrentRow, SqlStore Store, DbTransaction Transaction = null)
        {
            this.CurrentRow = CurrentRow;
            this.Store = Store;
            this.Transaction = Transaction;

            try
            {
                StringBuilder SB = new StringBuilder();

                string sPart;
                foreach (var Part in Parts)
                {
                    if (SB.Length > 0)
                        SB.Append(Descriptor.PartSeparator);

                    sPart = Execute(Part);
                    SB.Append(sPart);
                }
                    

                return SB.ToString();
            }
            finally
            {
                CurrentRow = null;
                Store = null;
                Transaction = null;
            }
        }

        /* properties */
        /// <summary>
        /// The field name of the field to put the produced Code.
        /// </summary>
        public string CodeFieldName => Descriptor.CodeFieldName;
        /// <summary>
        /// The character used by code producers as placeholder for digits.
        /// </summary>
        static public char ValidCodeFormatDigit { get; set; } = 'X';

    }




 




}
