/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

using Tripous.Data.Metadata;

namespace Tripous.Data
{


    /// <summary>
    /// A collection of static utility methods regarding data access
    /// </summary>
    static public class Db
    {
        static int datasetCount = 0;
        /// <summary>
        /// Constant
        /// </summary>
        static public readonly string StandardDefaultValues = "CompanyId;EmptyString;AppDate;SysDate;SysTime;DbServerTime;AppUserName;AppUserId;NetUserName;Guid";

        static List<SqlConnectionInfo> fConnections;
        static DbIni fMainIni;

        /// <summary>
        /// Initializes the data access layer
        /// </summary>
        [Initializer()]
        static public void Initialize()
        {
            ObjectStore.RegisterObjectsOf(typeof(Db).Assembly);
        }


        /// <summary>
        /// Returns the default connection string, if any, else throws an exception.
        /// </summary>
        static public SqlConnectionInfo GetDefaultConnectionInfo()
        {
            return GetConnectionInfo(SysConfig.DefaultConnection);
        }
        /// <summary>
        /// Finds and returns a connection string specified by name, if any, else null.
        /// </summary>
        static public SqlConnectionInfo FindConnectionInfo(string ConnectionName)
        {
            SqlConnectionInfo ConnectionInfo = Db.Connections.FirstOrDefault(item => string.Compare(ConnectionName, item.Name, StringComparison.InvariantCultureIgnoreCase) == 0);
            return ConnectionInfo;
        }
        /// <summary>
        /// Returns a connection string specified by name, if any, else throws an exception.
        /// </summary>
        static public SqlConnectionInfo GetConnectionInfo(string ConnectionName)
        {
            SqlConnectionInfo ConnectionInfo = FindConnectionInfo(ConnectionName);
            if (ConnectionInfo == null)
                throw new ApplicationException($"Connection string not registered. Connection name: {ConnectionName}");
            return ConnectionInfo;
        }


        /// <summary>
        /// Creates, opens and returns the default <see cref="DbConnection"/> connection
        /// </summary>
        static public DbConnection OpenDefaultConnection()
        {
            return OpenConnection(SysConfig.DefaultConnection);
        }
        /// <summary>
        /// Creates, opens and returns a connection based on a connection name.
        /// <para>The connection information must exist in the application config under the specified name, or an exception is thrown.</para>
        /// </summary>
        static public DbConnection OpenConnection(string ConnectionName)
        {
            return OpenConnection(GetConnectionInfo(ConnectionName));
        }
        /// <summary>
        /// Creates, opens and returns a database connection based on a connection info
        /// </summary>
        static public DbConnection OpenConnection(SqlConnectionInfo ConnectionInfo)
        {
           return ConnectionInfo.GetSqlProvider().OpenConnection(ConnectionInfo.ConnectionString);
        }
 
        /// <summary>
        /// Adds an input DbParameter to a DbCommand
        /// </summary>
        static public DbParameter AddInputParam(DbCommand Cmd, string ParamName, DbType Type, object Value)
        {
            var P = Cmd.CreateParameter();
            P.ParameterName = ParamName;
            P.Direction = ParameterDirection.Input;
            P.DbType = Type;
            P.Value = Value;
            Cmd.Parameters.Add(P);

            return P;
        }
        /// <summary>
        /// Adds an output DbParameter to a DbCommand
        /// </summary>
        static public DbParameter AddOutputParam(DbCommand Cmd, string ParamName, DbType Type)
        {
            var P = Cmd.CreateParameter();
            P.ParameterName = ParamName;
            P.Direction = ParameterDirection.Output;
            P.DbType = Type;
            Cmd.Parameters.Add(P);

            return P;
        }

        /* select and exec sql */
        /// <summary>
        /// Executes the SELECT SqlText with Params and returns a DataTable.
        /// <para>
        /// Passes Params values to the created Command Parameters.
        /// Params can be
        /// 1. either a comma separated list of parameters 
        /// 2. or the Params[0] element, that is the first element in Params, may be a DataRow, generic IDictionary, IList or Array
        /// and in that case no other Params elements are used.
        /// </para>
        /// </summary> 
        static public DataTable Select(string ConnectionString, string SqlText, params object[] Params)
        {
            return GetConnectionInfo(ConnectionString).GetSqlProvider().Select(ConnectionString, SqlText, Params);
        }
        /// <summary>
        /// Executes the SELECT SqlText and returns a DataTable.
        /// <para>ConnectionString must include a DataProvider Alias</para>
        /// </summary> 
        static public DataTable Select(string ConnectionString, string SqlText)
        {
            return Select(ConnectionString, SqlText, null);
        }
 

