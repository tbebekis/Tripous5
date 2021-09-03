﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data 
{
    /// <summary>
    /// Table definition
    /// </summary>
    public class DataTableDef
    {
        string fTitleKey;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public DataTableDef()
        {
        }


        /// <summary>
        /// Validates a specified database object identifier an throws an exception if it is invalid.
        /// </summary>
        static public void CheckIsValidDatabaseObjectIdentifier(string Name)
        {
            void Throw(string Text)
            {
                Sys.Throw($"{Name} is not a valid database object identifier. {Text}");
            }

            if (string.IsNullOrWhiteSpace(Name))
                Throw("Empty or null string");
 
            if (!(char.IsLetter(Name[0]) || Name[0] == '_'))
                Throw("First character should be a letter or _");

            if (Name.Contains(' '))
                Throw("Spaces not allowed");

            foreach (char C in Name)
            {
                if (!(char.IsLetterOrDigit(C) || C == '$' || C == '_'))
                    Throw("Special characters, except $ and _ are not allowed");
            }

            if (Name.Length > 32)
                Throw("Exceeds 32 character length");

        }

        /* public */
        /// <summary>
        /// Throws an exception if this instance is not a valid one.
        /// </summary>
        public void Check()
        {
             
            void Throw(string Text)
            {
                Sys.Throw($"Definition of {Name} table is invalid. {Text}");
            }

            // table            
            if (string.IsNullOrWhiteSpace(Name))
                Sys.Throw($"Table definition without Name");

            CheckIsValidDatabaseObjectIdentifier(Name);


            // fields
            if (Fields == null || Fields.Count == 0)
                Throw($"No fields.");

            Fields.ForEach(item => CheckIsValidDatabaseObjectIdentifier(item.Name));

            int Count = Fields.Count(item => item.IsPrimaryKey);
            //if (Count == 0)
            //    Throw($"No primary key.");

            if (Count > 1)
                Throw($"Compound primary keys not supported.");

            DataFieldDef F = Fields.FirstOrDefault(item => item.IsPrimaryKey);
            if (!(F.DataType == DataFieldType.String || F.DataType == DataFieldType.Integer))
                Throw($"Not supported data-type for primary key: {F.DataType.ToString()}");

            Count = Fields.Count(item => item.DataType == DataFieldType.Unknown);
            if (Count > 1)
                Throw($"Fields without data type.");

            Count = Fields.Count(item => string.IsNullOrWhiteSpace(item.Name));
            if (Count > 1)
                Throw($"Fields without Name.");

            Fields.ForEach((item) => {
                bool Flag = (string.IsNullOrWhiteSpace(item.ForeignTableName) && string.IsNullOrWhiteSpace(item.ForeignFieldName))
                            || (!string.IsNullOrWhiteSpace(item.ForeignTableName) && !string.IsNullOrWhiteSpace(item.ForeignFieldName));

                if (!Flag)
                    Throw($"Foreign Key constraint not fully defined in Field: {item.Name}");
            });

            // unique constraints
           // Count = UniqueConstraints.Count(item => string.IsNullOrWhiteSpace(item.FieldName));
           // if (Count > 1)
           //     Throw($"Unique constraints without FieldName.");

            // foreign key constraints
            //Count = ForeignKeys.Count(item => string.IsNullOrWhiteSpace(item.FieldName) || string.IsNullOrWhiteSpace(item.ForeignTableName) || string.IsNullOrWhiteSpace(item.ForeignFieldName));
            //if (Count > 1)
            //    Throw($"Foreign key constraints not fully defined.");
        }
        /// <summary>
        /// Returns the table definition text.
        /// <para>WARNING: The returned text must be passed through <see cref="SqlProvider.ReplaceDataTypePlaceholders"/> method, for the final result.</para>
        /// </summary>
        public string GetDefText()
        {
            string EnsureValidLength(string S)
            {
                if (!string.IsNullOrWhiteSpace(S))
                {
                    if (S.Length > 32)
                        S = S.Substring(0, 32);
                }

                return S;
            }

            StringBuilder SB = new StringBuilder();

            SB.AppendLine($"create table {Name} (");

            string sDef;

            for (int i = 0; i < Fields.Count; i++)
            {
                sDef = i > 0 ? "," + Fields[i].GetDefText() : Fields[i].GetDefText();

                SB.AppendLine("  " + sDef);
            }


            // unique constraints
            string sName;
            int Index = 0;
            foreach (DataFieldDef Field in Fields)
            {
                if (Field.Unique)
                {
                    sName = EnsureValidLength($"UC{Index}_{Name}");
                    sDef = $",constraint {sName} unique ({Field.Name})";
                    SB.AppendLine("  " + sDef);
                    Index++;
                }
            }

            // foreign key constraints
            Index = 0;
            foreach (DataFieldDef Field in Fields)
            {
                if (!string.IsNullOrWhiteSpace(Field.ForeignTableName) && !string.IsNullOrWhiteSpace(Field.ForeignFieldName))
                {
                    sName = EnsureValidLength($"FC{Index}_{Name}");
                    sDef = $",constraint {sName} foreign key ({Field.Name}) references {Field.ForeignTableName} ({Field.ForeignFieldName})";
                    SB.AppendLine("  " + sDef);
                    Index++;
                }
            }

 
            SB.AppendLine(")");
            return SB.ToString();
        }
        
        /// <summary>
        /// Saves this instance to a <see cref="SysDataItem"/>
        /// </summary>
        public SysDataItem ToSysDataItem(string Owner = "")
        {
            SysDataItem Result = new SysDataItem();
            ToSysDataItem(Result, Owner);
            return Result;
        }
        /// <summary>
        /// Saves this instance to a <see cref="SysDataItem"/>
        /// </summary>
        public void ToSysDataItem(SysDataItem Dest, string Owner = "")
        {
            Check();

            Dest.Clear();

            Dest.DataType = "Table";
            Dest.DataName = this.Name;
            Dest.Owner = Owner;

            Dest.Data1 = Json.Serialize(this);
        }

        /// <summary>
        ///  Creates, adds and returns a primary key field.
        /// </summary>
        public DataFieldDef AddPrimaryKey(string FieldName = "Id", int Length = 40, string TitleKey = null)
        {
            DataFieldDef Result = new DataFieldDef();
            Result.Name = FieldName;
            Result.TitleKey = !string.IsNullOrWhiteSpace(TitleKey) ? TitleKey : FieldName;
            Result.IsPrimaryKey = true;
            Result.DataType = SysConfig.GuidOids ? DataFieldType.String : DataFieldType.Integer;
            Result.Length = Length;
            Result.Required = true;
            Fields.Add(Result);
            return Result;
        }

        /// <summary>
        ///  Creates, adds and returns a field.
        /// </summary>
        public DataFieldDef AddField(string FieldName, DataFieldType DataType, bool Required = false, string TitleKey = null, string DefaultValue = null)
        {
            DataFieldDef Result = new DataFieldDef();
            Result.Name = FieldName;
            Result.TitleKey = !string.IsNullOrWhiteSpace(TitleKey) ? TitleKey : FieldName;
            Result.DataType = DataType;
            Result.Required = Required;
            Result.DefaultValue = DefaultValue;
            Fields.Add(Result);
            return Result;
        }
        
        /// <summary>
        ///  Creates, adds and returns a string (nvarchar) field.
        /// </summary>
        public DataFieldDef AddStringField(string FieldName, int Length = 96, bool Required = false, string TitleKey = null, string DefaultValue = null)
        {
            DataFieldDef Result = new DataFieldDef();
            Result.Name = FieldName;
            Result.TitleKey = !string.IsNullOrWhiteSpace(TitleKey) ? TitleKey : FieldName;
            Result.DataType = DataFieldType.String;
            Result.Length = Length;
            Result.Required = Required;
            Result.DefaultValue = DefaultValue;
            Fields.Add(Result);
            return Result;
        }
        /// <summary>
        ///  Creates, adds and returns a field of a certain type.
        /// </summary>
        public DataFieldDef AddIntegerField(string FieldName, bool Required = false, string TitleKey = null, string DefaultValue = null)
        {
            return AddField(FieldName, DataFieldType.Integer, Required, TitleKey, DefaultValue);
        }
        /// <summary>
        ///  Creates, adds and returns a field of a certain type.
        /// </summary>
        public DataFieldDef AddFloatField(string FieldName, bool Required = false, string TitleKey = null, string DefaultValue = null)
        {
            return AddField(FieldName, DataFieldType.Float, Required, TitleKey, DefaultValue);
        }
        /// <summary>
        ///  Creates, adds and returns a field of a certain type.
        /// </summary>
        public DataFieldDef AddDecimalField(string FieldName, bool Required = false, string TitleKey = null, string DefaultValue = null)
        {
            return AddField(FieldName, DataFieldType.Decimal, Required, TitleKey, DefaultValue);
        }
        /// <summary>
        ///  Creates, adds and returns a field of a certain type.
        /// </summary>
        public DataFieldDef AddDateTimeField(string FieldName, bool Required = false, string TitleKey = null, string DefaultValue = null)
        {
            return AddField(FieldName, DataFieldType.DateTime, Required, TitleKey, DefaultValue);
        }
        /// <summary>
        ///  Creates, adds and returns a field of a certain type.
        /// </summary>
        public DataFieldDef AddDateField(string FieldName, bool Required = false, string TitleKey = null, string DefaultValue = null)
        {
            return AddField(FieldName, DataFieldType.Date, Required, TitleKey, DefaultValue);
        }
        /// <summary>
        ///  Creates, adds and returns a field of a certain type.
        /// </summary>
        public DataFieldDef AddBooleanField(string FieldName, bool Required = false, string TitleKey = null, string DefaultValue = null)
        {
            return AddField(FieldName, DataFieldType.Boolean, Required, TitleKey, DefaultValue);
        }
        /// <summary>
        ///  Creates, adds and returns a field of a certain type.
        /// </summary>
        public DataFieldDef AddBlobField(string FieldName, bool Required = false, string TitleKey = null)
        {
            return AddField(FieldName, DataFieldType.Blob, Required, TitleKey, null);
        }
        /// <summary>
        ///  Creates, adds and returns a field of a certain type.
        /// </summary>
        public DataFieldDef AddTextBlobField(string FieldName, bool Required = false, string TitleKey = null)
        {
            return AddField(FieldName, DataFieldType.TextBlob, Required, TitleKey, null);
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
        /// Fields
        /// </summary>
        public List<DataFieldDef> Fields { get; set; } = new List<DataFieldDef>();
 
    }




 
}
