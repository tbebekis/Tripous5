using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data 
{


    /// <summary>
    /// Field definition
    /// </summary>
    public class DataFieldDef
    {
        string fTitleKey;
        bool fRequired;
        DataFieldType fDataType;
        int fLength;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public DataFieldDef()
        {
        }

        /* public */
        /// <summary>
        /// Returns the string representation of a field data type.
        /// </summary>
        static public string DataTypeToString(DataFieldType Type)
        {
            switch (Type)
            {
                case DataFieldType.String: return SqlProvider.CNVARCHAR;
                case DataFieldType.Integer: return "integer";
                case DataFieldType.Float: return SqlProvider.CFLOAT;
                case DataFieldType.Decimal: return SqlProvider.CDECIMAL;
                case DataFieldType.Date: return SqlProvider.CDATE;
                case DataFieldType.DateTime: return SqlProvider.CDATE_TIME;
                case DataFieldType.Boolean: return "integer";
                case DataFieldType.Blob: return SqlProvider.CBLOB;
                case DataFieldType.Memo: return SqlProvider.CBLOB_TEXT;
            }

            throw new ApplicationException($"DataType not supported in Field definition: {Type}");
        }

        /// <summary>
        /// Returns the string representation of a field data type.
        /// </summary>
        public string DataTypeToString()
        {
            return DataTypeToString(this.DataType);
        }
        /// <summary>
        /// Returns the field definition text.
        /// <para>WARNING: The returned text must be passed through <see cref="SqlProvider.ReplaceDataTypePlaceholders"/> method, for the final result.</para>
        /// </summary>
        public string GetDefText()
        {

            string sDataType;
            if (IsPrimaryKey)
            {
                sDataType = DataType == DataFieldType.String ? $"@NVARCHAR({Length})    @NOT_NULL primary key" : "@PRIMARY_KEY";
            }
            else if (DataType == DataFieldType.String)
            {
                sDataType = $"{SqlProvider.CNVARCHAR}({Length})";
            }
            else
            {
                sDataType = DataTypeToString(DataType);
            }

            string sNull = string.Empty;
            if (!IsPrimaryKey)
                sNull = Required ? SqlProvider.CNOT_NULL : SqlProvider.CNULL;

            string sDefault = !string.IsNullOrWhiteSpace(DefaultValue) ? $"default {DefaultValue}" : string.Empty;

            string Result = $"{Name} {sDataType} {sDefault} {sNull}";

            return Result;
        }

        /// <summary>
        /// Defines a foreign key upon this field. Returns this.
        /// </summary>
        public DataFieldDef SetForeign(string TableName, string FieldName = "Id")
        {
            this.ForeignTableName = TableName;
            this.ForeignFieldName = FieldName;
            return this;
        }
        /// <summary>
        /// Sets the value of a property and returns this instance.
        /// </summary>
        public DataFieldDef SetUnique(bool Value = true)
        {
            this.Unique = Value;
            return this;
        }
        /// <summary>
        /// Sets the value of a property and returns this instance.
        /// </summary>
        public DataFieldDef SetRequired(bool Value = true)
        {
            this.Required = Value;
            return this;
        }
        /// <summary>
        /// Sets the value of a property and returns this instance.
        /// </summary>
        public DataFieldDef SetDefaultValue(string Value = null)
        {
            this.DefaultValue = Value;
            return this;
        }
        /// <summary>
        /// Sets the value of a property and returns this instance.
        /// </summary>
        public DataFieldDef SetTitleKey(string Value = "")
        {
            this.TitleKey = Value;
            return this;
        }
        /// <summary>
        /// Sets the value of a property and returns this instance.
        /// </summary>
        public DataFieldDef SetLength(int Value = 0)
        {
            this.Length = Value;
            return this;
        }

        /* properties */
        /// <summary>
        /// Database Id
        /// </summary>
        public string Id { get; set; } = Sys.GenId(true);
        /// <summary>
        /// A name unique among all instances of this type
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Resource string key
        /// </summary>
        public string TitleKey
        {
            get { return !string.IsNullOrWhiteSpace(fTitleKey) ? fTitleKey : Name; }
            set { fTitleKey = value; }
        }

        /// <summary>
        /// True when the field is a primary key
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// The data-type of the field. One of the <see cref="DataFieldType"/> constants.
        /// </summary>
        public DataFieldType DataType
        {
            get
            {
                if (IsPrimaryKey)
                    return SysConfig.GuidOids ? DataFieldType.String : DataFieldType.Integer;

                return fDataType;
            }
            set { fDataType = value; }
        }
        /// <summary>
        /// Field length. Applicable to varchar fields only.
        /// </summary>
        public int Length
        {
            get
            {
                if (IsPrimaryKey && SysConfig.GuidOids)
                    return fLength <= 40 ? 40 : fLength;
                if (DataType == DataFieldType.String)
                    return fLength;
                return 0;
            }
            set { fLength = value; }

        }
        /// <summary>
        /// True when the field is NOT nullable 
        /// <para>NOTE: when true then produces 'not null'</para>
        /// </summary>
        public bool Required
        {
            get { return IsPrimaryKey ? true : fRequired; }
            set { fRequired = value; }
        }
        /// <summary>
        /// The default expression, if any. E.g. 0, or ''. Defaults to null.
        /// <para>NOTE:  e.g. produces default 0, or default '' </para>
        /// </summary>
        public string DefaultValue { get; set; }

        /// <summary>
        /// When true denotes a field upon which a unique constraint is applied
        /// </summary>
        public bool Unique { get; set; }

        /// <summary>
        /// When not empty is the name of a foreign table which this field references.
        /// <para>NOTE: Used in creating a foreign key constraint.</para>
        /// </summary>
        public string ForeignTableName { get; set; }
        /// <summary>
        /// When not empty is the name of a foreign field which this field references.  
        /// <para>NOTE: Used in creating a foreign key constraint.</para>
        /// </summary>
        public string ForeignFieldName { get; set; }
    }



}
