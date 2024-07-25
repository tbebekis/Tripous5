/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Text;
using System.Collections.Generic;

namespace Tripous.Data
{
    /// <summary>
    /// A group of S utility methods.
    /// </summary>
    static public class Sql
    {
        /// <summary>
        /// LineBreak
        /// </summary>
        static private readonly string LB = Environment.NewLine; // Line Break
        /// <summary>
        /// The spaces to be used when generating Sql statements
        /// </summary>
        public const int StatementDefaultSpaces = 30;

        /* Where type */
        /// <summary>
        /// Adds Value to sWhere with an AND i.e <code>... AND FIELDNAME = VALUE</code>
        /// </summary>
        static public void Where(ref string sWhere, string FieldName, bool Value)
        {
            Where(ref sWhere, FieldName, Value ? 1 : 0);
        }
        /// <summary>
        /// Adds Value to sWhere with an AND i.e <code>... AND FIELDNAME = VALUE</code>
        /// </summary>
        static public void Where(ref string sWhere, string FieldName, char Value)
        {
            Where(ref sWhere, FieldName, Value.ToString());
        }
        /// <summary>
        /// Adds Value to sWhere with an AND i.e <code>... AND FIELDNAME = VALUE</code>
        /// </summary>
        static public void Where(ref string sWhere, string FieldName, decimal Value)
        {
            Where(ref sWhere, FieldName, (double)Value);
        }
        /// <summary>
        /// Adds Value to sWhere with an AND i.e <code>... AND FIELDNAME = VALUE</code>
        /// </summary>
        static public void Where(ref string sWhere, string FieldName, short Value)
        {
            Where(ref sWhere, FieldName, (int)Value);
        }
        /// <summary>
        /// Adds Value to sWhere with an AND i.e <code>... AND FIELDNAME = VALUE</code>
        /// </summary>
        [CLSCompliant(false)]
        static public void Where(ref string sWhere, string FieldName, ushort Value)
        {
            Where(ref sWhere, FieldName, (int)Value);
        }
        /// <summary>
        /// Adds Value to sWhere with an AND i.e <code>... AND FIELDNAME = VALUE</code>
        /// </summary>
        [CLSCompliant(false)]
        static public void Where(ref string sWhere, string FieldName, uint Value)
        {
            Where(ref sWhere, FieldName, (int)Value);
        }
        /// <summary>
        /// Adds Value to sWhere with an AND i.e <code>... AND FIELDNAME = VALUE</code>
        /// </summary>
        static public void Where(ref string sWhere, string FieldName, long Value)
        {
            Where(ref sWhere, FieldName, (int)Value);
        }
        /// <summary>
        /// Adds Value to sWhere with an AND i.e <code>... AND FIELDNAME = VALUE</code>
        /// </summary>
        [CLSCompliant(false)]
        static public void Where(ref string sWhere, string FieldName, ulong Value)
        {
            Where(ref sWhere, FieldName, (int)Value);
        }
        /// <summary>
        /// Adds Value to sWhere with an AND i.e <code>... AND FIELDNAME = VALUE</code>
        /// </summary>
        static public void Where(ref string sWhere, string FieldName, string Value)
        {
            if (!string.IsNullOrWhiteSpace(Value))
            {
                if (sWhere.Length > 0)
                    sWhere = sWhere + " and ";

                sWhere = sWhere + string.Format(" {0} = '{1}' ", FieldName, Value) + LB;
            }
        }
        /// <summary>
        /// Adds Value to sWhere with an AND i.e <code>... AND FIELDNAME = VALUE</code>
        /// </summary>
        static public void Where(ref string sWhere, string FieldName, int Value)
        {
            if (sWhere.Length > 0)
                sWhere = sWhere + " and ";

            sWhere = sWhere + string.Format(" {0} = {1} ", FieldName, Value) + LB;
        }
        /// <summary>
        /// Adds Value to sWhere with an AND i.e <code>... AND FIELDNAME = VALUE</code>
        /// </summary>
        static public void Where(ref string sWhere, string FieldName, double Value)
        {
            if (sWhere.Length > 0)
                sWhere = sWhere + " and ";

            sWhere = sWhere + string.Format(" {0} = {1} ", FieldName, FloatSQL(Value)) + LB;
        }
        /// <summary>
        /// Adds Value to sWhere with an AND i.e <code>... AND FIELDNAME = VALUE</code>
        /// </summary>
        static public void Where(ref string sWhere, string FieldName, DateTime Value)
        {
                Where(ref sWhere, FieldName, Value.ToString("yyyy-MM-dd"));
        }
        /// <summary>
        /// Adds Value to sWhere with an AND i.e <code>... AND FIELDNAME = VALUE</code>
        /// <para>The Value is added only if is greater than zero.</para>
        /// </summary>
        static public void WhereID(ref string sWhere, string FieldName, int Value)
        {
            if (Value > 0)
                Where(ref sWhere, FieldName, Value);
        }

