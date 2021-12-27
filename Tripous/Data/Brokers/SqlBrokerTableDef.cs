using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using Newtonsoft.Json;

using Tripous.Data;

namespace Tripous.Data
{


    /// <summary>
    /// A broker table definition
    /// </summary>
    public class SqlBrokerTableDef
    {

        /// <summary>
        /// Constant
        /// </summary>
        public const string ITEM = "_ITEM_";
        /// <summary>
        /// Constant
        /// </summary>
        public const string LINES = "_LINES_";
        /// <summary>
        /// Constant
        /// </summary>
        public const string SUBLINES = "_SUBLINES_";

 

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public SqlBrokerTableDef()
        {

        }

        /* public */
        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Clears the property values of this instance.
        /// </summary>
        public void Clear()
        {
            SqlBrokerTableDef Empty = new SqlBrokerTableDef();
            Sys.AssignObject(Empty, this);
        }
        /// <summary>
        /// Assigns property values from a source instance.
        /// </summary>
        public void Assign(SqlBrokerTableDef Source)
        { 
            Sys.AssignObject(Source, this);
        }
        /// <summary>
        /// Returns a clone of this instance.
        /// </summary>
        public SqlBrokerTableDef Clone()
        {
            SqlBrokerTableDef Result = new SqlBrokerTableDef();
            Sys.AssignObject(this, Result);
            return Result;
        }


        /* find */
        /// <summary>
        /// Searces the whole joined tree for a table by a Name or Alias and returns
        /// a JoinTableDescriptor, if any, else null.
        /// </summary>
        public SqlBrokerTableDef FindAnyJoinTable(string NameOrAlias)
        {
            var Result = this.JoinTables.Find(item => item.Name.IsSameText(NameOrAlias) || item.Alias.IsSameText(NameOrAlias)); // base.Find(NameOrAlias);
            if (Result == null)
            {
                foreach (var JoinTable in this.JoinTables)
                {
                    Result = JoinTable.FindAnyJoinTable(NameOrAlias);
                    if (Result != null)
                        return Result;
                }
            }

            return Result;
        }
        /// <summary>
        /// Finds a join table descriptor by MasterKeyField, if any, else null.
        /// </summary>
        public SqlBrokerTableDef FindJoinTableByMasterKeyField(string MasterKeyField)
        {
            return this.JoinTables.Find(item => item.MasterKeyField.IsSameText(MasterKeyField));
        }
        /// <summary>
        /// Searces the whole joined tree for a join table descriptor by MasterKeyField
        /// and returns that table, if any, else null.
        /// </summary>
        public SqlBrokerTableDef FindAnyJoinTableByMasterKeyField(string MasterKeyField)
        {
            var Result = FindJoinTableByMasterKeyField(MasterKeyField);
            if (Result == null)
            {
                foreach (var JoinTable in this.JoinTables)
                {
                    Result = JoinTable.FindAnyJoinTableByMasterKeyField(MasterKeyField);
                    if (Result != null)
                        return Result;
                }
            }

            return Result;
        }

        /// <summary>
        /// Finds and returns, if exists, a field that has NameOrAlias Name or Alias. 
        /// It searches this table descriptor and its joined tables in the full tree.
        /// Returns null if a field not found.
        /// </summary>
        public Tuple<SqlBrokerTableDef, SqlBrokerFieldDef> FindAnyField(string NameOrAlias)
        {
            SqlBrokerFieldDef FieldDef = Fields.Find(item => item.Name.IsSameText(NameOrAlias) || item.Alias.IsSameText(NameOrAlias));
            if (FieldDef != null)
                return Tuple.Create(this, FieldDef);
            return FindAnyField(NameOrAlias, this.JoinTables);
        }
        /// <summary>
        /// Finds a field by Name or Alias by searching the whole tree of JoinTables tables.
        /// Returns null if a field not found.
        /// </summary>
        Tuple<SqlBrokerTableDef, SqlBrokerFieldDef> FindAnyField(string NameOrAlias, List<SqlBrokerTableDef> JoinTables)
        {

            Tuple<SqlBrokerTableDef, SqlBrokerFieldDef> Result = null;
            SqlBrokerFieldDef FieldDef = null;

            foreach (var JoinTable in JoinTables)
            {
                FieldDef = JoinTable.Fields.Find(item => item.Name.IsSameText(NameOrAlias) || item.Alias.IsSameText(NameOrAlias));
                if (FieldDef != null)
                {
                    Result = Tuple.Create(JoinTable, FieldDef);
                    break;
                }

                if (JoinTable.JoinTables != null)
                {
                    Result = FindAnyField(NameOrAlias, JoinTable.JoinTables);
                    if (Result != null)
                        break;
                }

            }

            return Result;
        }