        /// <summary>
        /// Executes SqlText and returns the first DataRow of the result set.
        /// <para>WARNING: If SqlText returns no rows at all then this method returns null.</para>
        /// <para></para>
        /// <para>Params can be: </para>
        /// <para>1. either a comma separated C# params list</para>
        /// <para>2. or the Params[0] element, that is the first element in Params, may be a DataRow, generic IDictionary, IList or Array
        /// and in that case no other Params elements are used.</para>
        /// </summary>
        static public DataRow SelectResults(string ConnectionString, string SqlText, params object[] Params)
        {
            DataTable Table = Select(ConnectionString, SqlText, Params);
            if (Table.Rows.Count > 0)
                return Table.Rows[0];
            else
                return null;
        }
        /// <summary>
        /// Executes SqlText and returns the first DataRow of the result set.
        /// <para>WARNING: If SqlText returns no rows at all then this method returns null.</para>
        /// </summary>
        static public DataRow SelectResults(string ConnectionString, string SqlText)
        {
            return SelectResults(ConnectionString, SqlText, null);
        }

        /// <summary>
        /// Ideal for executing SELECT statements of the type "select FIELD_NAME from TABLE_NAME"
        /// </summary>
        static public object SelectResult(string ConnectionString, string SqlText, object Default, params object[] Params)
        {
            object Result = Default;

            DataRow Row = SelectResults(ConnectionString, SqlText, Params);
            if ((Row != null) && !Row.IsNull(0))
            {
                Result = Row[0];
            }

            return Result;
        }
        /// <summary>
        /// Ideal for executing SELECT statements of the type "select FIELD_NAME from TABLE_NAME"
        /// </summary>
        static public object SelectResult(string ConnectionString, string SqlText, object Default)
        {
            return SelectResult(ConnectionString, SqlText, Default, null);
        }
        /// <summary>
        /// Ideal for executing SELECT statements of the type "select FIELD_NAME from TABLE_NAME"
        /// </summary>
        static public object SelectResult(string ConnectionString, string SqlText)
        {
            return SelectResult(ConnectionString, SqlText, DBNull.Value);
        }

        /// <summary>
        /// Ideal for executing SELECT statements of the type "select count(ID) as COUNT_ID from TABLE_NAME where ID = 1234"
        /// </summary>
        static public int IntegerResult(string ConnectionString, string SqlText, int Default, params object[] Params)
        {
            string S = SelectResult(ConnectionString, SqlText, Default, Params).ToString();
            return Sys.StrToInt(S, Default);
        }
        /// <summary>
        /// Ideal for executing SELECT statements of the type "select count(ID) as COUNT_ID from TABLE_NAME where ID = 1234"
        /// </summary>
        static public int IntegerResult(string ConnectionString, string SqlText, int Default)
        {
            return IntegerResult(ConnectionString, SqlText, Default, null);
        }

        /// <summary>
        /// Executes the DML SqlText with Params.
        /// <para>
        /// Passes Params values to the created Command Parameters.
        /// Params can be
        /// 1. either a comma separated list of parameters 
        /// 2. or the Params[0] element, that is the first element in Params, may be a DataRow, generic IDictionary, IList or Array
        /// and in that case no other Params elements are used.
        /// </para>
        /// </summary> 
        static public void ExecSql(string ConnectionString, string SqlText, params object[] Params)
        {
            GetConnectionInfo(ConnectionString).GetSqlProvider().ExecSql(ConnectionString, SqlText, Params);
        }
        /// <summary>
        /// Executes the DML SqlText.
        /// <para>ConnectionString must include a DataProvider Alias</para>
        /// </summary> 
        static public void ExecSql(string ConnectionString, string SqlText)
        {
            ExecSql(ConnectionString, SqlText, null);
        }

