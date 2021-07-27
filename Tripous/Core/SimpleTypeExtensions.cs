/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Data;

namespace Tripous
{

    /// <summary>
    /// Extensions
    /// </summary>
    static public class Simple
    {
        /* type convertion */
        /// <summary>
        /// Gets the Type value of Value
        /// </summary>
        static public Type GetNetType(this SimpleType Value)
        {
            switch (Value)
            {
                case SimpleType.String: return typeof(System.String);
                case SimpleType.WideString: return typeof(System.String);
                case SimpleType.Integer: return typeof(System.Int32);
                case SimpleType.Boolean: return typeof(System.Boolean);
                case SimpleType.Float: return typeof(System.Double);
                case SimpleType.Currency: return typeof(System.Decimal);
                case SimpleType.Date: return typeof(System.DateTime);
                case SimpleType.Time: return typeof(System.DateTime);
                case SimpleType.DateTime: return typeof(System.DateTime);
                case SimpleType.Memo: return typeof(System.Object);
                case SimpleType.Graphic: return typeof(System.Object);
                case SimpleType.Blob: return typeof(System.Object);
                case SimpleType.Object: return typeof(System.Object);
                case SimpleType.Interface: return typeof(System.Object);
                case SimpleType.Enum: return typeof(System.Enum);
            }

            return null; // Container = PrimitiveType.Container


        }
        /// <summary>
        /// Gets the simple type of Value
        /// </summary>
        static public SimpleType SimpleTypeOf(Type Value)
        {
            if (Value != null)
            {
                if (Value.IsEnum)
                    return SimpleType.Enum;

                TypeCode Code = System.Type.GetTypeCode(Value);

                switch (Code)
                {
                    case TypeCode.Empty: return SimpleType.None;
                    case TypeCode.Object: return SimpleType.Object;
                    case TypeCode.DBNull: return SimpleType.None;
                    case TypeCode.Boolean: return SimpleType.Boolean;
                    case TypeCode.Char: return SimpleType.None;
                    case TypeCode.SByte: return SimpleType.Integer;
                    case TypeCode.Byte: return SimpleType.Integer;
                    case TypeCode.Int16: return SimpleType.Integer;
                    case TypeCode.UInt16: return SimpleType.Integer;
                    case TypeCode.Int32: return SimpleType.Integer;
                    case TypeCode.UInt32: return SimpleType.Integer;
                    case TypeCode.Int64: return SimpleType.Integer;
                    case TypeCode.UInt64: return SimpleType.Integer;
                    case TypeCode.Single: return SimpleType.Float;
                    case TypeCode.Double: return SimpleType.Float;
                    case TypeCode.Decimal: return SimpleType.Currency;
                    case TypeCode.DateTime: return SimpleType.DateTime;
                    case TypeCode.String: return SimpleType.String;
                }
            }

            return SimpleType.None;
        }
        /// <summary>
        /// Gets the simple type of Value
        /// </summary>
        static public SimpleType SimpleTypeOf(DbType Value)
        {

            switch (Value)
            {
                case DbType.AnsiString: return SimpleType.String;
                case DbType.Binary: return SimpleType.Blob;
                case DbType.Byte: return SimpleType.Integer;
                case DbType.Boolean: return SimpleType.Boolean;
                case DbType.Currency: return SimpleType.Float;
                case DbType.Date: return SimpleType.Date;
                case DbType.DateTime: return SimpleType.DateTime;
                case DbType.Decimal: return SimpleType.Float;
                case DbType.Double: return SimpleType.Float;
                case DbType.Guid: return SimpleType.String;
                case DbType.Int16: return SimpleType.Integer;
                case DbType.Int32: return SimpleType.Integer;
                case DbType.Int64: return SimpleType.Integer;
                case DbType.Object: return SimpleType.Blob;
                case DbType.SByte: return SimpleType.Integer;
                case DbType.Single: return SimpleType.Float;
                case DbType.String: return SimpleType.String;
                case DbType.Time: return SimpleType.Time;
                case DbType.UInt16: return SimpleType.Integer;
                case DbType.UInt32: return SimpleType.Integer;
                case DbType.UInt64: return SimpleType.Integer;
                case DbType.VarNumeric: return SimpleType.Blob;
                case DbType.AnsiStringFixedLength: return SimpleType.String;
                case DbType.StringFixedLength: return SimpleType.String;
                case DbType.Xml: return SimpleType.Blob;
                case DbType.DateTime2: return SimpleType.DateTime;
                case DbType.DateTimeOffset: return SimpleType.DateTime;
            }

            return SimpleType.None;

        }