        /// <summary>
        /// Finds a field title by searching the whole tree of fields.
        /// </summary>
        public string FindAnyFieldTitle(string NameOrAlias)
        {
            Tuple<SqlBrokerTableDef, SqlBrokerFieldDef> Pair = FindAnyField(NameOrAlias);
            SqlBrokerFieldDef Field = Pair != null ? Pair.Item2 : null;
            return (Field == null) ? NameOrAlias : Field.Title;
        }

        /* sql generation */
        /// <summary>
        /// Generates SQL statements using the TableDes descriptor and the Flags
        /// </summary>
        public void BuildSql(TableSqls Statements, BuildSqlFlags Flags)
        {
            Statements.Clear();

            /* field lists preparation */
            string S = "";              // insert field list
            string S2 = "";             // insert params field list
            string S3 = "";             // update field list AND update params field list
            string FieldName = "";

            bool GuidOid = Bf.Member(BuildSqlFlags.GuidOids, Flags) || this.Fields.Find(item => item.Name.IsSameText(this.PrimaryKeyField)).DataType == DataFieldType.String;
            bool OidModeIsBefore = !GuidOid && ((Flags & BuildSqlFlags.OidModeIsBefore) == BuildSqlFlags.OidModeIsBefore);

            foreach (var FieldDes in this.Fields)
            {
                if (FieldDes.IsNativeField && !FieldDes.IsNoInsertOrUpdate)
                {
                    FieldName = FieldDes.Name;

                    if (!Sys.IsSameText(FieldName, this.PrimaryKeyField))
                    {
                        S = S + Environment.NewLine + "  " + FieldName + ", ";
                        S2 = S2 + Environment.NewLine + "  " + ":" + FieldName + ", ";
                        S3 = S3 + Environment.NewLine + "  " + FieldName + " = :" + FieldName + ", ";
                    }
                    else if (GuidOid || OidModeIsBefore)
                    {
                        S = S + Environment.NewLine + "  " + FieldName + ", ";
                        S2 = S2 + Environment.NewLine + "  " + ":" + FieldName + ", ";
                    }

                }
            }

            if (S.Length > 2)
                S = S.Remove(S.Length - 2, 2);


            if (S2.Length > 2)
                S2 = S2.Remove(S2.Length - 2, 2);


            if (S3.Length > 2)
                S3 = S3.Remove(S3.Length - 2, 2);

            /* Insert */
            string SqlText = "insert into {0} ( {1}" + Environment.NewLine + " ) values ( {2}" + Environment.NewLine + " ) ";
            Statements.InsertRowSql = string.Format(SqlText, this.Name, S, S2);

            /* Update */
            SqlText = "update {0} " + Environment.NewLine + "set {1} " + Environment.NewLine + "where " + Environment.NewLine + "  {2} = :{2} ";
            Statements.UpdateRowSql = string.Format(SqlText, this.Name, S3, this.PrimaryKeyField);

            /* Delete */
            Statements.DeleteRowSql = string.Format("delete from {0} where {1} = :{2}", this.Name, this.PrimaryKeyField, this.PrimaryKeyField);

            /* RowSelect */
            SelectSql SS = BuildSql_Select(Flags, false);
            SS.Where = string.Format("where {0}.{1} = :{1}", this.Name, this.PrimaryKeyField);
            Statements.SelectRowSql = SS.Text;


            /* Browse */
            SS = BuildSql_Select(Flags, true);
            // it is a detail table 
            bool IsDetailTable = !string.IsNullOrWhiteSpace(this.MasterTableName)
                                && !string.IsNullOrWhiteSpace(this.MasterKeyField)
                                && !string.IsNullOrWhiteSpace(this.DetailKeyField);

            if (IsDetailTable)
            {
                SS.Where = string.Format("{0}.{1} = :{2}",
                    this.Alias, this.DetailKeyField, Sys.MASTER_KEY_FIELD_NAME);
            }

            Statements.SelectSql = SS.Text;
        }
        /// <summary>
        /// Generates and returns the SELECT statements
        /// </summary>
        SelectSql BuildSql_Select(BuildSqlFlags Flags, bool IsBrowserSelect)
        {

            SelectSql SelectSql = new SelectSql();

            // native fields
            string S = string.Empty;
            foreach (var FieldDes in this.Fields)
            {
                if (FieldDes.IsNativeField && !FieldDes.IsNoInsertOrUpdate)
                {
                    if (IsBrowserSelect)
                    {
                        if (FieldDes.DataType.IsBlob() && ((Flags & BuildSqlFlags.IncludeBlobFields) == BuildSqlFlags.None))
                            continue;
                    }

                    S = S + "  " + Sql.FormatFieldNameAlias(this.Name, FieldDes.Name, FieldDes.Name, Sql.StatementDefaultSpaces);
                }
            }

            SelectSql.Select = S;

            // native from
            SelectSql.From = "  " + Sql.FormatTableNameAlias(this.Name, this.Alias) + " " + Environment.NewLine;


            // if LookUp fields are to be handled as joined tables
            NameValueStringList JoinTableNamesList = new NameValueStringList();

            string JoinTableName;
            if ((Flags & BuildSqlFlags.JoinLookUpFields) == BuildSqlFlags.JoinLookUpFields)
            {
                foreach (var FieldDes in this.Fields)
                {
                    if (FieldDes.IsForeignKeyField)
                    {
                        JoinTableName = Sql.FormatTableNameAlias(FieldDes.ForeignTableName, FieldDes.ForeignTableAlias);
                        if (JoinTableNamesList.IndexOf(JoinTableName) == -1)
                        {
                            JoinTableNamesList.Add(JoinTableName);
                            SelectSql.From += string.Format("    left join {0} on {1}.{2} = {3}.{4} " + Environment.NewLine,
                                                                        JoinTableName,
                                                                        FieldDes.ForeignTableAlias,
                                                                        FieldDes.ForeignKeyField,
                                                                        this.Alias,
                                                                        FieldDes.Alias);
                        }

                        SelectSql.Select += "  " + Sql.FormatFieldNameAlias(FieldDes.ForeignTableAlias, FieldDes.ForeignKeyField, FieldDes.Name, Sql.StatementDefaultSpaces);
                    }
                }
            }

            // joined tables and fields
            foreach (var JoinTableDes in this.JoinTables)
                BuildSql_AddJoinTable(JoinTableNamesList, SelectSql, this.Alias, JoinTableDes);


            // remove the last comma
            S = SelectSql.Select.TrimEnd(null);
            if ((S.Length > 1) && (S[S.Length - 1] == ','))
                S = S.Remove(S.Length - 1, 1);

            SelectSql.Select = S;
            SelectSql.From = SelectSql.From.TrimEnd(null);


            return SelectSql;

        }
        /// <summary>
        /// Called by BuildSql to handle join tables
        /// </summary>
        void BuildSql_AddJoinTable(NameValueStringList JoinTableNamesList, SelectSql SelectSql, string MasterAlias, SqlBrokerTableDef JoinTableDes)
        {

            string JoinTableName = Sql.FormatTableNameAlias(JoinTableDes.Name, JoinTableDes.Alias);

            if (JoinTableNamesList.IndexOf(JoinTableName) == -1)
            {
                JoinTableNamesList.Add(JoinTableName);
                SelectSql.From += string.Format("    left join {0} on {1}.{2} = {3}.{4} " + Environment.NewLine,
                                            JoinTableName,
                                            JoinTableDes.Alias,
                                            JoinTableDes.PrimaryKeyField,
                                            MasterAlias,
                                            JoinTableDes.MasterKeyField);
            }

            // joined fields
            foreach (var JoinFieldDes in JoinTableDes.Fields)
            {
                if (!Sys.IsSameText(JoinFieldDes.Name, JoinTableDes.PrimaryKeyField))
                {
                    SelectSql.Select += "  " + Sql.FormatFieldNameAlias(JoinTableDes.Alias, JoinFieldDes.Name, JoinFieldDes.Alias, Sql.StatementDefaultSpaces);
                }
            }

            // joined tables to this join table
            foreach (var JoinTableDescriptor in JoinTableDes.JoinTables)
                BuildSql_AddJoinTable(JoinTableNamesList, SelectSql, JoinTableDes.Alias, JoinTableDescriptor);

        }