        /* buils sql statements */
        /// <summary>
        /// Generates Sql statements for the Table.     
        /// </summary>
        static public void BuildSql(string TableName, string PrimaryKeyField, bool IsStringKey, SqlStore Store, SqlStatements SqlStatements, bool IsTopTable)
        {
            string LB = Environment.NewLine;
            BuildSqlFlags Flags = BuildSqlFlags.None;

            if (Store.Provider.OidMode == OidMode.Before)
                Flags |= BuildSqlFlags.OidModeIsBefore;

            DataTable SchemaTable = new DataTable("SchemaTable");
            Store.GetNativeSchema("", TableName, TableName, SchemaTable);

            bool GuidOid = IsStringKey || Simple.SimpleTypeOf(SchemaTable.Columns[PrimaryKeyField].DataType).IsString();
            bool OidModeIsBefore = !GuidOid && ((Flags & BuildSqlFlags.OidModeIsBefore) == BuildSqlFlags.OidModeIsBefore);


            /* DeleteSQL */
            SqlStatements.Delete = string.Format("delete from {0} where {1} = :{1}", TableName, PrimaryKeyField);


            /* InsertSQL */
            string S = "";              // insert field list
            string S2 = "";             // insert params field list
            string S3 = "";             // update field list AND update params field list
            string FieldName = "";

            for (int i = 0; i < SchemaTable.Columns.Count; i++)
            {
                FieldName = SchemaTable.Columns[i].ColumnName;

                if (!Sys.IsSameText(FieldName, PrimaryKeyField))
                {
                    S = S + LB + "  " + FieldName + ", ";
                    S2 = S2 + LB + "  " + ":" + FieldName + ", ";
                    S3 = S3 + LB + "  " + FieldName + " = :" + FieldName + ", ";
                }
                else if (GuidOid || OidModeIsBefore)
                {
                    S = S + LB + "  " + FieldName + ", ";
                    S2 = S2 + LB + "  " + ":" + FieldName + ", ";
                }

            }


            if (S.Length > 2)
                S = S.Remove(S.Length - 2, 2);
            //S = S + LB;

            if (S2.Length > 2)
                S2 = S2.Remove(S2.Length - 2, 2);
            //S2 = S2 + LB;

            if (S3.Length > 2)
                S3 = S3.Remove(S3.Length - 2, 2);

            string SQL = "insert into {0} ( " + LB + "{1}" + LB + " ) values ( " + LB + "{2}" + LB + " ) ";
            SqlStatements.Insert = string.Format(SQL, TableName, S, S2);

            /* UpdateSQL */
            SQL = "update {0} " + LB + "set {1} " + LB + "where " + LB + "  {2} = :{2} ";
            SqlStatements.Update = string.Format(SQL, TableName, S3, PrimaryKeyField);

            /* TopTable RowSelect */
            if (IsTopTable && string.IsNullOrEmpty(SqlStatements.RowSelect))
                SqlStatements.RowSelect = string.Format("select * from {0} where {1} = :{1}", TableName, PrimaryKeyField);

            /* All tables */
            if (SqlStatements.BrowseSelect.IsEmpty)
                SqlStatements.BrowseSelect.ParseFromTableName(TableName);
        }
        /// <summary>
        /// Generates Sql statements for the Table.     
        /// </summary>
        static public void BuildSql(MemTable Table, SqlStore Store, bool IsTopTable)
        {
            BuildSql(Table.TableName, Table.PrimaryKeyField, Table.IsStringPrimaryKey, Store, Table.SqlStatements, IsTopTable);
        }
        /// <summary>
        /// Generates Sql statements for the Table.     
        /// <para>WARNING: Assumes that Table primary key field is named Id.</para>
        /// </summary>
        static public SqlStatements BuildSql(DataTable Table, SqlStore Store, bool IsTopTable)
        {
            SqlStatements Result = new SqlStatements();
            BuildSql(Table.TableName, "Id", Table.IsStringField("Id"), Store, Result, IsTopTable);
            return Result;
        }

