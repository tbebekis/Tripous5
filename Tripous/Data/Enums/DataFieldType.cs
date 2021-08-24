﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Tripous.Data
{

    /// <summary>
    /// The data-type of a data field
    /// </summary>
    [Flags]
    [TypeStoreItem]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum DataFieldType
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// String (nvarchar, varchar)
        /// </summary>
        String = 1,
        /// <summary>
        /// Integer
        /// </summary>
        Integer = 2,
        /// <summary>
        /// Float (float, double precision, etc)
        /// </summary>
        Float = 4,
        /// <summary>
        /// Decimal (decimal(18, 4))
        /// </summary>
        Decimal = 8,
        /// <summary>
        /// Date (date)
        /// </summary>
        Date = 0x10,
        /// <summary>
        /// DateTime (datetime, timestamp, etc)
        /// </summary>
        DateTime = 0x20,
        /// <summary>
        /// Boolean (integer always, 1 = true, else false)
        /// </summary>
        Boolean = 0x40,
        /// <summary>
        /// Blob
        /// </summary>
        Blob = 0x80,
        /// <summary>
        /// Text Blob
        /// </summary>
        TextBlob = 0x100,
    }


    /// <summary>
    /// Helper
    /// </summary>
    static public class DataFieldTypeHelper
    {

        /// <summary>
        /// Returns the <see cref="DataFieldType"/> corresponding to a <see cref="Type"/>.
        /// </summary>
        static public DataFieldType DataFieldTypeOf(Type T)
        {
            if (T != null)
            {
                TypeCode Code = System.Type.GetTypeCode(T);

                if ((T == typeof(byte[]) || (T == typeof(object))))
                {
                    return DataFieldType.Blob;
                }
                else
                {
                    switch (Code)
                    {
                        case TypeCode.Boolean: return DataFieldType.Boolean;
                        case TypeCode.Char: return DataFieldType.String;
                        case TypeCode.SByte: return DataFieldType.Integer;
                        case TypeCode.Byte: return DataFieldType.Integer;
                        case TypeCode.Int16: return DataFieldType.Integer;
                        case TypeCode.UInt16: return DataFieldType.Integer;
                        case TypeCode.Int32: return DataFieldType.Integer;
                        case TypeCode.UInt32: return DataFieldType.Integer;
                        case TypeCode.Int64: return DataFieldType.Integer;
                        case TypeCode.UInt64: return DataFieldType.Integer;
                        case TypeCode.Single: return DataFieldType.Float;
                        case TypeCode.Double: return DataFieldType.Float;
                        case TypeCode.Decimal: return DataFieldType.Decimal;
                        case TypeCode.DateTime: return DataFieldType.DateTime;
                        case TypeCode.String: return DataFieldType.String;
                    }
                }
            }

            return DataFieldType.Unknown;
        }


        /// <summary>
        /// True if Value is DateTime or Date or Time
        /// </summary>
        static public bool IsDateTime(this DataFieldType Value)
        {
            return Value == DataFieldType.DateTime || Value == DataFieldType.Date;  
        }
        /// <summary>
        /// Returns true if Value is strictly DateTime
        /// </summary>
        static public bool IsDateTimeStrict(this DataFieldType Value)
        {
            return Value == DataFieldType.DateTime;
        }
        /// <summary>
        /// Returns true if Value is strictly Date
        /// </summary>
        static public bool IsDateStrict(this DataFieldType Value)
        {
            return Value == DataFieldType.Date;
        }
        /// <summary>
        /// True if Value is Memo or Graphic or Blob
        /// </summary>
        static public bool IsBlob(this DataFieldType Value)
        {
            return Value == DataFieldType.Blob || Value == DataFieldType.TextBlob;  
        }

        /// <summary>
        /// Gets the Type value of Value
        /// </summary>
        static public Type GetNetType(this DataFieldType Value)
        {
            switch (Value)
            {
                case DataFieldType.String: return typeof(System.String);
                case DataFieldType.Integer: return typeof(System.Int32);
                case DataFieldType.Float: return typeof(System.Double);
                case DataFieldType.Decimal: return typeof(System.Decimal);
                case DataFieldType.Date: return typeof(System.DateTime);
                case DataFieldType.DateTime: return typeof(System.DateTime);
                case DataFieldType.Boolean: return typeof(System.Boolean);
                case DataFieldType.Blob: return typeof(System.Object);
                case DataFieldType.TextBlob: return typeof(System.Object);
            }

            return null; 


        }

    }

}
