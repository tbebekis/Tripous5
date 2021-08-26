using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using Newtonsoft.Json;

using Tripous.Data;

namespace Tripous.Model2
{

#warning TODO: LocatorDescriptor, SqlBrowser, SqBroker JSON

    /// <summary>
    /// A broker table definition
    /// </summary>
    public class BrokerTableDef
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
        public BrokerTableDef()
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
            BrokerTableDef Empty = new BrokerTableDef();
            Sys.AssignObject(Empty, this);
        }
        /// <summary>
        /// Assigns property values from a source instance.
        /// </summary>
        public void Assign(BrokerTableDef Source)
        { 
            Sys.AssignObject(Source, this);
        }
        /// <summary>
        /// Returns a clone of this instance.
        /// </summary>
        public BrokerTableDef Clone()
        {
            BrokerTableDef Result = new BrokerTableDef();
            Sys.AssignObject(this, Result);
            return Result;
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
                        if (FieldDes.DataType.IsBlob() && ((Flags & BuildSqlFlags.BrowseBlobFields) == BuildSqlFlags.None))
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
        void BuildSql_AddJoinTable(NameValueStringList JoinTableNamesList, SelectSql SelectSql, string MasterAlias, BrokerTableDef JoinTableDes)
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
            BrokerFieldFlag Flags;
            string Title;

            foreach (DataColumn Field in Table.Columns)
            {
                var FieldDes = Fields.Find(item => item.Name.IsSameText(Field.ColumnName));

                if (FieldDes == null)
                {
                    Flags = BrokerFieldFlag.None;
                    Title = Res.GS(Field.ColumnName);
                    if (!Db.IsVisibleColumn(Field.ColumnName))
                        Flags |= BrokerFieldFlag.Hidden;

                    if (Simple.IsString(Field.DataType) || Simple.IsDateTime(Field.DataType))
                        Flags |= BrokerFieldFlag.Searchable;

                    Fields.Add(new BrokerFieldDef()
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
                    else if (FieldDes.DataType != DataFieldTypeHelper.DataFieldTypeOf(Field.DataType))
                    {
                        FieldDes.DataType = DataFieldTypeHelper.DataFieldTypeOf(Field.DataType);
                    }

                    if (FieldDes.DataType == DataFieldType.String && (Field.MaxLength != -1) && (FieldDes.MaxLength != Field.MaxLength))
                        FieldDes.MaxLength = Field.MaxLength;
                }

            }
        }


        /// <summary>
        /// Creates a DataTable based on a TableDescriptor.
        /// </summary>
        public void CreateDescriptorTable(SqlStore Store, List<MemTable> TableList, bool CreateLookUpTables)
        {
            MemTable Table = new MemTable() { TableName = this.Name };
            TableList.Add(Table);
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
                    CreateDescriptorTables_CreateLookUpTable(FieldDes, TableList);

                // joined table to TableDescriptor on this FieldDes
                BrokerTableDef JoinTableDes = this.FindAnyJoinTableByMasterKeyField(FieldDes.Name);
                if (JoinTableDes != null)
                    CreateDescriptorTables_AddJoinTableFields(JoinTableDes, Table);
            }

        }
        /// <summary>
        /// Sets up the <see cref="DataColumn.DefaultValue"/> of a specified column based on a specified field descriptor settings.
        /// </summary>
        void SetupDefaultValue(SqlStore Store, DataColumn Column, BrokerFieldDef FieldDes)
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
        void CreateDescriptorTables_CreateLookUpTable(BrokerFieldDef FieldDes, List<MemTable> TableList)
        { 
            MemTable Table = new MemTable() { TableName = FieldDes.ForeignTableAlias };
            TableList.Add(Table);
 
            DataColumn Column = new DataColumn(FieldDes.ForeignKeyField);
            Column.DataType = FieldDes.DataType.GetNetType();
            Column.MaxLength = FieldDes.MaxLength;
            Table.Columns.Add(Column); 
        }
        /// <summary>
        /// Adds join fields to the Table
        /// </summary>
        void CreateDescriptorTables_AddJoinTableFields(BrokerTableDef JoinTable, MemTable Table)
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
 
        /// <summary>
        /// Searces the whole joined tree for a table by a Name or Alias and returns
        /// a JoinTableDescriptor, if any, else null.
        /// </summary>
        public BrokerTableDef FindAnyJoinTable(string NameOrAlias)
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
        public BrokerTableDef FindJoinTableByMasterKeyField(string MasterKeyField)
        {
            return this.JoinTables.Find(item => item.MasterKeyField.IsSameText(MasterKeyField));
        }
        /// <summary>
        /// Searces the whole joined tree for a join table descriptor by MasterKeyField
        /// and returns that table, if any, else null.
        /// </summary>
        public BrokerTableDef FindAnyJoinTableByMasterKeyField(string MasterKeyField)
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
        /// Finds and returns, if exists, a TFieldDescriptorBase that has NameOrAlias
        /// Name or Alias. It searches this TableDes and its joined tables in the full tree.
        /// </summary>
        public BrokerFieldDef FindAnyField(string NameOrAlias)
        {
            var Result = Fields.Find(item => item.Name.IsSameText(NameOrAlias) || item.Alias.IsSameText(NameOrAlias));
            return Result != null? Result: FindAnyField(NameOrAlias, this.JoinTables);
        }
        /// <summary>
        /// Finds a field by Name or Alias by searching the whole tree of JoinTables tables.
        /// Returns null if a field not found.
        /// </summary>
        BrokerFieldDef FindAnyField(string NameOrAlias, List<BrokerTableDef> JoinTables)
        {
            BrokerFieldDef Result = null;

            foreach (var JoinTable in JoinTables)
            {
                Result = JoinTable.Fields.Find(item => item.Name.IsSameText(NameOrAlias) || item.Alias.IsSameText(NameOrAlias));

                if (Result == null && JoinTable.JoinTables != null)
                    Result = FindAnyField(NameOrAlias, JoinTable.JoinTables);

                if (Result != null)
                    break;
            }

            return Result;
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
        /// Gets or sets tha Title of this descriptor, used for display purposes.
        /// </summary>
        [JsonIgnore]
        public string Title => !string.IsNullOrWhiteSpace(TitleKey) ? Res.GS(TitleKey, TitleKey) : Name;
        /// <summary>
        /// Gets or sets a resource Key used in returning a localized version of Title
        /// </summary>
        public string TitleKey { get; set; }

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
        public List<BrokerFieldDef> Fields { get; set; } = new List<BrokerFieldDef>();
        /// <summary>
        /// The list of join tables. 
        /// </summary>
        public List<BrokerTableDef> JoinTables { get; set; } = new List<BrokerTableDef>();
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
        public List<BrokerQueryDef> StockTables { get; set; } = new List<BrokerQueryDef>();

    }

}