        /// <summary>
        /// Updates this descriptor information using a specified DataTable schema.
        /// </summary>
        public void UpdateFrom(DataTable Table)
        {
            FieldFlags Flags;
            string Title;

            foreach (DataColumn Field in Table.Columns)
            {
                var FieldDes = Fields.Find(item => item.Name.IsSameText(Field.ColumnName));

                if (FieldDes == null)
                {
                    Flags = FieldFlags.None;
                    Title = Res.GS(Field.ColumnName);
                    if (!Db.IsVisibleColumn(Field.ColumnName))
                        Flags |= FieldFlags.Hidden;

                    if (Simple.IsString(Field.DataType) || Simple.IsDateTime(Field.DataType))
                        Flags |= FieldFlags.Searchable;

                    Fields.Add(new SqlBrokerFieldDef()
                    {
                        Name = Field.ColumnName,
                        DataType = DataFieldTypeHelper.DataFieldTypeOf(Field.DataType),
                        MaxLength = Field.MaxLength,
                        TitleKey = Title,
                        Flags = Flags
                    });
                }
                else
                {

                    if (FieldDes.DataType.IsDateTime() && Field.DataType == typeof(DateTime))
                    {
                        // let FieldDes.DataType keep its original value
                    }
                    else if (FieldDes.DataType == DataFieldType.Boolean && (Field.DataType == typeof(int) || Field.DataType == typeof(System.Int64)))
                    {
                        // let FieldDes.DataType keep its original value
                    }
                    else if (FieldDes.DataType != DataFieldTypeHelper.DataFieldTypeOf(Field.DataType))
                    {
                        FieldDes.DataType = DataFieldTypeHelper.DataFieldTypeOf(Field.DataType);
                    }

                    if (FieldDes.DataType == DataFieldType.String && (Field.MaxLength != -1) && (FieldDes.MaxLength != Field.MaxLength))
                        FieldDes.MaxLength = Field.MaxLength;
                }

            }
        }

