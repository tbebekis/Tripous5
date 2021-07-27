/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Data;
using System.Collections.Generic;
using System.Threading;

using Tripous.Data;
 

namespace Tripous.Model
{

    /// <summary>
    /// A collection of business model related methods
    /// </summary>
    static public class Bm
    {
        static readonly int Spaces = 30;
        static private object syncLock = new LockObject();

        /// <summary>
        /// Initializes the business layer
        /// </summary>
        [Initializer()]
        static public void Initialize()
        {
            ObjectStore.RegisterObjectsOf(typeof(Bm).Assembly);
        }

        /// <summary>
        /// For thread synchronization
        /// </summary>
        static public void Lock()
        {
            Monitor.Enter(syncLock);
        }
        /// <summary>
        /// For thread synchronization
        /// </summary>
        static public void UnLock()
        {
            Monitor.Exit(syncLock);
        }


        static void BuildSql_Select(TableDescriptor TableDes, SelectSql SelectSql, BuildSqlFlags Flags, bool IsBrowserSelect)
        {


            // native fields
            string S = string.Empty;
            foreach (FieldDescriptor FieldDes in TableDes.Fields)
            {
                if (FieldDes.IsNativeField && !FieldDes.IsNoInsertOrUpdate)
                {
                    if (IsBrowserSelect)
                    {
                        if (FieldDes.DataType.IsBlob() && ((Flags & BuildSqlFlags.BrowseBlobFields) == BuildSqlFlags.None))
                            continue;
                    }

                    S = S + "  " + Sql.FormatFieldNameAlias(TableDes.Name, FieldDes.Name, FieldDes.Name, Spaces);
                }
            }

            SelectSql.Select = S;

            // native from
            SelectSql.From = "  " + Sql.FormatTableNameAlias(TableDes.Name, TableDes.Alias) + " " + Environment.NewLine;


            // if LookUp fields are to be handled as joined tables
            NameValueStringList JoinTableNamesList = new NameValueStringList();

            string JoinTableName;
            if ((Flags & BuildSqlFlags.JoinLookUpFields) == BuildSqlFlags.JoinLookUpFields)
            {
                foreach (FieldDescriptor FieldDes in TableDes.Fields)
                {
                    if (FieldDes.IsLookUpField)
                    {
                        JoinTableName = Sql.FormatTableNameAlias(FieldDes.LookUpTableName, FieldDes.LookUpTableAlias);
                        if (JoinTableNamesList.IndexOf(JoinTableName) == -1)
                        {
                            JoinTableNamesList.Add(JoinTableName);
                            SelectSql.From += string.Format("    left join {0} on {1}.{2} = {3}.{4} " + Environment.NewLine,
                                                                        JoinTableName,
                                                                        FieldDes.LookUpTableAlias,
                                                                        FieldDes.LookUpResultField,
                                                                        TableDes.Alias,
                                                                        FieldDes.Alias);
                        }

                        SelectSql.Select += "  " + Sql.FormatFieldNameAlias(FieldDes.LookUpTableAlias, FieldDes.LookUpResultField, FieldDes.Name, Spaces);
                    }
                }
            }

            // joined tables and fields
            foreach (JoinTableDescriptor JoinTableDes in TableDes.JoinTables)
                BuildSql_AddJoinTable(JoinTableNamesList, SelectSql, TableDes.Alias, JoinTableDes);


            // remove the last comma
            S = SelectSql.Select.TrimEnd(null);
            if ((S.Length > 1) && (S[S.Length - 1] == ','))
                S = S.Remove(S.Length - 1, 1);

            SelectSql.Select = S;
            SelectSql.From = SelectSql.From.TrimEnd(null);






        }