        /// <summary>
        /// Gets the simple type of Value. If it is null or DbNull, then SimpleType.None is returned.
        /// </summary>
        static public SimpleType SimpleTypeOf(object Value)
        {
            if ((Value == null) || (DBNull.Value == Value))
                return SimpleType.None;

            return Simple.SimpleTypeOf(Value.GetType());
        }
        /// <summary>
        /// Gets the simple type of the character Value
        /// </summary>
        static public SimpleType SimpleTypeOf(char Value)
        {
            switch (char.ToUpper(Value))
            {
                case 'S': return SimpleType.String;
                case 'W': return SimpleType.WideString;
                case 'I': return SimpleType.Integer;
                case 'L': return SimpleType.Boolean;
                case 'F': return SimpleType.Float;
                case 'C': return SimpleType.Currency;
                case 'D': return SimpleType.Date;
                case 'T': return SimpleType.Time;
                case 'M': return SimpleType.DateTime;
                case 'X': return SimpleType.Memo;
                case 'G': return SimpleType.Graphic;
                case 'B': return SimpleType.Blob;
                case 'O': return SimpleType.Object;
                case 'U': return SimpleType.Interface;
                case 'E': return SimpleType.Enum;
            }

            return SimpleType.None;
        }
        /// <summary>
        /// Gets the char value of Value
        /// </summary>
        static public char ToChar(this SimpleType Value)
        {
            switch (Value)
            {
                case SimpleType.String: return 'S';
                case SimpleType.WideString: return 'W';
                case SimpleType.Integer: return 'I';
                case SimpleType.Boolean: return 'L';
                case SimpleType.Float: return 'F';
                case SimpleType.Currency: return 'C';
                case SimpleType.Date: return 'D';
                case SimpleType.Time: return 'T';
                case SimpleType.DateTime: return 'M';
                case SimpleType.Memo: return 'X';
                case SimpleType.Graphic: return 'G';
                case SimpleType.Blob: return 'B';
                case SimpleType.Object: return 'O';
                case SimpleType.Interface: return 'U';
                case SimpleType.Enum: return 'E';
            }

            return 'N';
        }



        /* IsXXXXX methods */
        /// <summary>
        /// True if Value is String or WideString string
        /// </summary>
        static public bool IsString(this SimpleType Value)
        {
            return (Value & (SimpleType.String | SimpleType.WideString)) != SimpleType.None;
        }
        /// <summary>
        /// True if Value is Boolean
        /// </summary>
        static public bool IsBoolean(this SimpleType Value)
        {
            return Value == SimpleType.Boolean;
        }

