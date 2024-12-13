namespace Tripous.Data
{
    /// <summary>
    /// Sql store. Used in executing SELECT, INSERT, UPDATE, DELETE, etc commands, using a DbConnection.
    /// </summary>
    public class SqlStore
    {
        /* protected */
        /// <summary>
        /// Field
        /// </summary>
        protected Metastore fMetastore;
        /// <summary>
        /// Field
        /// </summary>
        protected bool ConnectionChecked;

        /// <summary>
        /// Prepares the store
        /// </summary>
        protected virtual void Setup(SqlConnectionInfo ConnectionInfo)
        {
            this.Provider = ConnectionInfo.GetSqlProvider();
            this.Factory = Provider.Factory;
            this.ConnectionInfo = ConnectionInfo;
        }

 
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public SqlStore(SqlConnectionInfo ConnectionInfo)
        {
            Setup(ConnectionInfo);
        }

        /* public */
        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return $"{Provider.Name}:{ConnectionInfo.Name}";
        }

        /* connection */
        /// <summary>
        /// Returns true if this connection info is valid and can connect to a database.
        /// </summary>
        public virtual bool CanConnect(bool ThrowIfNot = false)
        {
            return Provider.CanConnect(ConnectionInfo.ConnectionString, ThrowIfNot);
        }
        /// <summary>
        /// Ensures that a connection can be done by opening and closing the connection.
        /// </summary>
        public virtual void EnsureConnection()
        {
            if (!ConnectionChecked)
            {                
                Provider.EnsureConnection(ConnectionInfo.ConnectionString);
                ConnectionChecked = true;
            }
        }
        /// <summary>
        /// Creates and opens a DbConnection
        /// </summary>
        internal virtual DbConnection OpenConnection()
        {
            DbConnection Result = Provider.OpenConnection(ConnectionInfo.ConnectionString);
            ConnectionChecked = true;
            return Result;
        }
        /// <summary>
        /// Creates a DbConnection, opens the connection and begins a transaction.
        /// Returns the transaction.
        /// </summary>
        public virtual DbTransaction BeginTransaction()
        {
            DbConnection Con = OpenConnection();
            return Con.BeginTransaction();
        }

        /* select methods - Non Transactioned */
        /// <summary>
        /// Non transactioned.
        /// <para>An already created transaction is required in order to call this method.</para>
        /// Executes SqlText and returns a <see cref="DataTable"/>. 
        /// <para></para>
        /// <para>
        /// The passed Params can be: <br />
        /// a. either a comma separated list of parameters or <br />
        /// b. the Params[0] element, that is the first element in Params, may be 
        /// <list type="number">
        /// <item>a <see cref="DataRow"/></item>
        /// <item>a generic <see cref="IDictionary" />&lt;string, object&gt;</item>
        /// <item>or an <see cref="IList"/> or <see cref="Array"/></item>
        /// </list>
        /// and in this second case no other Params elements are used
        /// </para>
        /// </summary>
        public DataTable Select(DbTransaction Transaction, string SqlText, params object[] Params)
        {
            ResolveSql(ref SqlText);
            DateTime StartTimeUtc = DateTime.UtcNow;
            string Source = $"SqlStore.{ConnectionName}";
            string Scope = "Select";
  
            string EventId = string.Empty;
            string ParamsText = string.Empty;
            string CommandText = SqlText;

            using (DbCommand Cmd = Transaction.Connection.CreateCommand())
            {
                try
                {
                    if (ConnectionInfo.CommandTimeoutSeconds > 0)
                        Cmd.CommandTimeout = ConnectionInfo.CommandTimeoutSeconds;

                    Cmd.Transaction = Transaction;
                    using (DbDataAdapter Adapter = Provider.CreateAdapter())
                    {
                        Provider.SetupCommand(Cmd, SqlText, Params);

                        MemTable Table = new MemTable();
                        Table.Locale = CultureInfo.InvariantCulture;
                        string TableName = Table.TableName;
                        Table.TableName = "";

                        Adapter.SelectCommand = Cmd;

                        if (SqlMonitor.Active)
                        {
                            EventId = SqlMonitor.GetEventId(SqlText);
                            ParamsText = SqlMonitor.GetParamsText(Cmd);
                            CommandText = SqlMonitor.CommandToText(StartTimeUtc, Cmd, Source, Scope);
                        }
 
                            

                        bool Flag = Table.AutoGenerateGuidKeys;
                        Table.AutoGenerateGuidKeys = false;
                        try
                        {
                            Adapter.Fill(Table);
                        }
                        finally
                        {
                            Table.AutoGenerateGuidKeys = Flag;
                        }

                        Table.TableName = TableName;
                        Table.AcceptChanges();

                        if (SqlMonitor.Active)
                            SqlMonitor.LogSql(StartTimeUtc, SqlText, Source, Scope, EventId, ParamsText);

                        return Table;
                    }
                }
                catch (Exception ex)
                {
                    if (SqlMonitor.Active)
                        SqlMonitor.LogSql(StartTimeUtc, SqlText, Source, Scope, EventId, ParamsText);

                    ex.Data["SqlText"] = CommandText;
                    throw new SqlExceptionEx(ex.Message, ex, CommandText);
                }
            }
        } 
        /// <summary>
        /// Non transactioned.
        /// <para>An already created transaction is required in order to call this method.</para>
        /// Executes SqlText and returns a DataTable, actually a <see cref="DataTable"/>.
        /// </summary>
        public DataTable Select(DbTransaction Transaction, string SqlText)
        {
            return Select(Transaction, SqlText, null);
        }

        /// <summary>
        /// Non transactioned.
        /// <para>An already created transaction is required in order to call this method.</para>
        /// Executes SqlText and copies the returned DataRows to the Table.
        /// <para>If Table.Columns.Count > 0 then Table schema is preserved, otherwise
        /// Table gets the schema of the result list.</para>
        /// <para></para>
        /// <para>
        /// The passed Params can be: <br />
        /// a. either a comma separated list of parameters or <br />
        /// b. the Params[0] element, that is the first element in Params, may be 
        /// <list type="number">
        /// <item>a <see cref="DataRow"/></item>
        /// <item>a generic <see cref="IDictionary" />&lt;string, object&gt;</item>
        /// <item>or an <see cref="IList"/> or <see cref="Array"/></item>
        /// </list>
        /// and in this second case no other Params elements are used
        /// </para>
        /// </summary>              
        public int SelectTo(DbTransaction Transaction, DataTable Table, string SqlText, params object[] Params)
        {
            int Result = 0;

            DataTable Source = Select(Transaction, SqlText, Params);

            if (Table.Columns.Count > 0)
                Table.Rows.Clear();
            else
                Source.CopyStructureTo(Table);

            Source.SafeCopyTo(Table);

            Table.AcceptChanges();

            Result = Table.Rows.Count;

            return Result;

        }
        /// <summary>
        /// Non transactioned.
        /// <para>An already created transaction is required in order to call this method.</para>
        /// Executes SqlText and copies the returned DataRows to the Table.
        /// <para>If Table.Columns.Count > 0 then Table schema is preserved, otherwise
        /// Table gets the schema of the result list.</para> 
        /// </summary>
        public int SelectTo(DbTransaction Transaction, DataTable Table, string SqlText)
        {
            return SelectTo(Transaction, Table, SqlText, null);
        }

        /// <summary>
        /// Non transactioned.
        /// <para>An already created transaction is required in order to call this method.</para>
        /// Executes SqlText and returns the first DataRow of the result set.
        /// <para>WARNING: If SqlText returns no rows at all then this method returns null.</para>
        /// <para></para>
        /// <para>
        /// The passed Params can be: <br />
        /// a. either a comma separated list of parameters or <br />
        /// b. the Params[0] element, that is the first element in Params, may be 
        /// <list type="number">
        /// <item>a <see cref="DataRow"/></item>
        /// <item>a generic <see cref="IDictionary" />&lt;string, object&gt;</item>
        /// <item>or an <see cref="IList"/> or <see cref="Array"/></item>
        /// </list>
        /// and in this second case no other Params elements are used
        /// </para>
        /// </summary>
        public DataRow SelectResults(DbTransaction Transaction, string SqlText, params object[] Params)
        {
            DataTable Table = Select(Transaction, SqlText, Params);
            if (Table.Rows.Count > 0)
                return Table.Rows[0];
            else
                return null;
        }
        /// <summary>
        /// Non transactioned.
        /// <para>An already created transaction is required in order to call this method.</para>
        /// Executes SqlText and returns the first DataRow of the result set.
        /// <para>WARNING: If SqlText returns no rows at all then this method returns null.</para>
        /// </summary>
        public DataRow SelectResults(DbTransaction Transaction, string SqlText)
        {
            return SelectResults(Transaction, SqlText, null);
        }

        /// <summary>
        /// Non transactioned.
        /// <para>An already created transaction is required in order to call this method.</para>
        /// Ideal for executing SELECT statements of the type "select FIELD_NAME from TABLE_NAME"
        /// </summary>
        public object SelectResult(DbTransaction Transaction, string SqlText, object Default)
        {
            object Result = Default;

            DataRow Row = SelectResults(Transaction, SqlText);
            if ((Row != null) && !Row.IsNull(0))
            {
                Result = Row[0];
            }

            return Result;
        }
        /// <summary>
        /// Non transactioned.
        /// <para>An already created transaction is required in order to call this method.</para>
        /// Ideal for executing SELECT statements of the type "select count(ID) as COUNT_ID from TABLE_NAME where ID = 1234"
        /// </summary>
        public int IntegerResult(DbTransaction Transaction, string SqlText, int Default)
        {
            string S = SelectResult(Transaction, SqlText, Default).ToString();
            int Result;
            return int.TryParse(S, out Result) ? Result : Default;
        }

        /* select methods - Transactioned */
        /// <summary>
        /// Executes SqlText and returns a <see cref="DataTable"/>.
        /// <para></para>
        /// <para>
        /// The passed Params can be: <br />
        /// a. either a comma separated list of parameters or <br />
        /// b. the Params[0] element, that is the first element in Params, may be 
        /// <list type="number">
        /// <item>a <see cref="DataRow"/></item>
        /// <item>a generic <see cref="IDictionary" />&lt;string, object&gt;</item>
        /// <item>or an <see cref="IList"/> or <see cref="Array"/></item>
        /// </list>
        /// and in this second case no other Params elements are used
        /// </para>
        /// </summary>
        public virtual DataTable Select(string SqlText, params object[] Params)
        {
            DataTable Result = null;

            using (DbConnection Con = Provider.OpenConnection(ConnectionInfo.ConnectionString))
            {
                using (DbTransaction Transaction = Con.BeginTransaction())
                {
                    try
                    {
                        Result = Select(Transaction, SqlText, Params);
                        Transaction.Commit();
                    }
                    catch 
                    {
                        Transaction.Rollback();
                        throw;
                    }
                }
            }

            return Result;
        }
        /// <summary>
        /// Executes SqlText and returns a <see cref="DataTable"/>.
        /// </summary>
        public DataTable Select(string SqlText)
        {
            return Select(SqlText, null);
        }

        /// <summary>
        /// Executes SqlText and copies the returned DataRows to the Table.
        /// <para>If Table.Columns.Count > 0 then Table schema is preserved, otherwise
        /// Table gets the schema of the result list.</para>
        /// <para></para>
        /// <para>
        /// The passed Params can be: <br />
        /// a. either a comma separated list of parameters or <br />
        /// b. the Params[0] element, that is the first element in Params, may be 
        /// <list type="number">
        /// <item>a <see cref="DataRow"/></item>
        /// <item>a generic <see cref="IDictionary" />&lt;string, object&gt;</item>
        /// <item>or an <see cref="IList"/> or <see cref="Array"/></item>
        /// </list>
        /// and in this second case no other Params elements are used
        /// </para>
        /// </summary>            
        public int SelectTo(DataTable Table, string SqlText, params object[] Params)
        {
            int Result = 0;

            DataTable Source = Select(SqlText, Params);

            if (Table.Columns.Count > 0)
                Table.Rows.Clear();
            else
                Source.CopyStructureTo(Table);

            Source.SafeCopyTo(Table);

            Table.AcceptChanges();

            Result = Table.Rows.Count;

            return Result;

        }
        /// <summary>
        /// Executes SqlText and copies the returned DataRows to the Table.
        /// <para>If Table.Columns.Count > 0 then Table schema is preserved, otherwise
        /// Table gets the schema of the result list.</para> 
        /// </summary>
        public int SelectTo(DataTable Table, string SqlText)
        {
            return SelectTo(Table, SqlText, null);
        }


        /// <summary>
        /// Executes SqlText and returns the first DataRow of the result set.
        /// <para>WARNING: If SqlText returns no rows at all then this method returns null.</para>
        /// <para></para>
        /// <para>
        /// The passed Params can be: <br />
        /// a. either a comma separated list of parameters or <br />
        /// b. the Params[0] element, that is the first element in Params, may be 
        /// <list type="number">
        /// <item>a <see cref="DataRow"/></item>
        /// <item>a generic <see cref="IDictionary" />&lt;string, object&gt;</item>
        /// <item>or an <see cref="IList"/> or <see cref="Array"/></item>
        /// </list>
        /// and in this second case no other Params elements are used
        /// </para>
        /// </summary>
        public DataRow SelectResults(string SqlText, params object[] Params)
        {
            DataTable Table = Select(SqlText, Params);
            if (Table.Rows.Count > 0)
                return Table.Rows[0];
            else
                return null;
        }
        /// <summary>
        /// Executes SqlText and returns the first DataRow of the result set.
        /// <para>WARNING: If SqlText returns no rows at all then this method returns null.</para>
        /// </summary>
        public DataRow SelectResults(string SqlText)
        {
            return SelectResults(SqlText, null);
        }

        /// <summary>
        /// Ideal for executing SELECT statements of the type "select FIELD_NAME from TABLE_NAME".
        /// <para>Returns a specified default value if the SELECT returns no values.</para>
        /// <para>
        /// The passed Params can be: <br />
        /// a. either a comma separated list of parameters or <br />
        /// b. the Params[0] element, that is the first element in Params, may be 
        /// <list type="number">
        /// <item>a <see cref="DataRow"/></item>
        /// <item>a generic <see cref="IDictionary" />&lt;string, object&gt;</item>
        /// <item>or an <see cref="IList"/> or <see cref="Array"/></item>
        /// </list>
        /// and in this second case no other Params elements are used
        /// </para>
        /// </summary>
        public object SelectResult(string SqlText, object Default, params object[] Params)
        {
            object Result = Default;

            DataRow Row = SelectResults(SqlText, Params);
            if ((Row != null) && !Row.IsNull(0))
            {
                Result = Row[0];
            }

            return Result;
        }
        /// <summary>
        /// Ideal for executing SELECT statements of the type "select FIELD_NAME from TABLE_NAME"
        /// <para>Returns a specified default value if the SELECT returns no values.</para>
        /// </summary>
        public object SelectResult(string SqlText, object Default)
        {
            return SelectResult(SqlText, Default, null);
        }
        /// <summary>
        /// Ideal for executing SELECT statements of the type "select FIELD_NAME from TABLE_NAME"
        /// <para>Returns <see cref="DBNull"/> if the SELECT returns no values.</para>
        /// </summary>
        public object SelectResult(string SqlText)
        {
            return SelectResult(SqlText, DBNull.Value);
        }

        /// <summary>
        /// Ideal for executing SELECT statements of the type "select count(ID) as COUNT_ID from TABLE_NAME where ID = 1234"
        /// <para>
        /// The passed Params can be: <br />
        /// a. either a comma separated list of parameters or <br />
        /// b. the Params[0] element, that is the first element in Params, may be 
        /// <list type="number">
        /// <item>a <see cref="DataRow"/></item>
        /// <item>a generic <see cref="IDictionary" />&lt;string, object&gt;</item>
        /// <item>or an <see cref="IList"/> or <see cref="Array"/></item>
        /// </list>
        /// and in this second case no other Params elements are used
        /// </para>
        /// </summary>
        public int IntegerResult(string SqlText, int Default, params object[] Params)
        {
            string S = SelectResult(SqlText, Default, Params).ToString();
            int Result;
            return int.TryParse(S, out Result) ? Result : Default;
        }
        /// <summary>
        /// Ideal for executing SELECT statements of the type "select count(ID) as COUNT_ID from TABLE_NAME where ID = 1234"
        /// </summary>
        public int IntegerResult(string SqlText, int Default)
        {
            return IntegerResult(SqlText, Default, null);
        }

        /* execution methods */
        /// <summary>
        /// Non transactioned.
        /// <para>A call to StartTransaction is required prior to this call.</para>
        /// <para></para>
        /// <para>
        /// The passed Params can be: <br />
        /// a. either a comma separated list of parameters or <br />
        /// b. the Params[0] element, that is the first element in Params, may be 
        /// <list type="number">
        /// <item>a <see cref="DataRow"/></item>
        /// <item>a generic <see cref="IDictionary" />&lt;string, object&gt;</item>
        /// <item>or an <see cref="IList"/> or <see cref="Array"/></item>
        /// </list>
        /// and in this second case no other Params elements are used
        /// </para>
        /// </summary>
        public void ExecSql(DbTransaction Transaction, string SqlText, params object[] Params)
        {
            ResolveSql(ref SqlText);
            DateTime StartTimeUtc = DateTime.UtcNow;
            string Source = $"SqlStore.{ConnectionName}";
            string Scope = "ExecSql";
           
            string EventId = string.Empty;
            string ParamsText = string.Empty;
            string CommandText = SqlText;


            using (DbCommand Cmd = Transaction.Connection.CreateCommand())
            {
                try
                {
                    if (ConnectionInfo.CommandTimeoutSeconds > 0)
                        Cmd.CommandTimeout = ConnectionInfo.CommandTimeoutSeconds;

                    Provider.SetupCommand(Cmd, SqlText, Params);
                    Cmd.Transaction = Transaction;

                    if (SqlMonitor.Active)
                    {
                        EventId = SqlMonitor.GetEventId(SqlText);
                        ParamsText = SqlMonitor.GetParamsText(Cmd);
                        CommandText = SqlMonitor.CommandToText(StartTimeUtc, Cmd, Source, Scope);
                    }
 

                    Cmd.ExecuteNonQuery();

                    if (SqlMonitor.Active)
                        SqlMonitor.LogSql(StartTimeUtc, SqlText, Source, Scope, EventId, ParamsText);
                }
                catch (Exception ex)
                {
                    if (SqlMonitor.Active)
                        SqlMonitor.LogSql(StartTimeUtc, SqlText, Source, Scope, EventId, ParamsText);

                    ex.Data["SqlText"] = CommandText;
                    throw new SqlExceptionEx(ex.Message, ex, CommandText);
                }
            }
        }
        /// <summary>
        /// Non transactioned.
        /// <para>A call to StartTransaction is required prior to this call.</para>
        /// </summary>
        public void ExecSql(DbTransaction Transaction, string SqlText)
        {
            ExecSql(Transaction, SqlText, null);
        }
 
        /// <summary>
        /// Transactioned. Creates a DbTransaction, executes SqlText inside that transaction and commits.
        /// <para></para>
        /// <para>
        /// The passed Params can be: <br />
        /// a. either a comma separated list of parameters or <br />
        /// b. the Params[0] element, that is the first element in Params, may be 
        /// <list type="number">
        /// <item>a <see cref="DataRow"/></item>
        /// <item>a generic <see cref="IDictionary" />&lt;string, object&gt;</item>
        /// <item>or an <see cref="IList"/> or <see cref="Array"/></item>
        /// </list>
        /// and in this second case no other Params elements are used
        /// </para>
        /// </summary>
        public virtual void ExecSql(string SqlText, params object[] Params)
        {
            using (DbConnection Con = Provider.OpenConnection(ConnectionInfo.ConnectionString))
            {
                using (DbTransaction Transaction = Con.BeginTransaction())
                {
                    try
                    {
                        ExecSql(Transaction, SqlText, Params);
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
        /// <summary>
        /// Transactioned. Creates a DbTransaction, executes SqlText inside that transaction and commits.
        /// </summary>
        public virtual void ExecSql(string SqlText)
        {
            ExecSql(SqlText, null);
        }

        /// <summary>
        /// Executes a list of executable statements inside a single transaction
        /// </summary>
        public virtual void ExecSql(IEnumerable<string> SqlTextList)
        {
            using (DbTransaction transaction = this.BeginTransaction())
            {
                try
                {
                    foreach (string SqlText in SqlTextList)
                        ExecSql(transaction, SqlText);

                    /* commit the transaction */
                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        /* id generation - Non Transactioned */
        /// <summary>
        /// Returns the next id value of a generator named after the TableName table.
        /// <para>It should be used only with databases that support generators or when a CustomOid object is used.</para>
        /// </summary>
        public virtual int NextId(DbTransaction Transaction, string TableName)
        {
           return NextIdByGenerator(Transaction, "G_" + TableName);
        }
        /// <summary>
        /// Returns the last id produced by an INSERT Sqlt statement.
        /// <para>It should be used only with databases that support identity (auto-increment) columns</para>
        /// </summary>
        public virtual int LastId(DbTransaction Transaction, string TableName)
        {
            return Provider.LastId(this, Transaction, TableName);
        }
        /// <summary>
        /// Returns the next value of the GeneratorName generator.
        /// </summary>
        public virtual int NextIdByGenerator(DbTransaction Transaction, string GeneratorName)
        {
            return Provider.NextIdByGenerator(this, Transaction, GeneratorName);
        }

        /* id generation - Transactioned */
        /// <summary>
        /// Returns the next id value of a generator named after the TableName table.
        /// <para>It should be used only with databases that support generators or when a CustomOid object is used.</para>
        /// </summary>
        public virtual  int NextId(string TableName)
        {

            int Result;

            using (DbTransaction Transaction = BeginTransaction())
            {
                try
                {
                    Result = NextId(Transaction, TableName);
                    Transaction.Commit();
                }
                catch
                {
                    Transaction.Rollback();
                    throw;
                }
            }

            return Result;

        }
        /// <summary>
        /// Returns the last id produced by an INSERT Sqlt statement.
        /// <para>It should be used only with databases that support identity (auto-increment) columns</para>
        /// </summary>
        public virtual int LastId(string TableName)
        {

            int Result;

            using (DbTransaction Transaction = BeginTransaction())
            {
                try
                {
                    Result = LastId(Transaction, TableName);
                    Transaction.Commit();
                }
                catch 
                {
                    Transaction.Rollback();
                    throw;
                }
            }

            return Result;

        }
        /// <summary>
        /// Returns the next value of the GeneratorName generator.
        /// </summary>
        public virtual int NextIdByGenerator(string GeneratorName)
        {

            int Result = -1;

            using (DbTransaction Transaction = BeginTransaction())
            {
                try
                {
                    Result = NextIdByGenerator(Transaction, GeneratorName);
                    Transaction.Commit();
                }
                catch 
                {
                    Transaction.Rollback();
                    throw;
                }
            }

            return Result;
        }

        /* schema related */ 
        /// <summary>
        /// Returns schema information for the data source of this System.Data.Common.DbConnection.
        /// </summary>
        public virtual DataTable GetSchema()
        {
            using (DbConnection Con = OpenConnection())
                return Con.GetSchema();
        }
        /// <summary>
        /// Returns schema information for the data source of this System.Data.Common.DbConnection
        /// using the specified string for the schema name.
        /// </summary>
        public virtual DataTable GetSchema(string collectionName)
        {
            using (DbConnection Con = OpenConnection())
                return Con.GetSchema(collectionName);
        }
        /// <summary>
        /// Returns schema information for the data source of this System.Data.Common.DbConnection
        /// using the specified string for the schema name and the specified string array
        /// for the restriction values.
        /// </summary>
        public virtual DataTable GetSchema(string collectionName, string[] restrictionValues)
        {
            using (DbConnection Con = OpenConnection())
                return Con.GetSchema(collectionName, restrictionValues);
        }

        /// <summary>
        /// Executes either SqlText or a "select * from TableName", and stores the result schema
        /// to the <see cref="SqlCache"/> list, under the SchemaName, if given, else under the TableName.
        /// <para></para>
        /// <para>Either SqlText or TableName MUST be NOT null or empty.</para>
        /// <para>If SqlText is null or empty then SqlText becomes SELECT * FROM TableName.</para>
        /// <para>If TableName is null or empty then this function tries to extract it from SqlText.</para>
        /// <para>If SchemaName is null or empty then SchemaName = TableName. </para>
        /// <para>Table can be null. If it is not null then the result schema is merged to that Table schema.</para>
        /// </summary>
        public void GetNativeSchema(string SqlText, string TableName, string SchemaName, DataTable Table)
        {

            if (string.IsNullOrWhiteSpace(SqlText) && string.IsNullOrWhiteSpace(TableName))
                Sys.Throw("GetNativeSchema(): SqlText and TableName are both null or empty!");

            if (string.IsNullOrWhiteSpace(SqlText) && !string.IsNullOrWhiteSpace(TableName))
                SqlText = string.Format("select * from {0}", TableName);

            SelectSql SS = new SelectSql(SqlText);

            if (string.IsNullOrWhiteSpace(TableName))
            {
                TableName = SS.GetMainTableName();
                if (string.IsNullOrWhiteSpace(TableName))
                    Sys.Throw("GetNativeSchema(): Can NOT extract main table name from SqlText!");
            }

            if (string.IsNullOrWhiteSpace(SchemaName))
                SchemaName = TableName;

            DataTable SchemaTable = null;
            StringBuilder SB = new StringBuilder();


            if (!string.IsNullOrWhiteSpace(SchemaName) && SqlCache.Contains(this.ConnectionName, SchemaName))
            {
                SchemaTable = SqlCache.Find(this.ConnectionName, SchemaName);  
            }
            else
            {

                string[] Wheres = {
                            "{0}.ID = '-100000000'",
                            "{0}.CODE = '00000000'",
                            "{0}.NAME = '00000000'",
                            "1 > 2"
                          };

                for (int i = 0; i < Wheres.Length; i++)
                    Wheres[i] = string.Format(Wheres[i], TableName);


                string sSqlText = string.Empty;
                for (int i = 0; i < Wheres.Length; i++)
                {
                    try
                    {
                        SS.WhereUser = Wheres[i];
                        sSqlText = SS.Text;

                        SchemaTable = this.Select(sSqlText);
                        UpdateSchemaTable(SchemaTable, sSqlText);
                        SqlCache.Add(this.ConnectionName, SchemaName, SchemaTable);
                        break;
                    }
                    catch (Exception ex)
                    {
                        SB.AppendLine();
                        SB.AppendLine(string.Format("[{0}]", i));
                        SB.AppendLine(Sys.ExceptionText(ex));
                        SB.AppendLine();
                        SB.AppendLine(sSqlText);
                        SB.AppendLine("==========================================");
                    }
                }

            }


            if (SchemaTable == null)
            {
                string Message = "GetNativeSchema(): Can not get schema for: ";
                Message = !string.IsNullOrWhiteSpace(SqlText) ? Message + Environment.NewLine + SqlText : Message + TableName;

                if (SB.Length > 0)
                    Message += Environment.NewLine + SB.ToString();

                Sys.Throw(Message);
            }



            if (Table != null)
                SchemaTable.MergeStructure(Table);


        }
        /// <summary>
        /// Updates the SchemaTable with schema retrieved by using a DbDataReader.
        /// </summary>
        protected virtual void UpdateSchemaTable(DataTable SchemaTable, string SqlText)
        {

            try
            {
                using (DbConnection Con = this.OpenConnection())
                {
                    using (DbCommand Cmd = Con.CreateCommand())
                    {
                        Cmd.CommandText = SqlText;

                        using (DbDataReader Reader = Cmd.ExecuteReader())
                        {
                            DataTable Table = Reader.GetSchemaTable();
                            //DataColumn Field;
                            string ColumnName;
                            int ColumnSize;

                            foreach (DataRow ColumnRow in Table.Rows)
                            {
                                try
                                {
                                    ColumnName = ColumnRow[SchemaTableColumn.ColumnName].ToString();

                                    if (SchemaTable.Columns.Contains(ColumnName) && Simple.SimpleTypeOf(SchemaTable.Columns[ColumnName].DataType).IsString())
                                    {
                                        ColumnSize = (int)ColumnRow[SchemaTableColumn.ColumnSize];
                                        if (ColumnSize > 0)
                                            SchemaTable.Columns[ColumnName].MaxLength = ColumnSize;
                                    }
                                }
                                catch
                                {
                                }
                            }
                        }
                    }
                }
            }
            catch
            {
            }
        }


        /* schema related */
        /// <summary>
        /// Returns a string list with the table names in the database.
        /// </summary>
        public List<string> GetTableNames()
        {
            this.Metastore.Load();
            this.Metastore.Tables.Load();
            return this.Metastore.GetTableNames();
        }
        /// <summary>
        /// Returns a string list with the field names of the TableName
        /// </summary>
        public List<string> GetFieldNames(string TableName)
        {
            this.Metastore.Load();
            this.Metastore.Tables.Load();
            return this.Metastore.GetFieldNames(TableName);
        }
        /// <summary>
        /// Returns a string list with the index names in the database
        /// </summary>
        public List<string> GetIndexNames()
        {
            this.Metastore.Load();
            this.Metastore.Tables.Load();
            return this.Metastore.GetIndexNames();
        }

        /// <summary>
        /// Returns true if a table with TableName exists in the database.
        /// </summary>
        public bool TableExists(string TableName)
        {
            IList<string> List = GetTableNames();
            return List.ContainsText(TableName);
        }
        /// <summary>
        /// Empties the TableName table in the database and initializes its generator/sequencer or identity column.
        /// <para>DANGEROUS</para>
        /// </summary>
        public void ResetTable(string TableName)
        {
            ExecSql("delete from " + TableName);
            Provider.SetTableGeneratorTo(this.ConnectionInfo.ConnectionString, TableName, 0);
        }
        /// <summary>
        /// Returns true if a table contains no rows in the database.
        /// </summary>
        public bool TableIsEmpty(string TableName)
        {
            string SqlText = string.Format("select count(*) as RESULT from {0}", TableName);

            return IntegerResult(SqlText, 0) <= 0;
        }
        /// <summary>
        /// Creates a new table in the database by executing CommandText. Returns true
        /// only if creates the table, false if the table already exists.
        /// <para>The method creates a table generator too, if the database supports generators/sequences.</para>
        /// <para>CommandText should be a CREATE TABLE statement and can contain datatype placeholders.
        /// See <see cref="SqlProvider.ReplaceDataTypePlaceholders"/> for details.</para>
        /// </summary>
        public virtual bool CreateTable(string SqlText)
        {
            string TableName = Sql.ExtractTableName(SqlText);

            if (!string.IsNullOrWhiteSpace(TableName) && !TableExists(TableName))
            {
                SqlText = Provider.ReplaceDataTypePlaceholders(SqlText);

                ExecSql(SqlText);

                if (Provider.SupportsGenerators && ConnectionInfo.AutoCreateGenerators)
                {
                    if (!Provider.GeneratorExists(this.ConnectionInfo.ConnectionString, "G_" + TableName))
                        Provider.CreateGenerator(this.ConnectionInfo.ConnectionString, "G_" + TableName);
                    else
                        Provider.SetGeneratorTo(this.ConnectionInfo.ConnectionString, "G_" + TableName, 0);
                }

                return true;
            }


            return false;

        }
        /// <summary>
        /// Returns true if the FieldName exists in TableName table.
        /// </summary>
        public bool FieldExists(string TableName, string FieldName)
        {
            IList<string> List = GetFieldNames(TableName);
            return List.ContainsText(FieldName);
        }
        /// <summary>
        /// Returns true if an index with IndexName exists in the database.
        /// </summary>
        public bool IndexExists(string IndexName)
        {
            IList<string> List = GetIndexNames();
            return List.ContainsText(IndexName);


        }
       
        /* miscs */
        /// <summary>
        /// Puts Variable values into the SqlText by replacing value placeholders.
        /// <para>The default prefix for a Variable inside CommandText text is :@</para>
        /// </summary>
        public virtual void ResolveSql(ref string SqlText)
        {
            SqlValueProviders.Process(ref SqlText, this);
        }
 
        /* properties */
        /// <summary>
        /// The connection name
        /// </summary>
        public virtual string ConnectionName { get { return ConnectionInfo.Name; } }
        /// <summary>
        /// Connection string settings
        /// </summary>
        public virtual SqlConnectionInfo ConnectionInfo { get; protected set; }
        /// <summary>
        /// The provider
        /// </summary>
        public virtual SqlProvider Provider { get; protected set; }
        /// <summary>
        /// The provider factory
        /// </summary>
        public virtual DbProviderFactory Factory { get; protected set; }
        /// <summary>
        /// Returns the <see cref="Metastore"/> associated with this store.
        /// </summary>
        public virtual Metastore Metastore 
        { 
            get 
            { 
                if (fMetastore == null)
                {
                    fMetastore = Db.Metastores.Find(ConnectionName);
                    if (fMetastore == null)
                    {
                        fMetastore = new Metastore(ConnectionInfo);
                        Db.Metastores.Add(fMetastore);
                        fMetastore.Load();
                    }
                }
                
                return fMetastore; 
            
            } 
        }

    }
}
