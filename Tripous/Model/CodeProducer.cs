/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
using Tripous.Data;



namespace Tripous.Model
{

    /// <summary>
    /// Produces unique Codes.
    /// <para>The <see cref="CodeProducer.Descriptor"/> describes the form of the Code. It provides information regarding
    /// the various <see cref="CodePart"/> parts that make the Code.</para>
    /// <para>The <see cref="CodePart.Mode"/> property determines the mode of the <see cref="CodePart.Text"/> property or each part.</para>
    /// <para>The <see cref="CodePart.Format"/> property determines the part formatting.</para>
    /// </summary>
    /// <remarks>
    /// <example> Here are some examples.
    /// <para></para>
    /// 1.
    /// <code>
    /// CurrentRow      : ID, CODE
    /// PivotFieldName  : CODE
    /// Parts 
    ///   A. FieldName  = CODE
    ///      Format     = [empty stirng] no formatting takes place
    ///      LookUp     = select max(CODE) from :@TABLE_NAME 
    /// </code>
    /// 2.
    /// <code>
    /// CurrentRow      : ID, CODE, COUNTRY_ID
    /// PivotFieldName  : CODE
    /// Parts
    ///    A. FieldName = CODE
    ///       Format    = -XX-XX
    ///       LookUp    = select max(CODE) from :@TABLE_NAME
    /// 
    ///    B. FieldName = COUNTRY_ID
    ///       Format    = XXX
    ///       LookUp    = select CODE from COUNTRY where ID = :COUNTRY_ID 
    /// </code>
    /// 3.
    /// <code>
    /// CurrentRow      : ID, CODE, CATEGORY_ID, PIVOT_ID
    /// PivotFieldName  : PIVOT_ID
    /// Parts
    ///    A. FieldName = PIVOT_ID
    ///       Format    = -XXXXXXX
    ///       LookUp    = select max(PIVOT_ID) from :@TABLE_NAME where CATEGORY_ID = :CATEGORY_ID
    /// 
    ///    B. FieldName = CATEGORY_ID
    ///       Format    = XXX
    ///       LookUp    = select CODE from CATEGORY where ID = :CATEGORY_ID            
    /// </code>
    /// 4.
    /// <code>
    /// CurrentRow      : ID, CODE, COMPANY_ID, YEAR_ID, DOC_ID, DSR_ID, PIVOT_ID
    /// PivotFieldName  : PIVOT_ID
    /// Parts
    ///    A. FieldName = PIVOT_ID
    ///       Format    = XXXXXXX
    ///       LookUp    = select max(PIVOT_ID) from :@TABLE_NAME
    ///                   where
    ///                         COMPANY_ID = :COMPANY_ID
    ///                     and YEAR_ID    = :YEAR_ID
    ///                     and DOC_ID     = :DOC_ID
    ///                     and DSR_ID     = :DSR_ID   
    ///                     
    ///    B. FieldName = DSR_ID
    ///       Format    = [empty stirng] no formatting takes place
    ///       LookUp    = select CODE from DOC_SERIES
    ///                   where
    ///                         ID      = :DSR_ID
    ///                     and DOC_ID  = :DOC_ID          
    /// </code>
    /// </example>
    /// </remarks>
    [TypeStoreItem]
    public class CodeProducer
    {
        /// <summary>
        /// Field
        /// </summary>
        protected CodeDescriptor fDescriptor = new CodeDescriptor();

        /// <summary>
        /// Field
        /// </summary>
        protected string fMainTableName;