        /* create DataTable */
        /// <summary>
        /// Creates a DataTable based on a descriptor. 
        /// <para>Creates the look-up tables too if a flag is specified.</para>
        /// <para>The table may added to a list using a specified delegate.</para>
        /// </summary>
        public void CreateDescriptorTable(SqlStore Store, Action<MemTable> AddTableFunc, bool CreateLookUpTables)
        {
 
            MemTable Table = new MemTable() { TableName = this.Name };
            AddTableFunc(Table);
            Table.ExtendedProperties["Descriptor"] = this;

            Table.PrimaryKeyField = this.PrimaryKeyField;
            Table.MasterKeyField = this.MasterKeyField;
            Table.DetailKeyField = this.DetailKeyField;
            DataColumn Column;

            // native fields and lookups
            foreach (var FieldDes in this.Fields)
            {
                Column = new DataColumn(FieldDes.Name);
                Column.ExtendedProperties["Descriptor"] = FieldDes;
                Column.DataType = FieldDes.DataType.GetNetType();
                if (Sys.IsSameText(this.PrimaryKeyField, FieldDes.Name) && (FieldDes.DataType == DataFieldType.Integer))
                {
                    Column.AutoIncrement = true;
                    Column.AutoIncrementSeed = -1;
                    Column.AutoIncrementStep = -1;
                }
                if (Column.DataType == typeof(System.String))
                    Column.MaxLength = FieldDes.MaxLength;
                Column.Caption = FieldDes.Title;

                SetupDefaultValue(Store, Column, FieldDes);

                Table.Columns.Add(Column); 

                if (CreateLookUpTables && FieldDes.IsForeignKeyField)
                {
                    AddTableFunc(CreateDescriptorTables_CreateLookUpTable(FieldDes));
                }
     

                // joined table to TableDescriptor on this FieldDes
                SqlBrokerTableDef JoinTableDes = this.FindAnyJoinTableByMasterKeyField(FieldDes.Name);
                if (JoinTableDes != null)
                    CreateDescriptorTables_AddJoinTableFields(JoinTableDes, Table);
            }

            
        }
        /// <summary>
        /// Sets up the <see cref="DataColumn.DefaultValue"/> of a specified column based on a specified field descriptor settings.
        /// </summary>
        void SetupDefaultValue(SqlStore Store, DataColumn Column, SqlBrokerFieldDef FieldDes)
        {
            if ((FieldDes.DefaultValue != null) && !Sys.IsSameText(Sys.NULL, FieldDes.DefaultValue))
            {

                try
                {
                    if (Sys.IsSameText("EmptyString", FieldDes.DefaultValue))
                        Column.DefaultValue = string.Empty;
                    else if (!Db.StandardDefaultValues.ContainsText(FieldDes.DefaultValue))
                    {
                        SimpleType ColumnDataType = Simple.SimpleTypeOf(Column.DataType);

                        if (Simple.IsInteger(ColumnDataType))
                            Column.DefaultValue = int.Parse(FieldDes.DefaultValue);
                        else if (Simple.IsFloat(ColumnDataType))
                            Column.DefaultValue = double.Parse(FieldDes.DefaultValue);
                        else if (Simple.IsString(ColumnDataType))
                            Column.DefaultValue = FieldDes.DefaultValue;
                    }
                }
                catch
                {
                }
            }






        }
        /// <summary>
        /// Creates look up table for the browser table
        /// </summary>
        MemTable CreateDescriptorTables_CreateLookUpTable(SqlBrokerFieldDef FieldDes)
        { 
            MemTable Table = new MemTable() { TableName = FieldDes.ForeignTableAlias };            
 
            DataColumn Column = new DataColumn(FieldDes.ForeignKeyField);
            Column.DataType = FieldDes.DataType.GetNetType();
            Column.MaxLength = FieldDes.MaxLength;
            Table.Columns.Add(Column);

            return Table;
        }
        /// <summary>
        /// Adds join fields to the Table
        /// </summary>
        void CreateDescriptorTables_AddJoinTableFields(SqlBrokerTableDef JoinTable, MemTable Table)
        {
            DataColumn Column;
            foreach (var JoinFieldDes in JoinTable.Fields)
            {
                if (!Sys.IsSameText(JoinTable.PrimaryKeyField, JoinFieldDes.Name))
                {
                    Column = new DataColumn(JoinFieldDes.Alias);
                    Column.ExtendedProperties["Descriptor"] = JoinFieldDes;
                    Column.DataType = JoinFieldDes.DataType.GetNetType();
                    Column.MaxLength = JoinFieldDes.MaxLength;
                    Column.Caption = JoinFieldDes.Title;

                    Table.Columns.Add(Column);

                    // joined table to JoinTable on this JoinFieldDes
                    var JoinTableDes2 = JoinTable.FindAnyJoinTableByMasterKeyField(JoinFieldDes.Name);
                    if (JoinTableDes2 != null)
                        CreateDescriptorTables_AddJoinTableFields(JoinTableDes2, Table);
                }
            }
        }
 