        /// <summary>
        /// Commits (INSERT and UPDATE only) Row to TableName.
        /// <para>Handles primary key generation.</para>
        /// </summary>
        static public void Commit(DataRow Row, string TableName, string PrimaryKeyField, bool IsStringKey, SqlStore Store, SqlStatements SqlStatements)
        {
            if (Row != null)
            {
                if (IsStringKey)
                {
                    string SqlText = string.Format("select {0} from {1} where {0} = ", PrimaryKeyField, TableName);

                    if (Sys.IsNull(Row[PrimaryKeyField]) || string.IsNullOrEmpty(Row[PrimaryKeyField].ToString()) || ((string)Store.SelectResult(SqlText + Row[PrimaryKeyField].ToString().QS(), string.Empty) == string.Empty))
                    {
                        Row[PrimaryKeyField] = Sys.GenId();
                        Store.ExecSql(SqlStatements.Insert, Row);
                    }
                    else
                    {
                        Store.ExecSql(SqlStatements.Update, Row);
                    }
                }
                else
                {
                    if (Sys.IsNull(Row[PrimaryKeyField]) || (Sys.AsInteger(Row[PrimaryKeyField], -1) <= 0))
                    {
                        if (Store.Provider.OidMode == OidMode.Before)
                            Row[PrimaryKeyField] = Store.NextId(TableName);

                        Store.ExecSql(SqlStatements.Insert, Row);

                        if (Store.Provider.OidMode == OidMode.Along)
                            Row[PrimaryKeyField] = Store.LastId(TableName);
                    }
                    else
                    {
                        Store.ExecSql(SqlStatements.Update, Row);
                    }
                }
            }
        }
        /// <summary>
        /// Commits (INSERT and UPDATE only) all rows of the Table
        /// <para>Handles primary key generation.</para>
        /// </summary>
        static public void Commit(DataTable Table, string TableName, string PrimaryKeyField, bool IsStringKey, SqlStore Store, SqlStatements SqlStatements)
        {
            if (Table != null)
            {
                string SqlText = string.Format("select {0} from {1} where {0} = ", PrimaryKeyField, TableName);

                using (DbTransaction Transaction = Store.BeginTransaction())
                {
                    try
                    {
                        foreach (DataRow Row in Table.Rows)
                        {
                            if (IsStringKey)
                            {
                                if (Sys.IsNull(Row[PrimaryKeyField]) || string.IsNullOrEmpty(Row.AsString(PrimaryKeyField)) || ((string)Store.SelectResult(SqlText + Row[PrimaryKeyField].ToString().QS(), string.Empty) == string.Empty))
                                {
                                    Row[PrimaryKeyField] = Sys.GenId();
                                    Store.ExecSql(Transaction, SqlStatements.Insert, Row);
                                }
                                else
                                {
                                    Store.ExecSql(Transaction, SqlStatements.Update, Row);
                                }
                            }
                            else
                            {
                                if (Sys.IsNull(Row[PrimaryKeyField]) || (Sys.AsInteger(Row[PrimaryKeyField], -1) <= 0))
                                {
                                    if (Store.Provider.OidMode == OidMode.Before)
                                        Row[PrimaryKeyField] = Store.NextId(TableName);

                                    Store.ExecSql(Transaction, SqlStatements.Insert, Row);

                                    if (Store.Provider.OidMode == OidMode.Along)
                                        Row[PrimaryKeyField] = Store.LastId(TableName);
                                }
                                else
                                {
                                    Store.ExecSql(Transaction, SqlStatements.Update, Row);
                                }
                            }
                        }

                        Transaction.Commit();
                    }
                    catch
                    {
                        Transaction.Rollback();
                        throw;
                    }
                }
            }
        }


        /* to/from base64 */
        /// <summary>
        /// Converts Table to Base64 string
        /// </summary>
        static public string TableToToBase64(DataTable Table)
        {
            if (Table != null)
            {
                using (MemoryStream MS = new MemoryStream())
                {
                    Table.WriteXml(MS, XmlWriteMode.WriteSchema);
                    return Convert.ToBase64String(MS.ToArray());
                }
            }

            return string.Empty;
        }
        /// <summary>
        /// Converts the Base64 Text to a DataTable
        /// </summary>
        static public DataTable Base64ToTable(string Text)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                using (MemoryStream MS = new MemoryStream(Convert.FromBase64String(Text)))
                {
                    MS.Position = 0;
                    DataTable Table = new MemTable("");
                    Table.ReadXml(MS);
                    Table.AcceptChanges();
                    return Table;
                }
            }