        /* Where from-to */
        /// <summary>
        /// Adds FromValue or ToValue or both to sWhere, if any of the values is not null,  with an AND 
        /// <para>i.e <code>... (FIELDNAME &gt;= FromValue) and (FIELDNAME &lt;= ToValue) </code></para>
        /// </summary>
        static public void Where(ref string sWhere, string FieldName, string FromValue, string ToValue)
        {
            if (!string.IsNullOrWhiteSpace(FromValue) || !string.IsNullOrWhiteSpace(ToValue))
            {
                if (sWhere.Length > 0)
                    sWhere = sWhere + " and ";

                if (!string.IsNullOrWhiteSpace(FromValue) && !string.IsNullOrWhiteSpace(ToValue))
                    sWhere = sWhere + string.Format(" ( {0} >= '{1}' and {0} <= '{2}') ", FieldName, FromValue, ToValue) + LB;
                else if (!string.IsNullOrWhiteSpace(FromValue))
                    sWhere = sWhere + string.Format("  {0} >= '{1}'  ", FieldName, FromValue) + LB;
                else if (!string.IsNullOrWhiteSpace(ToValue))
                    sWhere = sWhere + string.Format("  {0} <= '{1}'  ", FieldName, ToValue) + LB;
            }
        }
        /// <summary>
        /// Adds FromValue or ToValue or both to sWhere, if any of the values is not null,  with an AND 
        /// <para>i.e <code>... AND ((FIELDNAME &gt;= FromValue) AND (FIELDNAME &lt;= ToValue)) </code></para>
        /// </summary>
        static public void WhereFloat(ref string sWhere, string FieldName, object FromValue, object ToValue)
        {
            if ((FromValue != null) || (ToValue != null))
            {
                if (sWhere.Length > 0)
                    sWhere = sWhere + " and ";

                if ((FromValue != null) && (ToValue != null))
                    sWhere = sWhere + string.Format(" ( {0} >= {1} and {0} <= {2}) ", FieldName, FloatSQL((double)FromValue), FloatSQL((double)ToValue)) + LB;
                else if (FromValue != null)
                    sWhere = sWhere + string.Format("  {0} >= {1}  ", FieldName, FloatSQL((double)FromValue)) + LB;
                else if (ToValue != null)
                    sWhere = sWhere + string.Format("  {0} <= {1}  ", FieldName, FloatSQL((double)ToValue)) + LB;
            }
        }
        /// <summary>
        /// Adds FromValue or ToValue or both to sWhere, if any of the values is not null,  with an AND 
        /// <para>i.e <code>... AND ((FIELDNAME &gt;= FromValue) AND (FIELDNAME &lt;= ToValue)) </code></para>
        /// </summary>
        static public void WhereDate(ref string sWhere, string FieldName, object FromValue, object ToValue)
        {
            string sFromDate = "";
            if (FromValue != null)
                sFromDate = ((DateTime)FromValue).ToString("yyyy-MM-dd");

            string sToDate = "";
            if (ToValue != null)
                sToDate = ((DateTime)ToValue).ToString("yyyy-MM-dd");

            Where(ref sWhere, FieldName, sFromDate, sToDate);
        }
        /// <summary>
        /// Adds FromValue or ToValue or both to sWhere, if any of the values is not null,  with an AND 
        /// <para>i.e <code>... AND ((FIELDNAME &gt;= FromValue) AND (FIELDNAME &lt;= ToValue)) </code></para>
        /// </summary>
        static public void WhereDateMSSQL(ref string sWhere, string FieldName, object FromValue, object ToValue)
        {
            string sFromDate = "";
            if (FromValue != null)
                sFromDate = ((DateTime)FromValue).ToString("yyyy-MM-dd 00:00:00");

            string sToDate = "";
            if (ToValue != null)
                sToDate = ((DateTime)ToValue).ToString("yyyy-MM-dd 23:59:59");

            Where(ref sWhere, FieldName, sFromDate, sToDate);
        }
        /// <summary>
        /// Returns the DateTime Value properly formatted and quoted for Oracle
        /// </summary>
        static public string QSOracleDateTime(DateTime Value)
        {
            return string.Format("to_date('{0}', 'YYYY-MM-DD:HH24:MI:SS')", Value.ToString("yyyy-MM-dd HH:mm:ss"));
        }
        /// <summary>
        /// Adds FromValue or ToValue or both to sWhere, if any of the values is not null,  with an AND 
        /// <para>i.e <code>... AND ((FIELDNAME &gt;= FromValue) AND (FIELDNAME &lt;= ToValue)) </code></para>
        /// <para>Returns a string properly formatted and quoted for Oracle</para>
        /// </summary>
        static public void WhereDateOracle(ref string sWhere, string FieldName, object FromValue, object ToValue)
        {
            string sFromDate = "";
            if (FromValue != null)
                sFromDate = QSOracleDateTime(((DateTime)FromValue).StartOfDay());

            string sToDate = "";
            if (ToValue != null)
                sToDate = QSOracleDateTime(((DateTime)ToValue).EndOfDay());


            if (!string.IsNullOrWhiteSpace(sFromDate) || !string.IsNullOrWhiteSpace(sToDate))
            {
                if (sWhere.Length > 0)
                    sWhere = sWhere + " and ";

                if (!string.IsNullOrWhiteSpace(sFromDate) && !string.IsNullOrWhiteSpace(sToDate))
                    sWhere = sWhere + string.Format(" ( {0} >= {1} and {0} <= {2}) ", FieldName, sFromDate, sToDate) + LB;
                else if (!string.IsNullOrWhiteSpace(sFromDate))
                    sWhere = sWhere + string.Format("  {0} >= {1}  ", FieldName, sFromDate) + LB;
                else if (!string.IsNullOrWhiteSpace(sToDate))
                    sWhere = sWhere + string.Format("  {0} <= {1}  ", FieldName, sToDate) + LB;
            }


        }