        /* sql filters */
        /// <summary>
        /// Creates <see cref="SqlFilterDef"/> items for each searchable <see cref="SqlBrokerFieldDef"/> item of a <see cref="SqlBrokerTableDef"/>.
        /// </summary>
        public void CreateSqlFilters(DataTable Table, SqlFilterDefs Filters)
        {
            SimpleType SimpleType;
            SqlBrokerTableDef TableDef;
            SqlBrokerFieldDef FieldDef;
            SqlFilterDef Filter;
            Tuple<SqlBrokerTableDef, SqlBrokerFieldDef> Pair = null;

            foreach (DataColumn Field in Table.Columns)
            {
                SimpleType = Simple.SimpleTypeOf(Field.DataType);
                if (SimpleType.IsString() || SimpleType.IsNumeric() || SimpleType.IsDateTime())
                {
                    Pair = this.FindAnyField(Field.ColumnName);
                    if (Pair != null)
                    {
                        TableDef = Pair.Item1;
                        FieldDef = Pair.Item2;

                        if ((FieldDef != null) && (FieldDef.IsSearchable))
                        {
                            if (SimpleType.IsString() && ((Field.MaxLength > 100) || (FieldDef.MaxLength > 100)))
                                continue;

                            Filter = Filters.Add(TableDef.Alias, FieldDef.Name, FieldDef.Title, Field.DataType.DataFieldTypeOf());
                            if (FieldDef.IsBoolean)
                                Filter.DataType = DataFieldType.Boolean;
                        }
                    }
                }
            }
        }

