using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Tripous;
using Tripous.Data;

namespace WebDesk
{
    /// <summary>
    /// A module definition
    /// </summary>
    public class ModuleDef
    {
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
        public string TitleKey { get; set; }
        /// <summary>
        /// Icon key
        /// </summary>
        public string IconKey { get; set; }

        /// <summary>
        /// The author (company, person, etc) who created this instance
        /// </summary>
        public string Author { get; set; }
        /// <summary>
        /// Creation date-time
        /// </summary>
        public DateTime CreatedOn { get; set; }
        /// <summary>
        /// Modification date-time
        /// </summary>
        public DateTime ModifiedOn { get; set; }
    }

     
    /// <summary>
    /// Table definition
    /// </summary>
    public class DataTableDef
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public DataTableDef()
        {
        }

        /* public */
        /// <summary>
        /// Returns the table definition text.
        /// <para>WARNING: The returned text must be passed through <see cref="SqlProvider.ReplaceDataTypePlaceholders"/> method, for the final result.</para>
        /// </summary>
        public string GetDefText()
        {
            StringBuilder SB = new StringBuilder();

            SB.AppendLine($"create table {Name} (");
 
            string sDef;

            for (int i = 0; i < Fields.Count; i++)
            {
                sDef = i > 0 ? "," + Fields[i].GetDefText() : Fields[i].GetDefText();
                
                SB.AppendLine("  " + sDef);
            }

            for (int i = 0; i < UniqueConstraints.Count; i++)
            {
                sDef = $",constraint UC_{Name}_{i} unique ({UniqueConstraints[i].FieldName})";
                SB.AppendLine("  " + sDef);
            }

            for (int i = 0; i < ForeignKeys.Count; i++)
            {
                sDef = $",constraint FC_{Name}_{i} foreign key ({ForeignKeys[i].FieldName}) references {ForeignKeys[i].ForeignTableName} ({ForeignKeys[i].ForeignFieldName})";
                SB.AppendLine("  " + sDef);
            }

            SB.AppendLine(")");
            return SB.ToString();
        }

        /// <summary>
        ///  Creates, adds and returns a primary key field.
        /// </summary>
        public DataFieldDef AddPrimaryKey(string FieldName = "Id", string TitleKey = null)
        {
            DataFieldDef Result = new DataFieldDef();
            Result.Name = FieldName;
            Result.TitleKey = !string.IsNullOrWhiteSpace(TitleKey) ? TitleKey : FieldName;
            Result.IsPrimaryKey = true;
            Fields.Add(Result);
            return Result;
        }
        /// <summary>
        ///  Creates, adds and returns a string (nvarchar) field.
        /// </summary>
        public DataFieldDef AddStringField(string FieldName, int Length, bool NotNull, string TitleKey = null, string DefaultValue = null)
        {
            DataFieldDef Result = new DataFieldDef();
            Result.Name = FieldName;
            Result.TitleKey = !string.IsNullOrWhiteSpace(TitleKey) ? TitleKey : FieldName;
            Result.DataType = DataFieldType.String;
            Result.Length = Length;
            Result.NotNull = NotNull;
            Result.DefaultValue = DefaultValue;
            Fields.Add(Result);
            return Result;
        }
        /// <summary>
        ///  Creates, adds and returns a field.
        /// </summary>
        public DataFieldDef AddField(string FieldName,  DataFieldType DataType, bool NotNull, string TitleKey = null, string DefaultValue = null)
        {
            DataFieldDef Result = new DataFieldDef();
            Result.Name = FieldName;
            Result.TitleKey = !string.IsNullOrWhiteSpace(TitleKey) ? TitleKey : FieldName;
            Result.DataType = DataType; 
            Result.NotNull = NotNull;
            Result.DefaultValue = DefaultValue;
            Fields.Add(Result);
            return Result;
        }

        /// <summary>
        ///  Creates, adds and returns a unique constraint
        /// </summary>
        public UniqueConstraintDef AddUniqueConstraint(string FieldName)
        {
            UniqueConstraintDef Result = new UniqueConstraintDef();
            Result.FieldName = FieldName;
            UniqueConstraints.Add(Result);
            return Result;
        }
        /// <summary>
        ///  Creates, adds and returns a foreign key field constraint.
        /// </summary>
        public ForeignKeyDef AddForeignKeyConstraint(string FieldName, string ForeignTableName, string ForeignFieldName)
        {
            ForeignKeyDef Result = new ForeignKeyDef();
            Result.FieldName = FieldName;
            Result.ForeignTableName = ForeignTableName;
            Result.ForeignFieldName = ForeignFieldName;
            ForeignKeys.Add(Result);
            return Result;
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
        public string TitleKey { get; set; }

        /// <summary>
        /// Fields
        /// </summary>
        public List<DataFieldDef> Fields { get; set; } = new List<DataFieldDef>();
        /// <summary>
        /// UniqueConstraints
        /// </summary>
        public List<UniqueConstraintDef> UniqueConstraints { get; set; } = new List<UniqueConstraintDef>();
        /// <summary>
        /// ForeignKeys
        /// </summary>
        public List<ForeignKeyDef> ForeignKeys { get; set; } = new List<ForeignKeyDef>();
    }


    /// <summary>
    /// The data-type of a data field
    /// </summary>
    [Flags]
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
    /// Field definition
    /// </summary>
    public class DataFieldDef
    {

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
                case DataFieldType.TextBlob: return SqlProvider.CBLOB_TEXT;                   
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
                sDataType = SysConfig.PrimaryKeyStr;
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
              sNull = NotNull ? SqlProvider.CNOT_NULL : SqlProvider.CNULL; 

            string sDefault = !string.IsNullOrWhiteSpace(DefaultValue) ? $"default {DefaultValue}" : string.Empty;

            string Result = $"{Name} {sDataType} {sDefault} {sNull}";

            return Result;
        }

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
        public string TitleKey { get; set; }

        /// <summary>
        /// True when the field is a primary key
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// The data-type of the field. One of the <see cref="DataFieldType"/> constants.
        /// </summary>
        public DataFieldType DataType { get; set; }
        /// <summary>
        /// Field length. Applicable to varchar fields only.
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// True when the field is NOT nullable  
        /// </summary>
        public bool NotNull { get; set; }   // when true then produces 'not null'
        /// <summary>
        /// The default expression, if any. E.g. 0, or ''. Defaults to null.
        /// </summary>
        public string DefaultValue { get; set; } // e.g. produces default 0, or default ''
    }


    /// <summary>
    /// constraint UC_{TABLE_NAME}_00 unique (FIELD_NAME)
    /// </summary>
    public class UniqueConstraintDef
    {
        /// <summary>
        /// Database Id
        /// </summary>
        public string Id { get; set; } = Sys.GenId(true);
        /// <summary>
        /// The field name upon where the constraint is applied
        /// </summary>
        public string FieldName { get; set; }
    }

    /// <summary>
    /// constraint FC_{TABLE_NAME}_00 foreign key (FIELD_NAME) references FOREIGN_TABLE_NAME (FOREIGN_FIELD_NAME)
    /// </summary>
    public class ForeignKeyDef
    {
        /// <summary>
        /// Database Id
        /// </summary>
        public string Id { get; set; } = Sys.GenId(true);
        /// <summary>
        /// The field name upon where the constraint is applied
        /// </summary>
        public string FieldName { get; set; }
        /// <summary>
        /// The foreign table name
        /// </summary>
        public string ForeignTableName { get; set; }
        /// <summary>
        /// The foreign field name  
        /// </summary>
        public string ForeignFieldName { get; set; }
    }


}