        /* like */
        /// <summary>
        /// Adds Value to sWhere using LIKE, with an AND
        /// <para>i.e <code>... AND  (FIELDNAME LIKE VALUE) </code></para>
        /// <para>Value may or may not contain mask characters (%, ?, *).</para>
        /// </summary>
        static public void Like(ref string sWhere, string FieldName, string Value)
        {
            if (!string.IsNullOrWhiteSpace(Value))
            {
                if (sWhere.Length > 0)
                    sWhere = sWhere + " and ";

                sWhere = sWhere + string.Format(" {0} {1} ", FieldName, NormalizeMaskForce(Value)) + LB;
            }
        }
        /// <summary>
        /// Adds Value to sWhere using LIKE, with an AND
        /// <para>i.e <code>... AND  (FIELDNAME LIKE VALUE) </code></para>
        /// <para>Value may or may not contain mask characters (%, ?, *).</para>
        /// <para>WARNING: This version adds a % in front of Value, if not there</para>
        /// </summary>
        static public void Like2(ref string sWhere, string FieldName, string Value)
        {
            if (!string.IsNullOrWhiteSpace(Value))
            {
                if (sWhere.Length > 0)
                    sWhere = sWhere + " and ";

                sWhere = sWhere + string.Format(" {0} {1} ", FieldName, NormalizeMask2(Value)) + LB;
            }
        }
        /// <summary>
        /// Adds Value to sWhere using LIKE, with an OR
        /// <para>i.e <code>... OR  (FIELDNAME LIKE VALUE) </code></para>
        /// <para>Value may or may not contain mask characters (%, ?, *).</para>
        /// </summary>
        static public void LikeOr(ref string sWhere, string FieldName, string Value)
        {
            if (!string.IsNullOrWhiteSpace(Value))
            {
                if (sWhere.Length > 0)
                    sWhere = sWhere + " or ";

                sWhere = sWhere + string.Format(" {0} {1} ", FieldName, NormalizeMaskForce(Value)) + LB;
            }
        }
        /// <summary>
        /// Adds Value to sWhere using LIKE, with an OR
        /// <para>i.e <code>... OR  (FIELDNAME LIKE VALUE) </code></para>
        /// <para>Value may or may not contain mask characters (%, ?, *).</para>
        /// <para>WARNING: This version adds a % in front of Value, if not there</para>
        /// </summary>
        static public void LikeOr2(ref string sWhere, string FieldName, string Value)
        {
            if (!string.IsNullOrWhiteSpace(Value))
            {
                if (sWhere.Length > 0)
                    sWhere = sWhere + " or ";

                sWhere = sWhere + string.Format(" {0} {1} ", FieldName, NormalizeMask2(Value)) + LB;
            }
        }