        /* display labels */
        /// <summary>
        /// Setups column titles for Table columns using the DisplayLabels string and the TableDes TableDescriptor.
        /// </summary>
        public void SetupFieldsDisplayLabelsFor(DataTable Table, string DisplayLabels)
        {
            SetupFieldsDisplayLabelsFor(Table, new NameValueStringList(DisplayLabels));
        }
        /// <summary>
        /// Setups column titles for Table columns using the DisplayLabes dictionary and the TableDes TableDescriptor.
        /// </summary>
        public void SetupFieldsDisplayLabelsFor(DataTable Table, Dictionary<string, string> DisplayLabels)
        {
            foreach (DataColumn Field in Table.Columns)
            {
                // if Column.Caption is not defined in some way
                if (string.IsNullOrWhiteSpace(Field.Caption) || Sys.IsSameText(Field.ColumnName, Field.Caption))
                {
                    // first look to the DisplayLabels 
                    if ((DisplayLabels.ContainsKey(Field.ColumnName)) && !string.IsNullOrWhiteSpace(DisplayLabels[Field.ColumnName]))
                    {
                        Field.Caption = DisplayLabels[Field.ColumnName];
                    }
                    // and then look to ANY field (joins included) of the TableDes
                    else if (!string.IsNullOrWhiteSpace(this.FindAnyFieldTitle(Field.ColumnName)))
                    {
                        Field.Caption = this.FindAnyFieldTitle(Field.ColumnName);
                    }
                }
            }
        }
        /// <summary>
        /// Setups column titles for Table columns using the DisplayLabes dictionary and the TableDes TableDescriptor.
        /// </summary>
        public void SetupFieldsDisplayLabelsFor(DataTable Table, NameValueStringList DisplayLabels)
        {
            string Caption;

            foreach (DataColumn Field in Table.Columns)
            {
                // if Column.Caption is not defined in some way
                if (string.IsNullOrWhiteSpace(Field.Caption) || Sys.IsSameText(Field.ColumnName, Field.Caption))
                {
                    /* DataColumn.Caption property seems to be case-insensitive.   
                       That is, assigning a "Phone" caption to an DataColumn.Caption 
                       already captioned as "PHONE" 
                       leaves the old "PHONE" intact. */
                    Caption = Field.Caption;
                    Field.Caption = "temp";

                    // first look to the DisplayLabels 
                    if ((DisplayLabels.ContainsName(Field.ColumnName)) && !string.IsNullOrWhiteSpace(DisplayLabels.Values[Field.ColumnName]))
                    {
                        Caption = DisplayLabels.Values[Field.ColumnName];
                    }
                    // and then look to ANY field (joins included) of the TableDes
                    else if (!string.IsNullOrWhiteSpace(this.FindAnyFieldTitle(Field.ColumnName)))
                    {
                        Caption = this.FindAnyFieldTitle(Field.ColumnName);
                    }

                    Field.Caption = Caption;
                }
            }
        }