        /* protected properties */
        /// <summary>
        /// Returns the Executor which is used to execute various SQL statements.
        /// <para>This property must have a valid value when mode is other than <see cref="CodePartMode.Literal"/></para>
        /// </summary>
        protected virtual SqlStore Store { get; set; }
        /// <summary>
        /// Returns the transactin which is used to execute various SQL statements
        /// </summary>
        protected virtual DbTransaction Transaction { get; set; }
        /// <summary>
        /// Returns the main table name.
        /// </summary>
        protected virtual string MainTableName { get { return string.IsNullOrEmpty(fMainTableName) ? "" : fMainTableName; } }
        /// <summary>
        /// Returns the current data row used to provide parameters when executing SQL statements.
        /// <para>This property must have a valid value when the mode is <see cref="CodePartMode.FieldName"/> or <see cref="CodePartMode.LookUpSql"/></para>
        /// </summary>
        protected virtual DataRow CurrentRow { get; set; }
        /// <summary>
        /// Gets the Prefix value. 
        /// </summary>
        protected virtual string Prefix { get; set; }
        /// <summary>
        /// Gets the pivot value.
        /// </summary>
        protected virtual int PivotValue { get; set; }


        /// <summary>
        /// Checks if this instance is efficiently initialized and so it is valid for a client to
        /// call the Execute() method. Throws exceptions if Executor or the CurrentRow is null
        /// and if the format of the various parts is not valid.
        /// </summary>
        protected void CheckExecute(CodePart Part)
        {
            if (((Part.Mode == CodePartMode.FieldName) || (Part.Mode == CodePartMode.LookUpSql) || (Part.Mode == CodePartMode.Sequencer)) && (Store == null))
                Sys.Error("{0}. Executor is null", this.GetType().Name);

            if (((Part.Mode == CodePartMode.FieldName) || (Part.Mode == CodePartMode.LookUpSql)) && (CurrentRow == null))
                Sys.Error("{0}. CurrentRow is null", this.GetType().Name);

            if (((Part.Mode == CodePartMode.FieldName) || (Part.Mode == CodePartMode.LookUpSql)) && (string.IsNullOrEmpty(MainTableName)))
                Sys.Error("{0}. MainTableName is empty", this.GetType().Name);

            if (!IsValidCodeFormat(Part.Format))
                Sys.Error("CodeProducer. Invalid character in Code Part format");

        }
        /// <summary>
        /// It is called by the <see cref="FormatPart"/> method when the <see cref="PivotValue"/>
        /// is less or equal to 0. Returns a valid initial pivot value, properly incremented.
        /// </summary>
        protected virtual int GetInitialPivotValue(int Value)
        {
            return Value <= 0 ? 1 : Value;
        }
        /// <summary>
        /// When the <see cref="CodePart.Mode"/> of a part is <see cref="CodePartMode.LookUpSql"/> it may
        /// contain date place holders, such as YYYY, YY, MM and DD.
        /// <para>This method replaces those placeholders by using the value of the <see cref="Sys.Today"/> property.</para>
        /// </summary>
        protected virtual string ReplaceDateDigits(string S)
        {
            if (S.IndexOf("YYYY", 0, StringComparison.InvariantCultureIgnoreCase) != -1)
                S = S.Replace("YYYY", Sys.Today.Year.ToString());
            else if (S.IndexOf("YY", 0, StringComparison.InvariantCultureIgnoreCase) != -1)
                S = S.Replace("YY", Sys.Today.Year.ToString().Remove(0, 2));

            if (S.IndexOf("MM", 0, StringComparison.InvariantCultureIgnoreCase) != -1)
                S = S.Replace("MM", Sys.Today.Month.ToString().PadLeft(2, '0'));

            if (S.IndexOf("DD", 0, StringComparison.InvariantCultureIgnoreCase) != -1)
                S = S.Replace("DD", Sys.Today.Day.ToString().PadLeft(2, '0'));

            return S;
        }
        /// <summary>
        /// Executes the S and returns a string value.
        /// <para>The S should be something like <c> select max(CODE) as RESULT from CUSTOMER</c> that is something that
        /// returns a single row and a single column.</para>
        /// <para>The method calls the Executor.Select() passing the <see cref="CurrentRow"/>.</para>
        /// </summary>
        protected virtual string Select(string SqlText)
        {
            DataTable Table;

            if (Transaction != null)
                Table = Store.Select(Transaction, SqlText, CurrentRow);
            else
                Table = Store.Select(SqlText, CurrentRow);

            if (Table.Rows.Count > 0)
                return Table.Rows[0].IsNull(0) ? "" : Table.Rows[0][0].ToString();
            else
                return "";
        }
        /// <summary>
        /// Returns the value the Part represents by executing code according to the <see cref="CodePart.Mode"/> of the Part.
        /// </summary>
        protected virtual string SelectPartValue(CodePart Part)
        {
            CheckExecute(Part);

            string Result = "";
            string S = "";

            switch (Part.Mode)
            {
                case CodePartMode.FieldName:
                    if (Part == Descriptor.Pivot)
                    {
                        S = string.Format(@"select max({0}) as RESULT from {1}", Part.Text, MainTableName);
                        if (!string.IsNullOrEmpty(Prefix))
                            S += string.Format(" where {0} like {1}", Part.Text, (Prefix + "%").QS());
                    }
                    else
                    {
                        S = string.Format(@"select {0} as RESULT from {1}", Part.Text, MainTableName);
                    }
                    Result = Select(S);
                    Result = string.IsNullOrEmpty(Result) ? "0" : Result;
                    break;

                case CodePartMode.LookUpSql:
                    S = Part.Text.Replace(":@TABLE_NAME", MainTableName);
                    Result = Select(S);
                    Result = string.IsNullOrEmpty(Result) ? "0" : Result;
                    break;

                case CodePartMode.Sequencer:
                    Result = Store.NextIdByGenerator(Part.Text).ToString();
                    break;



                case CodePartMode.Literal:
                    Result = ReplaceDateDigits(Part.Text);
                    break;
            }

            return Result;
        }
        /// <summary>
        /// Formats and returns S, which is the value returned by a <see cref="SelectPartValue"/> call,
        /// according to the Format of the Part. 
        /// <para>If the <see cref="CodePart.Mode"/> of the Part is not a <see cref="CodePartMode.Sequencer"/>, 
        /// and the Part is the pivot part, it increments the value too.</para>
        /// </summary>
        protected virtual string FormatPart(CodePart Part, string S)
        {
            string Result = S;

            if (!string.IsNullOrEmpty(Part.Format) && !string.IsNullOrEmpty(Result))
            {
                // set the length of the string to be equal to the length of the Format
                if (Result.Length > Part.Format.Length)
                    Result = Result.Remove(0, Result.Length - Part.Format.Length);

                // length of the part, not counting characters other than Db.ValidCodeFormatDigit, X by default
                int ValueLen = 0;
                foreach (char C in Part.Format)
                    if (C == ValidCodeFormatDigit)
                        ValueLen++;

                // if it is the Pivot, then increase by one
                if (Part == Descriptor.Pivot)
                {
                    // Sequencers already return an increased value
                    if (Part.Mode == CodePartMode.Sequencer)
                        PivotValue = int.Parse(Result);
                    else
                    {
                        S = "";

                        // remove non numeric charactes
                        foreach (char C in Result)
                            if (char.IsDigit(C))
                                S += C.ToString();

                        // pivot value increment
                        PivotValue = int.Parse(S);
                        if (PivotValue <= 0)
                            PivotValue = GetInitialPivotValue(PivotValue);
                        else
                            PivotValue++;
                        Result = PivotValue.ToString();
                    }
                }

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
        /// Processes a part and returns its string value.
        /// </summary>
        protected virtual string Execute(CodePart Part)
        {
            return FormatPart(Part, SelectPartValue(Part));
        }

        /* construction */
        /// <summary>
        /// Static constructor
        /// </summary>
        static CodeProducer()
        {
            SysCodeProducers.Register();
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        public CodeProducer()
        {
            PivotValue = int.MinValue;
        }
        /// <summary>
        /// Creates a CodeProducer base on CodeProducerClassType.
        /// </summary>
        static public CodeProducer Create(Type CodeProducerClassType, string MainTableName)
        {
            CodeProducer Result = CodeProducerClassType.Create() as CodeProducer;
            Result.fMainTableName = MainTableName;
            return Result;
        }
        /// <summary>
        /// Creates a CodeProducer. It uses the <see cref="Registry"/> and the Name, in order to locate
        /// the right code producer class type.
        /// </summary>
        static public CodeProducer Create(string Name, string MainTableName)
        {
            CodeProducer Result = null;

            if (Registry.CodeProducers.Contains(Name))
            {
                CodeDescriptor SourceDes = Registry.CodeProducers[Name];
                CodeDescriptor TypeDes = SourceDes.Clone() as CodeDescriptor;
                Result = TypeStore.Create(TypeDes.TypeClassName) as CodeProducer;
                if (Result != null)
                {
                    Result.Descriptor.Assign(TypeDes);
                    Result.fMainTableName = MainTableName;
                }
            }

            return Result;
        }


        


        /* public */
        /// <summary>
        /// Constructs and returns the Code.
        /// <para>The code producer must be properly initialized.</para>
        /// </summary>
        public virtual string Execute(DataRow CurrentRow, SqlStore Store, DbTransaction Transaction)
        {
            this.CurrentRow = CurrentRow;
            this.Store = Store;
            this.Transaction = Transaction;

            try
            {
                PivotValue = int.MinValue;
                Prefix = "";
                Prefix = Execute(Descriptor.Prefix);

                string S = "";
                foreach (CodePart Part in Descriptor)
                    S += Execute(Part);

                return Prefix + S + Execute(this.Descriptor.Pivot);
            }
            finally
            {
                CurrentRow = null;
                Store = null;
                Transaction = null;
            }
        }

        /* static */
        /// <summary>
        /// Throws an exception if Code doesn't respect Format.
        /// </summary>
        static public void CheckCode(string Format, string Code)
        {
            if (string.IsNullOrEmpty(Format) || string.IsNullOrEmpty(Code))
                Sys.Error("CodeProducer. Format or Code is null or empty");

            if (Format.Length != Code.Length)
                Sys.Error("CodeProducer. Format and Code are not equal in length");


            for (int i = 0; i < Format.Length; i++)
            {
                if (Array.IndexOf(ValidCodeFormatDelimiters, Format[i]) >= 0)
                    if (Code[i] != Format[i])
                        Sys.Error("CodeProducer. Invalid formatted code. Format {0}, Code {1}", Format, Code);
            }

        }
        /// <summary>
        /// Returns true if the Format is a valid to be used with a code producer
        /// </summary>
        static public bool IsValidCodeFormat(string Format)
        {
            if (!string.IsNullOrWhiteSpace(Format))
            {
                List<char> list = new List<char>(ValidCodeFormatDelimiters);
                list.Add(ValidCodeFormatDigit);

                foreach (char c in Format)
                    if (list.IndexOf(c) == -1)
                        return false;
            }

            return true;
        }

        /// <summary>
        /// Gets or sets the character used by code producers as placeholder for digits.
        /// </summary>
        static public char ValidCodeFormatDigit { get; set; } = 'X';

        /// <summary>
        /// Gets or sets tha array of valid characters for formating code producer values values.
        /// </summary>
        static public char[] ValidCodeFormatDelimiters { get; set; } = new char[] { '.', '-', '\\', '/', ' ' };

        /* properties */
        /// <summary>
        /// Gets or sets the descriptor of this code producer.
        /// <para>This property must be always properly initialized.</para>
        /// </summary>
        public CodeDescriptor Descriptor
        {
            get { return fDescriptor; }
            set { fDescriptor.Assign(value); }
        }

    }


}