        /* misc */
        /// <summary>
        /// Converts the double Value to a float string, valid for S, i.e. ensures that the decimal
        /// seperator is the point.
        /// </summary>
        static public string FloatSQL(double Value)
        {
            return Value.ToString().Replace(',', '.');
        }
        /// <summary>
        /// Normalizes Value for use in a LIKE clause. It returns a string as <code>LIKE 'VALUE%'</code>
        /// <para>Value may or may not contain mask characters (%, ?, *)</para>
        /// </summary>
        static public string NormalizeMaskForce(string Value)
        {
            if (Value != null)
            {
                Value = Value.Trim();

                if (Value.Length > 0)
                {
                    StringBuilder SB = new StringBuilder(Value);
                    SB.Replace('*', '%');
                    SB.Replace('?', '%');
                    Value = SB.ToString();

                    if (Value.IndexOf('%') != -1)
                        return string.Format(" like '{0}' ", Value);
                    else
                        return string.Format(" like '{0}' ", Value + @"%");

                }
            }

            return " like '__not__existed__' ";
        }
        /// <summary>
        /// Normalizes Value for use in a LIKE clause. It returns a string as <code>LIKE 'VALUE%'</code>
        /// <para>Value may or may not contain mask characters (%, ?, *)</para>
        /// </summary>
        static public string NormalizeMask(string Value)
        {
            if (Value != null)
            {
                Value = Value.Trim();

                if (Value.Length > 0)
                {
                    StringBuilder SB = new StringBuilder(Value);
                    SB.Replace('*', '%');
                    SB.Replace('?', '%');
                    Value = SB.ToString();

                    if (Value.IndexOf('%') != -1)
                        return string.Format(" like '{0}' ", Value);
                    else
                        return string.Format(" like '{0}' ", Value + @"%");

                }
            }

            return string.Empty;
        }
        /// <summary>
        /// Normalizes Value for use in a LIKE clause. It returns a string as <code>LIKE 'VALUE%'</code>
        /// <para>Value may or may not contain mask characters (%, ?, *)</para>
        /// <para>WARNING: This version adds a % in front of Value, if not there</para>
        /// </summary>
        static public string NormalizeMask2(string Value)
        {
            if (Value != null)
            {
                Value = Value.Trim();

                if (Value.Length > 0)
                {
                    StringBuilder SB = new StringBuilder(Value);
                    SB.Replace('*', '%');
                    SB.Replace('?', '%');
                    Value = SB.ToString();

                    if (Value.IndexOf('%') != -1)
                    {
                        if (!string.IsNullOrWhiteSpace(Value) && (Value[0] != '%'))
                            Value = "%" + Value;

                        return string.Format(" like '{0}' ", Value);
                    }
                    else
                    {
                        if (!string.IsNullOrWhiteSpace(Value) && (Value[0] != '%'))
                            Value = "%" + Value;

                        return string.Format(" like '{0}' ", Value + @"%");
                    }

                }
            }

            return string.Empty;

            /*  
                        string Result = Value;

                        if (Result.Length > 0)
                        {
                            StringBuilder SB = new StringBuilder(Result);
                            SB.Replace('*', '%');
                            SB.Replace('?', '%');
                            Result = SB.ToString();

                            if (Result.IndexOf('%') != -1)
                            {
                                if (!string.IsNullOrWhiteSpace(Result) && (Result[0] != '%'))
                                    Result = "%" + Result;

                                Result = string.Format(" like '{0}' ", Result);
                            }
                            else
                            {
                                if (!string.IsNullOrWhiteSpace(Result) && (Result[0] != '%'))
                                    Result = "%" + Result;

                                Result = string.Format(" like '{0}' ", Result + @"%");
                            }

                        }

                        return Result; 
             */
        }
        /// <summary>
        /// Returns true if Value contains any of the mask characters (%, ?, *)
        /// </summary>
        static public bool IsMasked(string Value)
        {
            if (string.IsNullOrWhiteSpace(Value))
                return false;

            if (Value.IndexOf('*') != -1)
                return true;

            if (Value.IndexOf('?') != -1)
                return true;

            if (Value.IndexOf('%') != -1)
                return true;

            return false;
        }

