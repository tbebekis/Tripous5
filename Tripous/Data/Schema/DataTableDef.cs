﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data 
{
    /// <summary>
    /// Database table definition
    /// </summary>
    public class DataTableDef
    {

        string fName;
        string fTitleKey;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public DataTableDef()
        {
        }

        /* static */
        /// <summary>
        /// Validates a specified database object identifier and throws an exception if it is invalid.
        /// </summary>
        static public void CheckIsValidDatabaseObjectIdentifier(string Name)
        {
            void Throw(string Text)
            {
                Sys.Throw($"Identifier name {Name} is not a valid database object identifier. {Text}");
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

            if (Name.Length > IdentifierMaxLength)
                Throw($"Identifier name {Name} exceeds max allowed length: {DataTableDef.IdentifierMaxLength}");

        }
        /// <summary>
        /// Forces a specified identifier name, such as Table or Field name, to have a valid max length.
        /// </summary>
        static public string EnsureIdentifierValidLength(string IdentifierName)
        {
            if (!string.IsNullOrWhiteSpace(IdentifierName))
            {
                if (IdentifierName.Length > IdentifierMaxLength)
                    IdentifierName = IdentifierName.Substring(0, IdentifierMaxLength);
            }

            return IdentifierName;
        }

        /// <summary>
        /// Splits a specified foreign key string of the form <code>TableName.ColumnName</code> and returns its two parts: ForeignTableName and ForeignColumnName
        /// </summary>
        static public void SplitForeignKey(string ForeignKey, out string ForeignTableName, out string ForeignColumnName)
        {
            string[] Parts = ForeignKey.Split('.', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
            ForeignTableName = Parts[0];
            ForeignColumnName = Parts[1];
        }


        /// <summary>
        /// Creates or alters a specified table in the database.
        /// <para>When only the new definition is passed a new database table is created.</para>
        /// <para>When both the new and the old definitions are passed, the function compares their fields and adds, drops or alters columns accordingly.</para>
        /// </summary>
        static public void CreateOrAlterTable(DataTableDef NewTableDef, DataTableDef OldTableDef = null)
        {
            // EDW: Test alter table with various databases

            void Throw(string Text)
            {
                Sys.Throw($"Cannot create or alter table: {OldTableDef.Name}.\n{Text}");
            }

            string SqlText;

            NewTableDef.Check();

            // is create table
            if (OldTableDef == null)
            {
                SqlText = NewTableDef.GetDefText();
                SchemaVersion SV = new SchemaVersion();
                SV.AddTable(SqlText);
                SV.Execute();
            }
            // is alter table columns
            else
            {
                if (!Sys.IsSameText(NewTableDef.Name, OldTableDef.Name))
                    Throw("Altering table name is not allowed.");

                string TableName = OldTableDef.Name;
                string ForeignTableName;
                string ForeignFieldName;

                SqlStore SqlStore = SqlStores.Default;
                SqlProvider Provider = SqlStore.Provider;

 
                List<string> SqlTextList = new List<string>();
  

                // drop column
                // ------------------------------------------------------------------------------------------------
                foreach (var OldField in OldTableDef.Fields)
                {
                    var NewField = NewTableDef.Fields.FirstOrDefault(item => Sys.IsSameText(item.Id, OldField.Id));
                    if (OldField == null)
                    {
                        if (OldField.Unique && Provider.SupportsAlterTableType(AlterTableType.TableUniqueConstraint))
                        {
                            SqlText = Provider.DropUniqueConstraintSql(TableName, OldField.UniqueConstraintName);
                            SqlTextList.Add(SqlText);
                        }

                        if (!string.IsNullOrWhiteSpace(OldField.ForeignKey) && Provider.SupportsAlterTableType(AlterTableType.TableForeignKeyConstraint))
                        {
                            SqlText = Provider.DropForeignKeySql(TableName, OldField.ForeignKeyConstraintName);
                            SqlTextList.Add(SqlText);
                        }

                        SqlText = Provider.DropColumnSql(TableName, OldField.Name);
                        SqlTextList.Add(SqlText);
                    }
                }



 
                foreach (var NewField in NewTableDef.Fields)
                {
                    var OldField = OldTableDef.Fields.FirstOrDefault(item => Sys.IsSameText(item.Id, NewField.Id));

                    // add new column
                    // ------------------------------------------------------------------------------------------------
                    if (OldField == null)
                    {
                        SqlText = Provider.AddColumnSql(TableName, NewField.Name, NewField.GetDefText());
                        SqlTextList.Add(SqlText);

                        // new column constraints
                        if (NewField.Unique && Provider.SupportsAlterTableType(AlterTableType.TableUniqueConstraint))
                        {
                            SqlText = Provider.AddUniqueConstraintSql(TableName, NewField.Name, NewField.UniqueConstraintName);
                            SqlTextList.Add(SqlText);
                        }

                        if (!string.IsNullOrWhiteSpace(NewField.ForeignKey) && Provider.SupportsAlterTableType(AlterTableType.TableForeignKeyConstraint))
                        {
                            SplitForeignKey(NewField.ForeignKey, out ForeignTableName, out ForeignFieldName);

                            SqlText = Provider.AddForeignKeySql(TableName, NewField.Name, ForeignTableName, ForeignFieldName, NewField.ForeignKeyConstraintName);
                            SqlTextList.Add(SqlText);
                        }
                    }

                    // alter column
                    // ------------------------------------------------------------------------------------------------
                    else
                    {
                        // rename column
                        if (!Sys.IsSameText(NewField.Name, OldField.Name))
                        {
                            SqlText = Provider.RenameColumnSql(TableName, OldField.Name, NewField.Name);
                            SqlTextList.Add(SqlText);
                        }

                        // alter column length
                        if (NewField.DataType == DataFieldType.String && NewField.Length != OldField.Length)
                        {
                            if (NewField.Length < OldField.Length)
                                Throw($"Column: {NewField.Name}. Reducing string column length is not permitted.");

                            SqlText = Provider.SetColumnLengthSql(TableName, NewField.Name, NewField.GetDefText());
                            SqlTextList.Add(SqlText);
                        }

                        // set/drop not null
                        if (NewField.Required != OldField.Required)
                        {
                            // set not null
                            if (NewField.Required)
                            {
                                if (string.IsNullOrWhiteSpace(NewField.DefaultExpression))
                                    Throw($"Column: {NewField.Name}. Cannot set a column to not null. No default value.");

                                SqlText = Provider.SetDefaultBeforeNotNullUpdateSql(TableName, NewField.Name, NewField.DefaultExpression);
                                SqlTextList.Add(SqlText);
 
                                SqlText = Provider.SetNotNullSql(TableName, NewField.Name, NewField.GetDataTypeDefText());
                                SqlTextList.Add(SqlText);
                            }
                            // drop not null
                            else
                            {
                                SqlText = Provider.DropNotNullSql(TableName, NewField.Name, NewField.GetDataTypeDefText());
                                SqlTextList.Add(SqlText);
                            }
                        }

                        // set/drop default
                        if (!Sys.IsSameText(NewField.DefaultExpression, OldField.DefaultExpression))
                        {
                            // drop default
                            if (!string.IsNullOrWhiteSpace(OldField.DefaultExpression))
                            {
                                SqlText = Provider.DropColumnDefaultSql(TableName, OldField.Name);
                                SqlTextList.Add(SqlText);
                            }                            
 
                            // set default
                            if (!string.IsNullOrWhiteSpace(NewField.DefaultExpression))
                            {
                                SqlText = Provider.SetColumnDefaultSql(TableName, NewField.Name, NewField.DefaultExpression);
                                SqlTextList.Add(SqlText);
                            }
                        }

                        // unique constraint
                        if (NewField.Unique != OldField.Unique && Provider.SupportsAlterTableType(AlterTableType.TableUniqueConstraint))
                        {
                            if (OldField.Unique)
                            {
                                SqlText = Provider.DropUniqueConstraintSql(TableName, OldField.UniqueConstraintName);
                                SqlTextList.Add(SqlText);
                            }

                            if (NewField.Unique)
                            {
                                SqlText = Provider.AddUniqueConstraintSql(TableName, NewField.Name, NewField.UniqueConstraintName);
                                SqlTextList.Add(SqlText);
                            }
                        }


                        // foreign key constraint
                        if (NewField.ForeignKey != OldField.ForeignKey && Provider.SupportsAlterTableType(AlterTableType.TableForeignKeyConstraint))
                        {
                            if (!string.IsNullOrWhiteSpace(OldField.ForeignKey))
                            {
                                SqlText = Provider.DropForeignKeySql(TableName, OldField.ForeignKeyConstraintName);
                                SqlTextList.Add(SqlText);
                            }

                            if (!string.IsNullOrWhiteSpace(NewField.ForeignKey))
                            {
                                SplitForeignKey(NewField.ForeignKey, out ForeignTableName, out ForeignFieldName);

                                SqlText = Provider.AddForeignKeySql(TableName, NewField.Name, ForeignTableName, ForeignFieldName, NewField.ForeignKeyConstraintName);
                                SqlTextList.Add(SqlText);
                            }
                        }

                    }
                }

                if (SqlTextList.Count > 0)
                    SqlStore.ExecSql(SqlTextList);
                else
                    Throw("Nothing changed");
                    


            }
        }

        /* public */
        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return this.Name;
        }

        /// <summary>
        /// Creates, adds and returns a unique constraint.
        /// <para>Used when a unique constraint is required on more than a single field, by adding a proper string, e.g. Field1, Field2</para>
        /// </summary>
        public UniqueConstraintDef AddUniqueConstraint(string FieldNames, string ConstraintName = "")
        {
            UniqueConstraintDef Result = new UniqueConstraintDef();
            Result.FieldNames = FieldNames;
            if (!string.IsNullOrWhiteSpace(ConstraintName))
                Result.Name = ConstraintName;

            UniqueConstraints.Add(Result);

            return Result;
        }
 

        /// <summary>
        /// Throws an exception if this instance is not a valid one.
        /// </summary>
        public void Check()
        {
             
            void Throw(string Text)
            {
                Sys.Throw($"Definition of {Name} table is invalid.\n{Text}");
            }

            // table name           
            if (string.IsNullOrWhiteSpace(Name))
                Sys.Throw($"Table definition without Name");

            CheckIsValidDatabaseObjectIdentifier(Name);


            // fields
            if (Fields == null || Fields.Count == 0)
                Throw($"No fields.");

            // valid identifiers: field Name and constraints
            Fields.ForEach((field) => {
                CheckIsValidDatabaseObjectIdentifier(field.Name);
                
                if (field.Unique)
                    CheckIsValidDatabaseObjectIdentifier(field.UniqueConstraintName);

                if (!string.IsNullOrWhiteSpace(field.ForeignKey))
                    CheckIsValidDatabaseObjectIdentifier(field.ForeignKeyConstraintName);
            });

            // duplicate field names
            List<string> FieldNames = new List<string>();
            Fields.ForEach(field =>
            {
                string FieldName = field.Name.ToLowerInvariant();
                if (FieldNames.Contains(FieldName))
                    Throw($"Duplicate column name: {field.Name}");
            });


            // compound primary key
            int Count = Fields.Count(field => field.IsPrimaryKey);
            //if (Count == 0)
            //    Throw($"No primary key.");

            if (Count > 1)
                Throw($"Compound primary keys not supported.");

            // primary key data-type
            DataFieldDef F = Fields.FirstOrDefault(field => field.IsPrimaryKey);
            if (!(F.DataType == DataFieldType.String || F.DataType == DataFieldType.Integer))
                Throw($"Not supported data-type for primary key: {F.DataType.ToString()}");

            // field data-type
            Count = Fields.Count(item => item.DataType == DataFieldType.Unknown);
            if (Count > 1)
                Throw($"Columns without data type.");
 

            Fields.ForEach((field) => {
                if (!string.IsNullOrWhiteSpace(field.ForeignKey))
                {
                    string[] Parts = field.ForeignKey.Split('.', StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                    if (Parts == null || Parts.Length != 2)
                        Throw($"Foreign Key constraint not fully defined in column: {field.Name}");
                } 
            });

 
        }
        /// <summary>
        /// Returns the table definition text.
        /// <para>WARNING: The returned text must be passed through <see cref="SqlProvider.ReplaceDataTypePlaceholders"/> method, for the final result.</para>
        /// </summary>
        public string GetDefText()
        {
            string sDef;
            StringBuilder SB = new StringBuilder();

            SB.AppendLine($"create table {Name} (");

            // columns
            for (int i = 0; i < Fields.Count; i++)
            {
                sDef = Fields[i].GetDefText();

                if (i == 0)
                    SB.AppendLine($"  {sDef}");
                else
                    SB.AppendLine($"  ,{sDef}");
            }
 
            // single-field unique constraints
            Fields.ForEach((field) => {
                if (field.Unique)
                {
                    sDef = field.GetUniqueConstraintDefText();
                    SB.AppendLine($"  ,{sDef}");
                }
            });

            // multi-field unique constraints
            UniqueConstraints.ForEach((constraint) => {
                sDef = constraint.GetDefText();
                SB.AppendLine($"  ,{sDef}");
            });

            // foreign key constraints
            Fields.ForEach((field) => {
                if (!string.IsNullOrWhiteSpace(field.ForeignKey))
                {
                    sDef = field.GetForeignKeyConstraintDefText();
                    SB.AppendLine($"  ,{sDef}");
                }
            });

 
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
            Dest.TitleKey = this.TitleKey;
            Dest.Owner = Owner;

            Dest.Data1 = Json.Serialize(this);
        }

        /// <summary>
        ///  Creates, adds and returns a primary key field of a defined type.
        ///  <para>NOTE: String and Integer are the only supported types.</para>
        /// </summary>
        public DataFieldDef AddId(string FieldName, DataFieldType DataType, int Length, string TitleKey = null)
        {
            if (!Bf.In(DataType, DataFieldType.String | DataFieldType.Integer))
                Sys.Throw($"DataType not supported for a table Primary Key. {DataType}");

            DataFieldDef Result = new DataFieldDef();
            Result.Name = FieldName;
            Result.TitleKey = !string.IsNullOrWhiteSpace(TitleKey) ? TitleKey : FieldName;
            Result.IsPrimaryKey = true;
            Result.DataType = DataType;
            if (DataType == DataFieldType.String)
                Result.Length = Length;
            Result.Required = true;
            Fields.Add(Result);
            return Result;
        }
        /// <summary>
        ///  Creates, adds and returns a primary key field.
        ///  <para>NOTE: This version adds a data-type according to <see cref="SysConfig.GuidOids"/> flag.</para>
        /// </summary>
        public DataFieldDef AddId(string FieldName = "Id",  int Length = 40, string TitleKey = null)
        {
            DataFieldType DataType = SysConfig.GuidOids ? DataFieldType.String : DataFieldType.Integer;
            return AddId(FieldName, DataType, Length, TitleKey);
        }
        /// <summary>
        ///  Creates, adds and returns an integer primary key field.
        /// </summary>
        public DataFieldDef AddIntegerId(string FieldName = "Id", string TitleKey = null)
        {
            return AddId(FieldName, DataFieldType.Integer, 0, TitleKey);
        }
        /// <summary>
        ///  Creates, adds and returns a string primary key field.
        /// </summary>
        public DataFieldDef AddStringId(string FieldName = "Id", int Length = 40, string TitleKey = null)
        {
            return AddId(FieldName, DataFieldType.String, 0, TitleKey);
        }

        /// <summary>
        ///  Creates, adds and returns a field.
        /// </summary>
        public DataFieldDef AddField(string FieldName, DataFieldType DataType, bool Required = false, string TitleKey = null, string DefaultExpression = null)
        {
            DataFieldDef Result = new DataFieldDef();
            Result.Name = FieldName;
            Result.TitleKey = !string.IsNullOrWhiteSpace(TitleKey) ? TitleKey : FieldName;
            Result.DataType = DataType;
            Result.Required = Required;
            Result.DefaultExpression = DefaultExpression;
            Fields.Add(Result);
            return Result;
        }
        
        /// <summary>
        ///  Creates, adds and returns a string (nvarchar) field.
        /// </summary>
        public DataFieldDef AddString(string FieldName, int Length = 96, bool Required = false, string TitleKey = null, string DefaultExpression = null)
        {
            DataFieldDef Result = new DataFieldDef();
            Result.Name = FieldName;
            Result.TitleKey = !string.IsNullOrWhiteSpace(TitleKey) ? TitleKey : FieldName;
            Result.DataType = DataFieldType.String;
            Result.Length = Length;
            Result.Required = Required;
            Result.DefaultExpression = DefaultExpression;
            Fields.Add(Result);
            return Result;
        }
        /// <summary>
        ///  Creates, adds and returns a field of a certain type.
        /// </summary>
        public DataFieldDef AddInteger(string FieldName, bool Required = false, string TitleKey = null, string DefaultExpression = null)
        {
            return AddField(FieldName, DataFieldType.Integer, Required, TitleKey, DefaultExpression);
        }
        /// <summary>
        ///  Creates, adds and returns a field of a certain type.
        /// </summary>
        public DataFieldDef AddFloat(string FieldName, bool Required = false, string TitleKey = null, string DefaultExpression = null)
        {
            return AddField(FieldName, DataFieldType.Float, Required, TitleKey, DefaultExpression);
        }
        /// <summary>
        ///  Creates, adds and returns a field of a certain type.
        /// </summary>
        public DataFieldDef AddDecimal(string FieldName, bool Required = false, string TitleKey = null, string DefaultExpression = null)
        {
            return AddField(FieldName, DataFieldType.Decimal, Required, TitleKey, DefaultExpression);
        }
        /// <summary>
        ///  Creates, adds and returns a field of a certain type.
        /// </summary>
        public DataFieldDef AddDateTime(string FieldName, bool Required = false, string TitleKey = null, string DefaultExpression = null)
        {
            return AddField(FieldName, DataFieldType.DateTime, Required, TitleKey, DefaultExpression);
        }
        /// <summary>
        ///  Creates, adds and returns a field of a certain type.
        /// </summary>
        public DataFieldDef AddDate(string FieldName, bool Required = false, string TitleKey = null, string DefaultExpression = null)
        {
            return AddField(FieldName, DataFieldType.Date, Required, TitleKey, DefaultExpression);
        }
        /// <summary>
        ///  Creates, adds and returns a field of a certain type.
        /// </summary>
        public DataFieldDef AddBoolean(string FieldName, bool Required = false, string TitleKey = null, string DefaultExpression = null)
        {
            return AddField(FieldName, DataFieldType.Boolean, Required, TitleKey, DefaultExpression);
        }
        /// <summary>
        ///  Creates, adds and returns a field of a certain type.
        /// </summary>
        public DataFieldDef AddBlob(string FieldName, bool Required = false, string TitleKey = null)
        {
            return AddField(FieldName, DataFieldType.Blob, Required, TitleKey, null);
        }
        /// <summary>
        ///  Creates, adds and returns a field of a certain type.
        /// </summary>
        public DataFieldDef AddTextBlob(string FieldName, bool Required = false, string TitleKey = null)
        {
            return AddField(FieldName, DataFieldType.TextBlob, Required, TitleKey, null);
        }




        /* properties */
        /// <summary>
        /// A GUID string. 
        /// </summary>
        public string Id { get; set; } = Sys.GenId(true);
        /// <summary>
        /// A name unique among all instances of this type
        /// </summary>
        public string Name
        {
            get { return fName; }
            set
            {
                CheckIsValidDatabaseObjectIdentifier(value);
                fName = value;
            }
        }
        /// <summary>
        /// Gets or sets a resource Key used in returning a localized version of Title
        /// </summary>
        public string TitleKey
        {
            get { return !string.IsNullOrWhiteSpace(fTitleKey) ? fTitleKey : Name; }
            set { fTitleKey = value; }
        }
        /// <summary>
        /// Gets the Title of this instance, used for display purposes. 
        /// <para>NOTE: The setter is fake. Do NOT use it.</para>
        /// </summary>    
        public string Title
        {
            get { return !string.IsNullOrWhiteSpace(TitleKey) ? Res.GS(TitleKey, TitleKey) : Name; }
            set { }
        }

        /// <summary>
        /// Fields
        /// </summary>
        public List<DataFieldDef> Fields { get; set; } = new List<DataFieldDef>();
        /// <summary>
        /// For multi-field unique constraints.
        /// <para>Use it when a unique constraint is required on more than a single field adding a proper string, e.g. Field1, Field2</para>
        /// </summary>
        public List<UniqueConstraintDef> UniqueConstraints { get; set; } = new List<UniqueConstraintDef>();

        /// <summary>
        /// Max length for all identifier names such as Table, Field and Constraint names.
        /// </summary>
        static public int IdentifierMaxLength { get; set; } = 30;
 
    }


    /// <summary>
    /// For table-wise unique constraints, possibly on multiple fields.
    /// </summary>
    public class UniqueConstraintDef
    {
        string fFieldNames;

        /// <summary>
        /// Constructor
        /// </summary>
        public UniqueConstraintDef()
        {
        }

        /// <summary>
        /// The constraint name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// A proper string, e.g. <code>Field1, Field2</code>
        /// </summary>
        public string FieldNames
        {
            get { return fFieldNames; }
            set
            {
                if (fFieldNames != value)
                {
                    if (!string.IsNullOrWhiteSpace(value) && string.IsNullOrWhiteSpace(Name))
                        Name = "UC_" + Sys.GenerateRandomString(DataTableDef.IdentifierMaxLength - 3);

                    fFieldNames = value;
                }
            }
        }


        /// <summary>
        /// Returns the definition text.
        /// </summary>
        public string GetDefText()
        { 
            string sName = DataTableDef.EnsureIdentifierValidLength(this.Name);
            string Result = $"constraint {sName} unique ({this.Name})";
            return Result;
        }

    }



}