        /// <summary>
        /// True if Value is DateTime or Date or Time
        /// </summary>
        static public bool IsDateTime(this SimpleType Value)
        {
            return (Value & (SimpleType.DateTime | SimpleType.Date | SimpleType.Time)) != SimpleType.None;
        }
        /// <summary>
        /// Returns true if Value is strictly DateTime
        /// </summary>
        static public bool IsDateTimeStrict(this SimpleType Value)
        {
            return (Value == SimpleType.DateTime);
        }
        /// <summary>
        /// Returns true if Value is strictly Date
        /// </summary>
        static public bool IsDateStrict(this SimpleType Value)
        {
            return (Value == SimpleType.Date);
        }
        /// <summary>
        /// Returns true if Value is strictly Time
        /// </summary>
        static public bool IsTimeStrict(this SimpleType Value)
        {
            return (Value == SimpleType.Time);
        }
        /// <summary>
        /// Returns true if Value is Integer
        /// </summary>
        static public bool IsInteger(this SimpleType Value)
        {
            return (Value == SimpleType.Integer);
        }
        /// <summary>
        /// True if Value is Float or Currency 
        /// </summary>
        static public bool IsFloat(this SimpleType Value)
        {
            return (Value & (SimpleType.Float | SimpleType.Currency)) != SimpleType.None;
        }
        /// <summary>
        /// Returns true if Value is Float, Currency or Integer
        /// </summary>
        static public bool IsNumeric(this SimpleType Value)
        {
            return (Value.IsFloat()) || (Value == SimpleType.Integer);
        }
        /// <summary>
        /// True if Value is Memo or Graphic or Blob
        /// </summary>
        static public bool IsBlob(this SimpleType Value)
        {
            return (Value & (SimpleType.Memo | SimpleType.Graphic | SimpleType.Blob)) != SimpleType.None;
        }

        /// <summary>
        /// True if Value is String or WideString string
        /// </summary>
        static public bool IsString(Type Value)
        {
            return Simple.SimpleTypeOf(Value).IsString();
        }
        /// <summary>
        /// True if Value is Boolean
        /// </summary>
        static public bool IsBoolean(Type Value)
        {
            return Simple.SimpleTypeOf(Value).IsBoolean();
        }
        /// <summary>
        /// True if Value is DateTime or Date or Time
        /// </summary>
        static public bool IsDateTime(Type Value)
        {
            return Simple.SimpleTypeOf(Value).IsDateTime();
        }
        /// <summary>
        /// Returns true if Value is Integer
        /// </summary>
        static public bool IsInteger(Type Value)
        {
            return Simple.SimpleTypeOf(Value).IsInteger();
        }
        /// <summary>
        /// True if Value is Float or Currency 
        /// </summary>
        static public bool IsFloat(Type Value)
        {
            return Simple.SimpleTypeOf(Value).IsFloat();
        }
        /// <summary>
        /// Returns true if Value is Float, Currency or Integer
        /// </summary>
        static public bool IsNumeric(Type Value)
        {
            return Simple.SimpleTypeOf(Value).IsNumeric();
        }
        /// <summary>
        /// True if Value is Memo or Graphic or Blob
        /// </summary>
        static public bool IsBlob(Type Value)
        {
            return Simple.SimpleTypeOf(Value).IsBlob();
        }

        /* miscs 
        /// <summary>
        /// Returns a string array of "db type names" for the Value
        /// </summary>
        static public string[] ColumnTypes(this SimpleType Value)
        {
            switch (Value)
            {
                case SimpleType.String: return new string[] { "string", "nvarchar", "nchar", "varchar", "char" };
                case SimpleType.WideString: return new string[] { "widestring", "widechar", "unicode", "bstr", "olestr", "olestring" };
                case SimpleType.Integer: return new string[] { "integer", "int", "larginteger", "largint", "smallint", "autoinc", "autoincrement", "identity", "counter" };
                case SimpleType.Boolean: return new string[] { "boolean", "bit", "logical" };
                case SimpleType.Float: return new string[] { "float", "double", "extended", "real", "BCD", "FBCD" };
                case SimpleType.Currency: return new string[] { "currency", "money" };
                case SimpleType.Date: return new string[] { "date" };
                case SimpleType.Time: return new string[] { "time" };
                case SimpleType.DateTime: return new string[] { "datetime", "timestamp" };
                case SimpleType.Memo: return new string[] { "memo", "text", "clob" };
                case SimpleType.Graphic: return new string[] { "graphic", "image" };
                case SimpleType.Blob: return new string[] { "blob", "bin", "binary" };
                case SimpleType.Object: return new string[] { "object", "ref", "reference", "byref" };
                case SimpleType.Interface: return new string[] { "interface", "IUnknown", "IDispatch", "IInterface" };
                //case SimpleType.Enum: return 'E';
            }

            return new string[0];
        }
*/
    }
}