            return null;
        }
        /// <summary>
        /// Converts DataSet to Base64 string
        /// </summary>
        static public string DataSetToToBase64(DataSet DS)
        {
            if (DS != null)
            {
                using (MemoryStream MS = new MemoryStream())
                {
                    DS.WriteXml(MS, XmlWriteMode.WriteSchema);
                    return Convert.ToBase64String(MS.ToArray());
                }
            }

            return string.Empty;
        }
        /// <summary>
        /// Converts the Base64 Text to a DataSet
        /// </summary>
        static public DataSet Base64ToDataSet(string Text)
        {
            if (!string.IsNullOrEmpty(Text))
            {
                using (MemoryStream MS = new MemoryStream(Convert.FromBase64String(Text)))
                {
                    MS.Position = 0;
                    DataSet ds = new DataSet("DataSet");
                    ds.ReadXml(MS);
                    ds.AcceptChanges();
                    return ds;
                }
            }

            return null;
        }

        /* DataViews */
        /// <summary>
        /// Finds and returns a value in DataView by Key.
        /// <para>NOTE: DataView.Sort sould be set already to a field name. 
        /// Key is a value in that field.</para>
        /// <para>If Key not found or is null then Default is returned</para>
        /// </summary>
        static public T FindViewValue<T>(DataView View, object[] Key, string ValueField, T Default)
        {
            T Result = Default;

            if (!Sys.IsNull(Key))
            {
                int Index = View.Find(Key);
                if (Index != -1)
                {
                    object Value = View[Index].Row[ValueField];
                    if (!Sys.IsNull(Value))
                    {
                        if (typeof(T) == typeof(int))
                            Value = Convert.ToInt32(Value);
                        else
                            Value = Convert.ToString(Value);

                        Result = (T)Value;
                    }
                }
            }

            return Result;
        }
        /// <summary>
        /// Finds and returns a value in DataView by Key.
        /// <para>NOTE: DataView.Sort sould be set already to a field name. 
        /// Key is a value in that field.</para>
        /// <para>If Key not found or is null then Default is returned</para>
        /// </summary>
        static public T FindViewValue<T>(DataView View, object Key, string ValueField, T Default)
        {
            T Result = Default;

            if (!Sys.IsNull(Key))
            {
                int Index = View.Find(Key);
                if (Index != -1)
                {
                    object Value = View[Index].Row[ValueField];
                    if (!Sys.IsNull(Value))
                    {
                        if (typeof(T) == typeof(int))
                            Value = Convert.ToInt32(Value);
                        else
                            Value = Convert.ToString(Value);

                        Result = (T)Value;
                    }
                }
            }

            return Result;
        }
        /// <summary>
        /// Finds and returns a value in DataView by Key.
        /// <para>NOTE: DataView.Sort sould be set already to a field name. 
        /// Key is a value in that field.</para>
        /// <para>If Key not found or is null then DBNull.Value is returned</para>
        /// </summary>
        static public object FindViewValue(DataView View, object[] Key, string ValueField)
        {
            object Result = DBNull.Value;

            if (!Sys.IsNull(Key))
            {
                int Index = View.Find(Key);
                if (Index != -1)
                {
                    object Value = View[Index].Row[ValueField];
                    if (!Sys.IsNull(Value))
                        Result = Value;
                }
            }

            return Result;
        }
        /// <summary>
        /// Finds and returns a value in DataView by Key.
        /// <para>NOTE: DataView.Sort sould be set already to a field name. 
        /// Key is a value in that field.</para>
        /// <para>If Key not found or is null then DBNull.Value is returned</para>
        /// </summary>
        static public object FindViewValue(DataView View, object Key, string ValueField)
        {
            object Result = DBNull.Value;

            if (!Sys.IsNull(Key))
            {
                int Index = View.Find(Key);
                if (Index != -1)
                {
                    object Value = View[Index].Row[ValueField];
                    if (!Sys.IsNull(Value))
                        Result = Value;
                }
            }

            return Result;
        }

        /// <summary>
        /// Finds and returns a DataRow in the DataView.
        /// <para>DataView MUST be Sort-ed by a certain column.
        /// Key is a value from that column</para>
        /// </summary>
        static public DataRow FindDataRow(DataView View, object[] Key)
        {
            if (!Sys.IsNull(Key))
            {
                int Index = View.Find(Key);
                if (Index != -1)
                {
                    return View[Index].Row;
                }
            }

            return null;
        }
        /// <summary>
        /// Finds and returns a DataRow in the DataView.
        /// <para>DataView MUST be Sort-ed by a certain column.
        /// Key is a value from that column</para>
        /// </summary>
        static public DataRow FindDataRow(DataView View, object Key)
        {
            if (!Sys.IsNull(Key))
            {
                int Index = View.Find(Key);
                if (Index != -1)
                {
                    return View[Index].Row;
                }
            }

            return null;
        }

        /* Table.DefaultView */
        /// <summary>
        /// Finds and returns a value in Table.DefaultView by Key.
        /// <para>NOTE: Table.DefaultView.Sort sould be set already to a field name. 
        /// Key is a value in that field.</para>
        /// <para>If Key not found or is null then Default is returned</para>
        /// </summary>
        static public T FindViewValue<T>(DataTable Table, object[] Key, string ValueField, T Default)
        {
            return FindViewValue(Table.DefaultView, Key, ValueField, Default);
        }
        /// <summary>
        /// Finds and returns a value in Table.DefaultView by Key.
        /// <para>NOTE: Table.DefaultView.Sort sould be set already to a field name. 
        /// Key is a value in that field.</para>
        /// <para>If Key not found or is null then Default is returned</para>
        /// </summary>
        static public T FindViewValue<T>(DataTable Table, object Key, string ValueField, T Default)
        {
            return FindViewValue(Table.DefaultView, Key, ValueField, Default);
        }
        /// <summary>
        /// Finds and returns a value in Table.DefaultView by Key.
        /// <para>NOTE: Table.DefaultView.Sort sould be set already to a field name. 
        /// Key is a value in that field.</para>
        /// <para>If Key not found or is null then DBNull.Value is returned</para>
        /// </summary>
        static public object FindViewValue(DataTable Table, object[] Key, string ValueField)
        {
            return FindViewValue(Table.DefaultView, Key, ValueField);
        }
        /// <summary>
        /// Finds and returns a value in Table.DefaultView by Key.
        /// <para>NOTE: Table.DefaultView.Sort sould be set already to a field name. 
        /// Key is a value in that field.</para>
        /// <para>If Key not found or is null then DBNull.Value is returned</para>
        /// </summary>
        static public object FindViewValue(DataTable Table, object Key, string ValueField)
        {
            return FindViewValue(Table.DefaultView, Key, ValueField);
        }

        /// <summary>
        /// Finds and returns a DataRow in the Table.
        /// <para>Table.DefaultView MUST be Sort-ed by a certain column.
        /// Key is a value from that column</para>
        /// </summary>
        static public DataRow FindDataRow(DataTable Table, object[] Key)
        {
            return FindDataRow(Table.DefaultView, Key);
        }
        /// <summary>
        /// Finds and returns a DataRow in the Table.
        /// <para>Table.DefaultView MUST be Sort-ed by a certain column.
        /// Key is a value from that column</para>
        /// </summary>
        static public DataRow FindDataRow(DataTable Table, object Key)
        {
            return FindDataRow(Table.DefaultView, Key);
        }

        /* field names */
        /// <summary>
        /// If FieldName contains bad characters such as spaces etc, 
        /// it returns FieldName surrounded by double quotes
        /// </summary>
        static public string NormalizeFieldName(string FieldName, SqlConnectionInfo ConnectionInfo)
        {
            if (string.IsNullOrWhiteSpace(FieldName))
                Sys.Throw("Can not normalize a field name: Field name is null or empty string");

            if (ConnectionInfo == null)
                Sys.Throw("Can not normalize a field name: ConnectionInfo is null");

            SqlProvider Provider = ConnectionInfo.GetSqlProvider(); 

            return Provider.NormalizeFieldName(FieldName);
        }
        /// <summary>
        /// If FieldName is surrounded by double quotes, removes those quotes
        /// </summary>
        static public string DenormalizeFieldName(string FieldName, SqlConnectionInfo ConnectionInfo)
        {
            return ConnectionInfo.GetSqlProvider().DenormalizeFieldName(FieldName);
        }

        /* dataset */
        /// <summary>
        /// Creates and returns a DataSet. Name can be null or empty
        /// </summary>
        static public DataSet CreateDataset(string Name)
        {
            if (string.IsNullOrEmpty(Name))
            {
                datasetCount++;
                Name = "DataSet_" + datasetCount.ToString();
            }

            return new DataSet(Name);
        }

        /* miscs */
        /// <summary>
        /// Returns true if the ColumnName is Id or ends with Id
        /// </summary>
        static public bool IsIdColumn(string ColumnName)
        {
            return !string.IsNullOrEmpty(ColumnName) && ColumnName.EndsWith("Id", StringComparison.InvariantCultureIgnoreCase);
        }
        /// <summary>
        /// Returns true if ColumnName is not Id or ends with Id
        /// </summary>
        static public bool IsVisibleColumn(string ColumnName)
        {
            return !IsIdColumn(ColumnName);
        }
        /// <summary>
        /// Sets the FieldName column of the specified Table to the passed Value, in all rows.
        /// </summary>
        static public void SetTableRowsTo(DataTable Table, string FieldName, object Value)
        {
            DataColumn Column = Table.FindColumn(FieldName);
            if (Column == null)
                return;

            foreach (DataRow Row in Table.Rows)
                Row[Column] = Value;
        }
        /// <summary>
        /// Returns true if Column is null or empty in the Row.
        /// </summary>
        static public bool IsNullOrEmpty(DataRow Row, DataColumn Column)
        {
            return (Row.IsNull(Column)) || ((Simple.IsString(Column.DataType)) && (string.IsNullOrEmpty(Row[Column].ToString())));
        }

        /* properties */
        /// <summary>
        /// A list of connection information coming from the application config file.
        /// <para>WARNING: This property can be assigned just once. 
        /// The provided value must be not null and contain at least one connection.
        /// The first connection found in the provided list is considered the MAIN connection and the SysConfig.MainConnection is assigned accordingly.
        /// </para>
        /// </summary>
        static public List<SqlConnectionInfo> Connections
        {
            get
            {
                if (fConnections == null || fConnections.Count == 0)
                    Sys.Throw("Can not get Connections. No database connections provided");

                return fConnections;
            }
            set
            {
                if (fConnections != null)
                    Sys.Throw("Connections strings are already set.");

                if (value == null || value.Count == 0)
                    Sys.Throw("Can not set Connections to a null or empty list. No database connections provided");

                fConnections = value;

                SqlConnectionInfo CS = fConnections.FirstOrDefault(item => item.Name.IsSameText(SysConfig.DefaultConnection));
                if (CS == null)
                {
                    CS = fConnections.FirstOrDefault(item => item.Name.IsSameText(Sys.DEFAULT));
                }
 
                string DefaultConnectionName = CS != null ? CS.Name : fConnections[0].Name;
                if (SysConfig.DefaultConnection.IsSameText(DefaultConnectionName))
                    SysConfig.DefaultConnection = DefaultConnectionName;


                foreach (SqlConnectionInfo Item in fConnections)
                {
                    Item.ConnectionString = ConnectionStringBuilder.NormalizeConnectionString(Item.ConnectionString);
                }

            }
        }
        /// <summary>
        /// Returns the default <see cref="SqlConnectionInfo"/>
        /// </summary>
        static public SqlConnectionInfo DefaultConnectionInfo { get { return GetDefaultConnectionInfo(); } }
        /// <summary>
        /// Metastores
        /// </summary>
        static public Metastores Metastores { get; } = new Metastores();
        /// <summary>
        /// The default DbIni
        /// </summary>
        static public DbIni MainIni { get { return fMainIni ?? (fMainIni = new DbIni(DefaultConnectionInfo)); } }
 
    }


}