        /* fields */
        /// <summary>
        /// Adds and returns a field.
        /// </summary>
        public SqlBrokerFieldDef AddField(string Name, DataFieldType DataType, string TitleKey, FieldFlags Flags = FieldFlags.None)
        {
            SqlBrokerFieldDef Result = Fields.Find(item => item.Name.IsSameText(Name));

            if (Result == null)
            {
                Result = new SqlBrokerFieldDef() 
                { 
                    Name = Name,
                    TitleKey = !string.IsNullOrWhiteSpace(TitleKey)? TitleKey: Name,
                    DataType = DataType, 
                    Flags = Flags 
                };

                Fields.Add(Result);
            }

            return Result;
        }

        /// <summary>
        /// Adds and returns an Id field.
        /// </summary>
        public SqlBrokerFieldDef AddId(string Name, DataFieldType DataType, int MaxLength = 40)
        {
            if (!Bf.In(DataType, DataFieldType.String | DataFieldType.Integer))
                Sys.Throw($"DataType not supported for a table Primary Key. {DataType}");

            var Result = AddField(Name, DataType, "", FieldFlags.Hidden);
            if (DataType == DataFieldType.String)
                Result.MaxLength = MaxLength;
            return Result;
        }
        /// <summary>
        /// Adds and returns an Id field based on settings on <see cref="SysConfig.OidDataType"/> and <see cref="SysConfig.OidSize"/>.
        /// </summary>
        public SqlBrokerFieldDef AddId(string Name = "Id")
        {
            return AddId(Name, SysConfig.OidDataType, SysConfig.OidSize);
        }
        /// <summary>
        /// Adds and returns a string Id field
        /// </summary>
        public SqlBrokerFieldDef AddStringId(string Name = "Id", int MaxLength = 40)
        {
            return AddId(Name, DataFieldType.String, MaxLength);
        }
        /// <summary>
        /// Adds and returns an integer Id field
        /// </summary>
        public SqlBrokerFieldDef AddIntegerId(string Name = "Id")
        {
            return AddId(Name, DataFieldType.Integer, 0);
        }

        /// <summary>
        /// Adds and returns a string field.
        /// </summary>
        public SqlBrokerFieldDef Add(string Name, int MaxLength, string TitleKey = "", FieldFlags Flags = FieldFlags.None)
        {
            SqlBrokerFieldDef Result = AddField(Name, DataFieldType.String, TitleKey, Flags);
            Result.MaxLength = MaxLength;
            return Result;
        }

        /// <summary>
        /// Adds and returns a string field.
        /// </summary>
        public SqlBrokerFieldDef AddString(string Name, int MaxLength = 96, string TitleKey = "", FieldFlags Flags = FieldFlags.None)
        {
            SqlBrokerFieldDef Result = AddField(Name, DataFieldType.String, TitleKey, Flags);
            Result.MaxLength = MaxLength;
            return Result;
        }
        /// <summary>
        /// Adds and returns an integer field.
        /// </summary>
        public SqlBrokerFieldDef AddInteger(string Name, string TitleKey = "", FieldFlags Flags = FieldFlags.None)
        {
            SqlBrokerFieldDef Result = AddField(Name, DataFieldType.Integer, TitleKey, Flags); 
            return Result;
        }
        /// <summary>
        /// Adds and returns an double field.
        /// </summary>
        public SqlBrokerFieldDef AddDouble(string Name, int Decimals = 4, string TitleKey = "", FieldFlags Flags = FieldFlags.None)
        {
            SqlBrokerFieldDef Result = AddField(Name, DataFieldType.Float, TitleKey, Flags);
            Result.Decimals = Decimals;
            return Result;
        }
        /// <summary>
        /// Adds and returns an decimal field.
        /// </summary>
        public SqlBrokerFieldDef AddDecimal(string Name, int Decimals = 4, string TitleKey = "", FieldFlags Flags = FieldFlags.None)
        {
            SqlBrokerFieldDef Result = AddField(Name, DataFieldType.Decimal, TitleKey, Flags);
            Result.Decimals = Decimals;
            return Result;
        }
        /// <summary>
        /// Adds and returns an date field.
        /// </summary>
        public SqlBrokerFieldDef AddDate(string Name, string TitleKey = "", FieldFlags Flags = FieldFlags.None)
        {
            SqlBrokerFieldDef Result = AddField(Name, DataFieldType.Date, TitleKey, Flags);
            return Result;
        }
        /// <summary>
        /// Adds and returns an date-time field.
        /// </summary>
        public SqlBrokerFieldDef AddDateTime(string Name, string TitleKey = "", FieldFlags Flags = FieldFlags.None)
        {
            SqlBrokerFieldDef Result = AddField(Name, DataFieldType.DateTime, TitleKey, Flags);
            return Result;
        }
        /// <summary>
        /// Adds and returns an integer-boolean field.
        /// </summary>
        public SqlBrokerFieldDef AddBoolean(string Name, string TitleKey = "", FieldFlags Flags = FieldFlags.None)
        {
            SqlBrokerFieldDef Result = AddField(Name, DataFieldType.Boolean, TitleKey, Flags | FieldFlags.Boolean);
            return Result;
        }
        /// <summary>
        /// Adds and returns a blob field.
        /// </summary>
        public SqlBrokerFieldDef AddBlob(string Name, string TitleKey = "", FieldFlags Flags = FieldFlags.None)
        {
            SqlBrokerFieldDef Result = AddField(Name, DataFieldType.Blob, TitleKey, Flags);
            return Result;
        }
        /// <summary>
        /// Adds and returns a text blob field.
        /// </summary>
        public SqlBrokerFieldDef AddTextBlob(string Name, string TitleKey = "", FieldFlags Flags = FieldFlags.Memo)
        {
            SqlBrokerFieldDef Result = AddField(Name, DataFieldType.TextBlob, TitleKey, Flags);
            return Result;
        }