        /* sql generation */
        /// <summary>
        /// Called by BuildSql to handle join tables
        /// </summary>
        static private void BuildSql_AddJoinTable(NameValueStringList JoinTableNamesList, SelectSql SelectSql, string MasterAlias, JoinTableDescriptor JoinTableDes)
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
            foreach (JoinFieldDescriptor JoinFieldDes in JoinTableDes.Fields)
            {
                if (!Sys.IsSameText(JoinFieldDes.Name, JoinTableDes.PrimaryKeyField))
                {
                    SelectSql.Select += "  " + Sql.FormatFieldNameAlias(JoinTableDes.Alias, JoinFieldDes.Name, JoinFieldDes.Alias, Spaces);
                }
            }

            // joined tables to this join table
            foreach (JoinTableDescriptor JoinTableDescriptor in JoinTableDes.JoinTables)
                BuildSql_AddJoinTable(JoinTableNamesList, SelectSql, JoinTableDes.Alias, JoinTableDescriptor);

        }
        /// <summary>
        /// Generates SQL statements using the TableDes descriptor and the Flags
        /// </summary>
        static public void BuildSql(TableDescriptor TableDes, SqlStatements Statements, BuildSqlFlags Flags)
        {
            Statements.Clear();

            /* field lists preparation */
            string S = "";              // insert field list
            string S2 = "";             // insert params field list
            string S3 = "";             // update field list AND update params field list
            string FieldName = "";

            bool GuidOid = Bf.Member(BuildSqlFlags.GuidOids, Flags) || TableDes.Fields[TableDes.PrimaryKeyField].DataType.IsString();
            bool OidModeIsBefore = !GuidOid && ((Flags & BuildSqlFlags.OidModeIsBefore) == BuildSqlFlags.OidModeIsBefore);

            foreach (FieldDescriptor FieldDes in TableDes.Fields)
            {
                if (FieldDes.IsNativeField && !FieldDes.IsNoInsertOrUpdate)
                {
                    FieldName = FieldDes.Name;

                    if (!Sys.IsSameText(FieldName, TableDes.PrimaryKeyField))
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
            Statements.Insert = string.Format(SqlText, TableDes.Name, S, S2);

            /* Update */
            SqlText = "update {0} " + Environment.NewLine + "set {1} " + Environment.NewLine + "where " + Environment.NewLine + "  {2} = :{2} ";
            Statements.Update = string.Format(SqlText, TableDes.Name, S3, TableDes.PrimaryKeyField);

            /* Delete */
            Statements.Delete = string.Format("delete from {0} where {1} = :{2}", TableDes.Name, TableDes.PrimaryKeyField, TableDes.PrimaryKeyField);

            /* RowSelect */
            SelectSql RowSql = new SelectSql();
            BuildSql_Select(TableDes, RowSql, Flags, false);
            RowSql.Where = string.Format("where {0}.{1} = :{1}", TableDes.Name, TableDes.PrimaryKeyField);
            Statements.RowSelect = RowSql.Text;


            /* Browse */
            BuildSql_Select(TableDes, Statements.BrowseSelect, Flags, true);
            // it is a detail table 
            bool IsDetailTable = !string.IsNullOrWhiteSpace(TableDes.MasterTableName)
                                && !string.IsNullOrWhiteSpace(TableDes.MasterKeyField)
                                && !string.IsNullOrWhiteSpace(TableDes.DetailKeyField);

            if (IsDetailTable)
            {
                Statements.BrowseSelect.Where = string.Format("{0}.{1} = :{2}",
                    TableDes.Alias, TableDes.DetailKeyField, Sys.MASTER_KEY_FIELD_NAME);
            }


        }
        /// <summary>
        /// Constructs SQL statements based on a TableDescriptor that is created
        /// and updated based on the TableName and Table
        /// </summary>
        static public void BuildSql(string TableName, DataTable Table, SqlStatements Statements, BuildSqlFlags Flags)
        {
            TableDescriptor TableDes = new TableDescriptor();
            TableDes.Name = TableName;
            UpdateTableDescriptorFrom(Table, TableDes);
            BuildSql(TableDes, Statements, Flags);
        }

        /* table descriptors */
        static private void SetupDefaultValue(SqlStore Store, DataColumn Column, FieldDescriptor FieldDes)
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
        static private void CreateDescriptorTables_CreateLookUpTable(FieldDescriptor FieldDes, Tables TableList)
        {
            MemTable Table = TableList.Add(FieldDes.LookUpTableAlias);

            DataColumn Column = new DataColumn(FieldDes.LookUpResultField);
            Column.DataType = FieldDes.Table.Fields.Find(FieldDes.Name).DataType.GetNetType();
            Column.MaxLength = FieldDes.Table.Fields.Find(FieldDes.Name).Size;
            Table.Columns.Add(Column);

            Column = new DataColumn(FieldDes.LookUpResultField);
            Column.DataType = typeof(System.String);
            Column.MaxLength = 96;
            Table.Columns.Add(Column);
        }
        /// <summary>
        /// Adds join fields to the Table
        /// </summary>
        static private void CreateDescriptorTables_AddJoinTableFields(JoinTableDescriptor JoinTable, MemTable Table)
        {
            DataColumn Column;
            foreach (JoinFieldDescriptor JoinFieldDes in JoinTable.Fields)
            {
                if (!Sys.IsSameText(JoinTable.PrimaryKeyField, JoinFieldDes.Name))
                {
                    Column = new DataColumn(JoinFieldDes.Alias);
                    Column.ExtendedProperties["Descriptor"] = JoinFieldDes;
                    Column.DataType = JoinFieldDes.DataType.GetNetType();
                    Column.MaxLength = JoinFieldDes.Size;
                    Column.Caption = JoinFieldDes.Title;

                    Table.Columns.Add(Column);

                    // joined table to JoinTable on this JoinFieldDes
                    JoinTableDescriptor JoinTableDes2 = JoinTable.JoinTables.FindAnyByMasterKeyField(JoinFieldDes.Name);
                    if (JoinTableDes2 != null)
                        CreateDescriptorTables_AddJoinTableFields(JoinTableDes2, Table);
                }
            }

            // joined tables to this joined table
            //foreach (JoinTableDescriptor JoinTableDescriptor in JoinTable.JoinTables)
            //    CreateDescriptorTables_AddJoinTableFields(JoinTableDescriptor, Table);
        }
        /// <summary>
        /// Creates a DataTable based on a TableDescriptor.
        /// </summary>
        static public void CreateDescriptorTable(SqlStore Store, TableDescriptor TableDescriptor, Tables TableList, bool CreateLookUpTables)
        {
            MemTable Table = TableList.Add(TableDescriptor.Name);
            Table.ExtendedProperties["Descriptor"] = TableDescriptor;

            Table.PrimaryKeyField = TableDescriptor.PrimaryKeyField;
            Table.MasterKeyField = TableDescriptor.MasterKeyField;
            Table.DetailKeyField = TableDescriptor.DetailKeyField;
            DataColumn Column;

            // native fields and lookups
            foreach (FieldDescriptor FieldDes in TableDescriptor.Fields)
            {
                Column = new DataColumn(FieldDes.Name);
                Column.ExtendedProperties["Descriptor"] = FieldDes;
                Column.DataType = FieldDes.DataType.GetNetType();
                if (Sys.IsSameText(TableDescriptor.PrimaryKeyField, FieldDes.Name) && (FieldDes.DataType == SimpleType.Integer))
                {
                    Column.AutoIncrement = true;
                    Column.AutoIncrementSeed = -1;
                    Column.AutoIncrementStep = -1;
                }
                if (Column.DataType == typeof(System.String))
                    Column.MaxLength = FieldDes.Size;
                Column.Caption = FieldDes.Title;

                SetupDefaultValue(Store, Column, FieldDes);

                Table.Columns.Add(Column);

                //if (!string.IsNullOrEmpty(FieldDes.Expression))
                //    Column.Expression = FieldDes.Expression;

                if (CreateLookUpTables && FieldDes.IsLookUpField)
                    CreateDescriptorTables_CreateLookUpTable(FieldDes, TableList);

                // joined table to TableDescriptor on this FieldDes
                JoinTableDescriptor JoinTableDes = TableDescriptor.JoinTables.FindAnyByMasterKeyField(FieldDes.Name);
                if (JoinTableDes != null)
                    CreateDescriptorTables_AddJoinTableFields(JoinTableDes, Table);
            }


            // joined tables to this table
            //foreach (JoinTableDescriptor JoinTableDescriptor in TableDescriptor.JoinTables)
            //    CreateDescriptorTables_AddJoinTableFields(JoinTableDescriptor, Table);
        }
        /// <summary>
        /// Updates TableDes information using Table schema.
        /// </summary>
        static public void UpdateTableDescriptorFrom(DataTable Table, TableDescriptor TableDes)
        {
            FieldFlags Flags;
            string Title;


            foreach (DataColumn Field in Table.Columns)
            {
                FieldDescriptor FieldDes = TableDes.Fields.Find(Field.ColumnName);

                if (FieldDes == null)
                {
                    Flags = FieldFlags.None;
                    Title = Res.GS(Field.ColumnName);
                    if (Db.IsVisibleColumn(Field.ColumnName))
                        Flags |= FieldFlags.Visible;

                    if (Simple.IsString(Field.DataType) || Simple.IsDateTime(Field.DataType))
                        Flags |= FieldFlags.Searchable;

                    TableDes.Fields.Add(Field.ColumnName, Simple.SimpleTypeOf(Field.DataType), Field.MaxLength, Title, Flags);
                }
                else
                {

                    if (FieldDes.DataType.IsDateTime() && Field.DataType == typeof(DateTime))
                    {
                        // let FieldDes.DataType keep its original value
                    }
                    else if (FieldDes.DataType != Simple.SimpleTypeOf(Field.DataType))
                    {
                        FieldDes.DataType = Simple.SimpleTypeOf(Field.DataType);
                    }

                    if (FieldDes.DataType.IsString() && (Field.MaxLength != -1) && (FieldDes.Size != Field.MaxLength))
                        FieldDes.Size = Field.MaxLength;
                }

            }
        }


        /* criteria */
        /// <summary>
        /// Creates <see cref="SqlFilter"/> items for each searchable <see cref="DataColumn"/> of the Table.
        /// </summary>
        static public void TableToCriteriaItems(DataTable Table, SqlFilters Filters)
        {
            foreach (DataColumn Field in Table.Columns)
            {
                if (Field.IsVisible() && ((Field.DataType == typeof(System.String)) || (Field.DataType == typeof(DateTime))))
                    Filters.Add(Table.TableName, Field.ColumnName, Field.Caption, Field.DataType);
            }
        }
        /// <summary>
        /// Creates <see cref="SqlFilter"/> items for each searchable <see cref="FieldDescriptorBase"/> item of the TableDes.
        /// </summary>
        static public void CreateSqlFilters(DataTable Table, TableDescriptor TableDes, SqlFilters Filters)
        {
            SimpleType SimpleType;
            FieldDescriptorBase FieldDes;
            SqlFilter Filter;

            foreach (DataColumn Field in Table.Columns)
            {
                SimpleType = Simple.SimpleTypeOf(Field.DataType);
                if (SimpleType.IsString() || SimpleType.IsNumeric() || SimpleType.IsDateTime())
                {
                    FieldDes = TableDes.FindAnyField(Field.ColumnName);
                    if ((FieldDes != null) && (FieldDes.IsSearchable))
                    {
                        if (SimpleType.IsString() && ((Field.MaxLength > 100) || (FieldDes.Size > 100)))
                            continue;

                        Filter = Filters.Add(FieldDes.TableAlias, FieldDes.Name, FieldDes.Title, Field.DataType);
                        if (FieldDes.IsBoolean)
                            Filter.DataType = SimpleType.Boolean;
                    }
                }
            }
        }

        /* miscs */
        /// <summary>
        /// Setups column titles for Table columns using the DisplayLabels string and the TableDes TableDescriptor.
        /// </summary>
        static public void SetupFieldsDisplayLabelsFor(DataTable Table, string DisplayLabels, TableDescriptor TableDes)
        {
            SetupFieldsDisplayLabelsFor(Table, new NameValueStringList(DisplayLabels), TableDes);
        }
        /// <summary>
        /// Setups column titles for Table columns using the DisplayLabes dictionary and the TableDes TableDescriptor.
        /// </summary>
        static public void SetupFieldsDisplayLabelsFor(DataTable Table, Dictionary<string, string> DisplayLabels, TableDescriptor TableDes)
        {
            foreach (DataColumn Field in Table.Columns)
            {
                // if Column.Caption is not defined in some way
                if (string.IsNullOrEmpty(Field.Caption) || Sys.IsSameText(Field.ColumnName, Field.Caption))
                {
                    // first look to the DisplayLabels 
                    if ((DisplayLabels.ContainsKey(Field.ColumnName)) && !string.IsNullOrEmpty(DisplayLabels[Field.ColumnName]))
                    {
                        Field.Caption = DisplayLabels[Field.ColumnName];
                    }
                    // and then look to ANY field (joins included) of the TableDes
                    else if ((TableDes != null) && !string.IsNullOrEmpty(TableDes.FindAnyFieldTitle(Field.ColumnName)))
                    {
                        Field.Caption = TableDes.FindAnyFieldTitle(Field.ColumnName);
                    }
                }
            }
        }
        /// <summary>
        /// Setups column titles for Table columns using the DisplayLabes dictionary and the TableDes TableDescriptor.
        /// </summary>
        static public void SetupFieldsDisplayLabelsFor(DataTable Table, NameValueStringList DisplayLabels, TableDescriptor TableDes)
        {
            string Caption;

            foreach (DataColumn Field in Table.Columns)
            {
                // if Column.Caption is not defined in some way
                if (string.IsNullOrEmpty(Field.Caption) || Sys.IsSameText(Field.ColumnName, Field.Caption))
                {
                    /* DataColumn.Caption property seems to be case-insensitive.   
                       That is, assigning a "Phone" caption to an DataColumn.Caption 
                       already captioned as "PHONE" 
                       leaves the old "PHONE" intact. */
                    Caption = Field.Caption;
                    Field.Caption = "temp";

                    // first look to the DisplayLabels 
                    if ((DisplayLabels.ContainsName(Field.ColumnName)) && !string.IsNullOrEmpty(DisplayLabels.Values[Field.ColumnName]))
                    {
                        Caption = DisplayLabels.Values[Field.ColumnName];
                    }
                    // and then look to ANY field (joins included) of the TableDes
                    else if ((TableDes != null) && !string.IsNullOrEmpty(TableDes.FindAnyFieldTitle(Field.ColumnName)))
                    {
                        Caption = TableDes.FindAnyFieldTitle(Field.ColumnName);
                    }

                    Field.Caption = Caption;
                }
            }
        }

 

        /// <summary>
        /// Returns the Broker of the specified ClientObject 
        /// <para>NOTE: ClientObject may be either a Broker or a DataEntryForm </para>
        /// </summary>
        static public Broker BrokerOf(object ClientObject)
        {
            if (ClientObject != null)
            {
                if (ClientObject is Broker)
                {
                    return ClientObject as Broker;
                }

                if (ClientObject is IBrokerContainer)
                {
                    return (ClientObject as IBrokerContainer).Broker;
                }

                if (Sys.HasProperty(ClientObject, "Broker"))
                {
                    return Sys.GetProperty(ClientObject, "Broker") as Broker;
                }
            }

            return null;
        }
        /// <summary>
        /// Returns the SqlBroker of the specified ClientObject
        /// <para>NOTE: ClientObject may be either a Broker or a DataEntryForm </para>
        /// </summary>
        static public SqlBroker SqlBrokerOf(object ClientObject)
        {
            return BrokerOf(ClientObject) as SqlBroker;
        }

    }
}