        /// <summary>
        /// Returns a string such as  
        /// <para><c>TableNameOrAlias.FieldName as Alias</c></para>
        /// </summary>
        static public string FormatFieldNameAlias(string TableNameOrAlias, string FieldName, string Alias, int Spaces)
        {
            if (string.IsNullOrWhiteSpace(FieldName))
                throw new ArgumentNullException("TableNameOrAlias");

            if (string.IsNullOrWhiteSpace(FieldName))
                throw new ArgumentNullException("FieldName");

            return FormatFieldNameAlias(TableNameOrAlias + "." + FieldName, Alias, Spaces);
        }
        /// <summary>
        /// Returns a string such as 
        /// <para><c>FieldName as Alias</c></para>
        /// </summary>
        static public string FormatFieldNameAlias(string FieldName, string Alias, int Spaces)
        {
            if (string.IsNullOrWhiteSpace(FieldName))
                throw new ArgumentNullException("FieldName");

            if (string.IsNullOrWhiteSpace(FieldName))
                throw new ArgumentNullException("Alias");

            return FieldName.PadRight(Spaces, ' ') + " as " + Alias + ", " + LB;
        }
        /// <summary>
        /// Returns a string such as 
        /// <para><c>TableName  Alias</c></para>
        /// </summary>
        static public string FormatTableNameAlias(string TableName, string Alias)
        {
            if (string.IsNullOrWhiteSpace(TableName))
                throw new ArgumentNullException("TableName");

            if (string.IsNullOrWhiteSpace(Alias) || Sys.IsSameText(TableName, Alias))
                return TableName;
            else
                return TableName + " " + Alias;
        }
        /// <summary>
        /// Transforms a string list into a field list by adding a <c>,</c> and a line break in every string item, except the last one.
        /// </summary>
        static public string TransformToFieldList(List<string> StringList)
        {
            string Result = string.Join(", " + Environment.NewLine, StringList.ToArray()).TrimEnd();
            return Result;
        }
 

        /// <summary>
        /// Formats Value. Quotes the result if Quoted is true.
        /// </summary>
        static public string DateToStr(DateTime Value, bool Quoted)
        {
            if (Quoted)
                return Value.ToString("yyyy-MM-dd").QS();
            else
                return Value.ToString("yyyy-MM-dd");
        }
        /// <summary>
        /// Formats Value. Quotes the result if Quoted is true.
        /// </summary>
        static public string TimeToStr(DateTime Value, bool Quoted)
        {
            if (Quoted)
                return Value.ToString("HH:mm").QS();
            else
                return Value.ToString("HH:mm");
        }
        /// <summary>
        /// Formats Value. Quotes the result if Quoted is true.
        /// </summary>
        static public string DateTimeToStr(DateTime Value, bool Quoted)
        {
            if (Quoted)
                return Value.ToString("yyyy-MM-dd HH:mm").QS();
            else
                return Value.ToString("yyyy-MM-dd HH:mm");
        }

        /// <summary>
        /// Returns the value of the Value as a string for constructing Sql statements.
        /// <para>In case of a string or DateTime it surrounds the result with single quotes.</para>
        /// </summary>
        static public string Format(object Value)
        {
            if (Value != null)
            {
                SimpleType simpleType = Simple.SimpleTypeOf(Value);

                if (simpleType.IsString())
                    return Value.ToString().QS();

                if (simpleType.IsInteger())
                    return Value.ToString();

                if (simpleType.IsFloat())
                    return FloatSQL((double)Value);

                if (simpleType.IsDateStrict())
                    return DateToStr((DateTime)Value, true);

                if (simpleType.IsTimeStrict())
                    return TimeToStr((DateTime)Value, true);

                if (simpleType.IsDateTimeStrict())
                    return DateTimeToStr((DateTime)Value, true);

                if (simpleType.IsDateTime())
                    return DateToStr((DateTime)Value, true);

                return Value.ToString();
            }

            return string.Empty;
        }
        /// <summary>
        /// Returns the value of the Id as a string for constructing Sql statements.
        /// <para>Id could be either a guid string or a 32-bit integer.</para>
        /// </summary>
        static public string FormatId(object Id)
        {
            /* null */
            if (Sys.IsNull(Id))
            {
                return "null";
            }

            string S = Id.ToString();

            /* int */
            int Num;
            if (int.TryParse(S, out Num))
            {
                return S;
            }

            /* string */
            return S.QS();

        }

        /// <summary>
        /// If the specified CreateTableSqlText is a CREATE TABLE TABLE_NAME (...) sql statement
        /// then this method returns the TABLE_NAME, else returns string.Empty
        /// </summary>
        static public string ExtractTableName(string CreateTableSqlText)
        {

            if (!string.IsNullOrWhiteSpace(CreateTableSqlText))
            {
                int Index = CreateTableSqlText.IndexOf('(');

                if (Index != -1)
                {
                    string SqlText = CreateTableSqlText.Substring(0, Index);

                    SqlText = SqlText.Trim();
                    SqlText = SqlText.Replace("  ", " ");

                    string[] Words = SqlText.Split(' ');

                    if ((Words.Length >= 3) && Sys.IsSameText(Words[0], "create") && Sys.IsSameText(Words[1], "table"))
                    {
                        return Words[2];
                    }
                }
            }

            return string.Empty;
        }
    }
}