        /// <summary>
        /// Sets the master table of this table.
        /// </summary>
        public SqlBrokerTableDef SetMaster(string MasterTableName, string MasterKeyField, string DetailKeyField)
        {
            this.MasterTableName = MasterTableName;
            this.MasterKeyField = MasterKeyField;
            this.DetailKeyField = DetailKeyField;
            return this;
        }

        /* properties */
        /// <summary>
        /// The table name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// An alias of this table
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets a resource Key used in returning a localized version of Title
        /// </summary>
        public string TitleKey { get; set; }
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
        /// Gets or sets the name of the primary key field of this table.
        /// </summary>
        public string PrimaryKeyField { get; set; } = "Id";
 
        /// <summary>
        /// Gets or sets the name of the master table.
        /// <para>It is used when this table is a detail table in a master-detail relation.</para>
        /// </summary>
        public string MasterTableName { get; set; }
        /// <summary>
        /// Gets or sets the field name of a field belonging to a master table.
        /// <para>Used when this table is a detail table in a master-detail relation or when this is a join table.</para>
        /// </summary>
        public string MasterKeyField { get; set; } = "Id";
        /// <summary>
        /// Gets or sets the detail key field. A field that belongs to this table and mathes the <see cref="MasterTableName"/> primary key field.
        /// <para>It is used when this table is a detail table in a master-detail relation.</para>
        /// </summary>
        public string DetailKeyField { get; set; }

        /// <summary>
        /// The fields of this table
        /// </summary>
        public List<SqlBrokerFieldDef> Fields { get; set; } = new List<SqlBrokerFieldDef>();
        /// <summary>
        /// The list of join tables. 
        /// </summary>
        public List<SqlBrokerTableDef> JoinTables { get; set; } = new List<SqlBrokerTableDef>();
        /// <summary>
        /// The main table of a Broker (Item) is selected as 
        /// <para>  <c>select * from TABLE_NAME where ID = :ID</c></para>
        /// <para>
        /// If the table contains foreign keys, for instance CUSTOMER_ID etc, then those foreign tables are NOT joined. 
        /// The programmer who designs the UI just creates a Locator where needed.
        /// </para>
        /// <para>
        /// But there is always the need to have data from those foreign tables in many situations, i.e. in reports.
        /// </para>
        /// <para>
        /// StockTables are used for that. They are selected each time after the select of the main broker table (Item)          
        /// </para>
        /// </summary>
        public List<SqlBrokerQueryDef> StockTables { get; set; } = new List<SqlBrokerQueryDef>();

    }

}
