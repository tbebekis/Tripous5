/*--------------------------------------------------------------------------------------        
                           Copyright © 2019 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Globalization;
using System.Data;
using System.Data.Common;
using System.Reflection;

using Tripous.Logging;

namespace Tripous.Data
{
    /// <summary>
    /// Describes a Sql provider
    /// </summary>
    public abstract class SqlProvider
    {
        /* Aliases */
        /// <summary>
        /// Provider name constant
        /// </summary>
        public const string MsSql = "MsSql";
        /// <summary>
        /// Provider name constant
        /// </summary>
        public const string Oracle = "Oracle";
        /// <summary>
        /// Provider name constant
        /// </summary>
        public const string Firebird = "Firebird";
        /// <summary>
        /// Provider name constant
        /// </summary>
        public const string SQLite = "SQLite";
        /// <summary>
        /// Provider name constant
        /// </summary>
        public const string MySql = "MySql";
        /// <summary>
        /// Provider name constant
        /// </summary>
        public const string PostgreSQL = "PostgreSQL";

        /// <summary>
        /// Provider name constant
        /// </summary>
        public const string OleDb = "OleDb";
        /// <summary>
        /// Provider name constant
        /// </summary>
        public const string MsAccess = "MsAccess";
        /// <summary>
        /// Provider name constant
        /// </summary>
        public const string MsExcel = "MsExcel";
        /// <summary>
        /// Provider name constant
        /// </summary>
        public const string OleDbDbf = "OleDbDbf";

        /// <summary>
        /// Provider name constant
        /// </summary>
        public const string Odbc = "Odbc";

        /* column type constants */
        /// <summary>
        /// constant
        /// </summary>
        public const string CPRIMARY_KEY = "@PRIMARY_KEY";
        /// <summary>
        /// constant
        /// </summary>
        public const string CVARCHAR = "@VARCHAR";
        /// <summary>
        /// constant
        /// </summary>
        public const string CNVARCHAR = "@NVARCHAR";
        /// <summary>
        /// constant
        /// </summary>
        public const string CFLOAT = "@FLOAT";
        /// <summary>
        /// constant
        /// </summary>
        public const string CDECIMAL = "@DECIMAL";
        /// <summary>
        /// constant
        /// </summary>
        public const string CDATE = "@DATE";
        /// <summary>
        /// constant
        /// </summary>
        public const string CDATE_TIME = "@DATE_TIME";
        /// <summary>
        /// constant
        /// </summary>
        public const string CBOOL = "@BOOL";
        /// <summary>
        /// constant
        /// </summary>
        public const string CBLOB = "@BLOB";
        /// <summary>
        /// constant
        /// </summary>
        public const string CBLOB_TEXT = "@BLOB_TEXT";
        /// <summary>
        /// constant
        /// </summary>
        public const string CNBLOB_TEXT = "@NBLOB_TEXT";
        /// <summary>
        /// constant
        /// </summary>
        public const string CNOT_NULL = "@NOT_NULL";
        /// <summary>
        /// constant
        /// </summary>
        public const string CNULL = "@NULL";

        /// <summary>
        /// Field
        /// </summary>
        protected DbProviderFactory fFactory;

        /* protected */
        /// <summary>
        /// Field
        /// </summary>
        static protected char[] badFieldCharactes = new char[] { ' ', '(', ')', '%', '.', '!' };

        /// <summary>
        /// NOTE: .Net Standard (and .Net Core) does not provide a DbProviderFactories class, used in loading data provider factories.
        /// <para>This method provides a solution to the problem by loading an assembly and a factory type dynamically.</para>
        /// <para>This method returns a DbProviderFactory instance specified either by simply a type name, 
        /// or a namespace qualified type name or an AssemblyQualifiedName type name.</para>
        /// <para>Optionally, an Assembly may be specified in order to load and search for the type, using either the long form of its name or a file path. </para>
        /// <para>This method throws an error if nothing is found. </para>
        /// <para>Example: LoadDbProviderFactory("FirebirdSql.Data.FirebirdClient.FirebirdClientFactory", "FirebirdSql.Data.FirebirdClient.dll"); </para>
        /// </summary>
        protected virtual DbProviderFactory LoadDbProviderFactory(string DbProviderFactoryTypeName, string AssemblyNameOrPath)
        {
            DbProviderFactory Result = null;

            Type T = TypeFinder.GetTypeByName(DbProviderFactoryTypeName, AssemblyNameOrPath);
            if (T != null)
            {
                Result = Sys.GetStaticProperty(T, "Instance") as DbProviderFactory;

                if (Result == null)
                {
                    BindingFlags Flags = BindingFlags.Instance | BindingFlags.NonPublic;
                    ConstructorInfo Constructor = T.GetConstructor(Flags, null, new Type[] { }, null);
                    if (Constructor != null)
                        Result = Constructor.Invoke(new object[] { }) as DbProviderFactory;
                }
            }

            if (Result == null)
                throw new ApplicationException(string.Format("DbProviderFactory not found: {0}", DbProviderFactoryTypeName));

            return Result as DbProviderFactory;

        }
        /// <summary>
        /// Returns true if C is a name delimiter.
        /// </summary>
        protected bool IsNameDelimiter(char C)
        {
            return (C == ' ') || (C == ',') || (C == ';') || (C == ')') || (C == '\n') || (C == '\r');
        }
        /// <summary>
        /// Returns true if C is a literal delimiter.
        /// </summary>
        protected bool IsLiteral(char C)
        {
            return (C == '\'') || (C == '"') || (C == '`');
        }

        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Name">The name of the provider, e.g. MsSql, Oracle, SQLite, etc.</param>
        /// <param name="Factory">The provider factory</param>       
        public SqlProvider(string Name,  DbProviderFactory Factory = null)
        {
            this.Name = Name;
            this.Factory = Factory;
        }

        /// <summary>
        /// Returns a string representation of this instance
        /// </summary>
        public override string ToString()
        {
            return Name;
        }

        /* DbProviderFactory related */
 
        /// <summary>
        /// Creates and returns a DbConnection initialized with ConnectionString.
        /// <para>WARNING: It does NOT open the connection.</para>
        /// </summary>
        public virtual DbConnection CreateConnection(string ConnectionString)
        {
            DbConnection Result = Factory.CreateConnection();
            ConnectionString = ConnectionStringBuilder.ReplacePathPlaceholders(ConnectionString);
            Result.ConnectionString = ConnectionString;
            return Result;
        }
        /// <summary>
        /// Creates, opens and returns a DbConnection initialized with ConnectionString.
        /// <para>WARNING: It opens the connection too.</para>
        /// </summary>
        public virtual DbConnection OpenConnection(string ConnectionString)
        {
            DbConnection Result = CreateConnection(ConnectionString);
            Result.Open();
            return Result;
        }
        /// <summary>
        /// Creates and returns a DbDataAdapter.
        /// </summary>
        public virtual DbDataAdapter CreateAdapter()
        {
            return Factory.CreateDataAdapter();
        }
        /// <summary>
        /// Creates and returns a DbCommandBuilder
        /// </summary>
        public virtual DbCommandBuilder CreateCommandBuilder()
        {
            return Factory.CreateCommandBuilder();
        }
        /// <summary>
        /// Creates and returns a DbConnectionStringBuilder
        /// </summary>
        public virtual DbConnectionStringBuilder CreateConnectionStringBuilder()
        {
            return Factory.CreateConnectionStringBuilder();
        }
        /// <summary>
        /// Creates and returns a DbParameter
        /// </summary>
        public virtual DbParameter CreateParameter()
        {
            return Factory.CreateParameter();
        }


        /// <summary>
        /// Returns true if the database represented by the specified database exists, by checking the connection.
        /// </summary>
        public virtual bool DatabaseExists(string ConnectionString)
        {
            bool Result = false;
            try
            {
                using (DbConnection Con = OpenConnection(ConnectionString))
                {
                    Con.Close();
                }

                Result = true;
            }
            catch 
            {
            }

            return Result;
        }
        /// <summary>
        /// Creates a new database, if not exists. Returns true only if creates the database.
        /// </summary>
        public virtual bool CreateDatabase(string ConnectionString)
        {
            return false;
        }

        /// <summary>
        /// Returns true if this connection info is valid and can connect to a database.
        /// </summary>
        public virtual bool CanConnect(string ConnectionString, bool ThrowIfNot = false)
        {
            try
            {
                using (DbConnection Con = OpenConnection(ConnectionString))
                {
                    Con.Close();
                }

                return true;
            }
            catch
            {
                if (ThrowIfNot)
                    throw;
            }

            return false;
        }
        /// <summary>
        /// Ensures that a connection can be done by opening and closing the connection.
        /// </summary>
        public virtual void EnsureConnection(string ConnectionString)
        {
            using (DbConnection Con = OpenConnection(ConnectionString))
            {
                Con.Close();
            }
        }

        /// <summary>
        /// Returns schema information for the data source of this System.Data.Common.DbConnection.
        /// </summary>
        public virtual DataTable GetSchema(string ConnectionString)
        {
            using (DbConnection Con = OpenConnection(ConnectionString))
                return Con.GetSchema();
        }
        /// <summary>
        /// Returns schema information for the data source of this System.Data.Common.DbConnection
        /// using the specified string for the schema name.
        /// </summary>
        public virtual DataTable GetSchema(string ConnectionString, string collectionName)
        {
            using (DbConnection Con = OpenConnection(ConnectionString))
                return Con.GetSchema(collectionName);
        }
        /// <summary>
        /// Returns schema information for the data source of this System.Data.Common.DbConnection
        /// using the specified string for the schema name and the specified string array
        /// for the restriction values.
        /// </summary>
        public virtual DataTable GetSchema(string ConnectionString, string collectionName, string[] restrictionValues)
        {
            using (DbConnection Con = OpenConnection(ConnectionString))
                return Con.GetSchema(collectionName, restrictionValues);
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
        public DataTable Select(string ConnectionString, string SqlText, params object[] Params)
        {
            DataTable Result = new DataTable();

            ConnectionString = ConnectionStringBuilder.RemoveAliasEntry(ConnectionString);

            using (DbConnection Con = OpenConnection(ConnectionString))
            {
                using (DbCommand Cmd = Con.CreateCommand())
                {
                    SetupCommand(Cmd, SqlText, Params);

                    using (DbDataAdapter adapter = CreateAdapter())
                    {
                        adapter.SelectCommand = Cmd;
                        adapter.Fill(Result);
                    }
                }
            }

            return Result;
        }
        /// <summary>
        /// Executes the SELECT SqlText and returns a DataTable.
        /// <para>ConnectionString must include a DataProvider Alias</para>
        /// </summary> 
        public DataTable Select(string ConnectionString, string SqlText)
        {
            return Select(ConnectionString, SqlText, null);
        }
        /// <summary>
        /// Executes Command and returns a DataTable
        /// </summary> 
        public DataTable Select(DbCommand Command)
        {
            DataTable Result = new DataTable();

            using (DbDataAdapter adapter = CreateAdapter())
            {
                adapter.SelectCommand = Command;
                adapter.Fill(Result);
            }

            return Result;

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
        public DataRow SelectResults(string ConnectionString, string SqlText, params object[] Params)
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
        public DataRow SelectResults(string ConnectionString, string SqlText)
        {
            return SelectResults(ConnectionString, SqlText, null);
        }

        /// <summary>
        /// Ideal for executing SELECT statements of the type "select FIELD_NAME from TABLE_NAME"
        /// </summary>
        public object SelectResult(string ConnectionString, string SqlText, object Default, params object[] Params)
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
        public object SelectResult(string ConnectionString, string SqlText, object Default)
        {
            return SelectResult(ConnectionString, SqlText, Default, null);
        }
        /// <summary>
        /// Ideal for executing SELECT statements of the type "select FIELD_NAME from TABLE_NAME"
        /// </summary>
        public object SelectResult(string ConnectionString, string SqlText)
        {
            return SelectResult(ConnectionString, SqlText, DBNull.Value);
        }

        /// <summary>
        /// Ideal for executing SELECT statements of the type "select count(ID) as COUNT_ID from TABLE_NAME where ID = 1234"
        /// </summary>
        public int IntegerResult(string ConnectionString, string SqlText, int Default, params object[] Params)
        {
            string S = SelectResult(ConnectionString, SqlText, Default, Params).ToString();
            return Sys.StrToInt(S, Default);
        }
        /// <summary>
        /// Ideal for executing SELECT statements of the type "select count(ID) as COUNT_ID from TABLE_NAME where ID = 1234"
        /// </summary>
        public int IntegerResult(string ConnectionString, string SqlText, int Default)
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
        public void ExecSql(string ConnectionString, string SqlText, params object[] Params)
        {
            ConnectionString = ConnectionStringBuilder.RemoveAliasEntry(ConnectionString);

            using (DbConnection Con = OpenConnection(ConnectionString))
            {
                using (DbCommand Cmd = Con.CreateCommand())
                {
                    SetupCommand(Cmd, SqlText, Params);
                    Cmd.ExecuteNonQuery();
                }
            }
        }
        /// <summary>
        /// Executes the DML SqlText.
        /// <para>ConnectionString must include a DataProvider Alias</para>
        /// </summary> 
        public void ExecSql(string ConnectionString, string SqlText)
        {
            ExecSql(ConnectionString, SqlText, null);
        }

        /// <summary>
        /// Returns the current date and time of the database server
        /// </summary>
        public virtual DateTime GetServerDateTime(SqlStore Store)
        {
            return System.DateTime.Now.ToUniversalTime();
        }
        /// <summary>
        /// Quotes and formats a date value as a string, properly for use with an Sql statement
        /// </summary>
        public virtual string QSDate(DateTime Value)
        {
            return Value.ToString("yyyy-MM-dd").QS();
        }
        /// <summary>
        /// Quotes and formats a date-time value as a string, properly for use with an Sql statement
        /// </summary>
        public virtual string QSDateTime(DateTime Value)
        {
            return Value.ToString("yyyy-MM-dd HH:mm:ss").QS();
        }

        /// <summary>
        /// Concatenates two strings.
        /// <para>Example: SELECT FirstName || ' ' || LastName As FullName FROM Customers </para>
        /// <para>Oracle, Firebird, SQLite: || </para>
        /// <para>MsSql, Access : + </para>
        /// </summary>
        public virtual string Concat(string A, string B, string Separator)
        {
            return string.Format("{0} ||{1}|| {2}", A, Separator, B);
        }
        /// <summary>
        /// Concatenates two strings.
        /// <para>Example: SELECT FirstName || LastName As FullName FROM Customers </para>
        /// <para>Oracle, Firebird, SQLite: || </para>
        /// <para>MsSql, Access : + </para>
        /// </summary>
        public virtual string Concat(string A, string B)
        {
            return string.Format(@"{0} || {1}", A, B);
        }
 
        /* public */
        /// <summary>
        /// Replaces data type place-holders contained in the SqlText statement
        /// according to datatypes of the database server.
        /// </summary>
        public virtual string ReplaceDataTypePlaceholders(string SqlText)
        {

            SqlText = SqlText.Replace(CPRIMARY_KEY, PrimaryKey);
            SqlText = SqlText.Replace(CVARCHAR, Varchar);
            SqlText = SqlText.Replace(CNVARCHAR, NVarchar);
            SqlText = SqlText.Replace(CFLOAT, Float);
            SqlText = SqlText.Replace(CDECIMAL, Decimal);
            SqlText = SqlText.Replace(CDATE_TIME, DateTime);
            SqlText = SqlText.Replace(CDATE, Date);
            SqlText = SqlText.Replace(CBOOL, Bool);
            SqlText = SqlText.Replace(CBLOB_TEXT, TextBlob);
            SqlText = SqlText.Replace(CNBLOB_TEXT, NTextBlob);
            SqlText = SqlText.Replace(CBLOB, Blob);
            SqlText = SqlText.Replace(CNOT_NULL, NotNullable);
            SqlText = SqlText.Replace(CNULL, Nullable);

            return SqlText;
        }
 
 
        /// <summary>
        /// Applies the specified RowLimit to the specified SelectSql according to the server technology.
        /// </summary>
        public virtual void ApplyRowLimit(SelectSql SelectSql, int RowLimit)
        {
        }
        /// <summary>
        /// Normalizes RowLimit. If it is less than zero it returns a default limit value.
        /// </summary>
        public int NormalizeRowLimit(int RowLimit)
        {
            if (RowLimit < 0)
                RowLimit = Db.DefaultRowLimit;
            return RowLimit;
        }

        /* alter column */
        /// <summary>
        /// Returns true if this provider supports a specified <see cref="AlterColumnType"/>
        /// </summary>
        public virtual bool SupportsAlterColumnType(AlterColumnType AlterType)
        {
            return Bf.In(AlterType, SupportedAlterColumnTypes);
        }
        
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public virtual string AddColumnSql(string TableName, string ColumnName, string ColumnDef)
        {
            // alter table TableName add ColumnName ColumnDef 
            return $"alter table {TableName} add {ColumnName} {ColumnDef}";
        }
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public virtual string DropColumnSql(string TableName, string ColumnName)
        {
            // alter table TableName drop ColumnName
            return $"alter table {TableName} drop {ColumnName}";
        }
        
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public virtual string RenameColumnSql(string TableName, string ColumnName, string NewColumnName)
        {
            throw new NotSupportedException("rename column not supported");
        }
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public virtual string SetColumnLengthSql(string TableName, string ColumnName, string ColumnDef)
        {
            throw new NotSupportedException("altering column length not supported");
        }

        /// <summary>
        /// Returns an "UPDATE" statement for setting the default value to a column when it is null, i.e. where ColumnName is null.
        /// <para>To be used before setting a "not null" constraint to a column.</para>
        /// </summary>
        public virtual string SetDefaultBeforeNotNullUpdateSql(string TableName, string ColumnName, string DefaultValue)
        {
            return $"update table {TableName} set {ColumnName} = {DefaultValue} where {ColumnName} is null";
        }
       
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public virtual string SetNotNullSql(string TableName, string ColumnName, string ColumnDef)
        {
            throw new NotSupportedException("setting column to not null not supported");
        }
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public virtual string DropNotNullSql(string TableName, string ColumnName, string ColumnDef)
        {
            throw new NotSupportedException("dropping column not null not supported");
        }
        
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public virtual string SetColumnDefaultSql(string TableName, string ColumnName, string DefaultExpression)
        {
            throw new NotSupportedException("setting column default expression not supported");
        }
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public virtual string DropColumnDefaultSql(string TableName, string ColumnName)
        {
            throw new NotSupportedException("dropping column default expression not supported");
        }

        /* generators */
        /// <summary>
        /// Returns true if the GeneratorName exists in a database.
        /// </summary>
        public virtual bool GeneratorExists(string ConnectionString, string GeneratorName)
        {
            return false;
        }
        /// <summary>
        /// Creates the GeneratorName generator to the database.
        /// </summary>
        public virtual void CreateGenerator(string ConnectionString, string GeneratorName)
        {
        }
        /// <summary>
        /// Attempts to set a generator/sequencer to Value.
        /// <para>DANGEROOUS.</para>
        /// </summary>
        public virtual void SetGeneratorTo(string ConnectionString, string GeneratorName, int Value)
        {
        }
        /// <summary>
        /// Attempts to set a generator/sequencer or identity column to Value.
        /// <para>VERY DANGEROOUS.</para>
        /// </summary>
        public virtual void SetTableGeneratorTo(string ConnectionString, string TableName, int Value)
        {
        }
        /// <summary>
        /// Returns the last id produced by an INSERT Sqlt statement.
        /// <para>It should be used only with databases that support identity (auto-increment) columns</para>
        /// </summary>
        public virtual int LastId(SqlStore Store, DbTransaction Transaction, string TableName)
        {
            return -1;
        }
        /// <summary>
        /// Returns the next value of the GeneratorName generator.
        /// </summary>
        public virtual int NextIdByGenerator(SqlStore Store, DbTransaction Transaction, string GeneratorName)
        {
            return -1;
        }

        /* command handling */
        /// <summary>
        /// Creates a <see cref="DbCommand"/> and its <see cref="DbParameter"/>s based on a specified Sql statement and passed in Params.
        /// If the Params is not null, they used in assigning <see cref="DbParameter"/> values.
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
        public DbCommand CreateCommand(DbConnection Connection, string SqlText, params object[] Params)
        {
            DbCommand Result = Connection.CreateCommand();
            if (!string.IsNullOrWhiteSpace(SqlText))
                SetupCommand(Result, SqlText, Params);
            return Result;
        }
        /// <summary>
        /// Sets up a <see cref="DbCommand"/> by assigning a specified Sql statement to <see cref="DbCommand.CommandText"/> 
        /// and creating <see cref="DbParameter"/> instances for that command, based on passed params. <br />
        /// Specifically:
        /// <list type="number">
        /// <item>
        /// Analyzes the passed SqlText which may contain parameter names prefixed by the <see cref="SqlProvider.GlobalPrefix"/>, i.e. <br />
        /// <c>     select * from CUSTOMER where ID > :ID  </c>         
        /// </item>
        /// <item>
        /// Converts the passed SqlText to be what the native ADO.NET Data Provider expects to be and assigns the Command.CommandText. <br />
        /// For the MS Sql the above statement would be converted to <br />
        /// <c>     select * from CUSTOMER where ID > @ID</c> <br />
        /// and for the OleDb would be converted to <br />
        /// <c>     select * from CUSTOMER where ID > ? </c>
        /// </item>
        /// <item>
        /// Creates DbParameter objects and adds them to the Command.Parameters collection. 
        /// The ParameterName of those objects has no prefix, ie. ID.
        /// If required by the provider though, a later call to PrefixToNative() method would arrange the values
        /// of those ParameterName properties to be what the native provider expects to be.
        /// </item>
        /// <item>
        /// If the passed Params is not null and contains parameter values, it assigns the Parameters of the Command
        /// object by calling the AssignParameters() method.
        /// </item>
        /// </list>
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
        public void SetupCommand(DbCommand Command, string SqlText, params object[] Params)
        {
            if ((Params == null) || (Params.Length == 0))
            {
                Command.CommandText = SqlText;
                return;
            }

            int CurPos = 0;
            int StartPos = 0;
            char CurChar;
            bool AppendChar = true;
            bool Literal = false;
            string Name = "";

            StringBuilder SB = new StringBuilder();
            SqlText = SqlText + " ";
            int Len = SqlText.Length;

            Command.Parameters.Clear();

            /* analyze the passed SqlText and create DbParameter objects */
            while (CurPos <= Len - 1)
            {
                AppendChar = true;
                CurChar = SqlText[CurPos];
                if ((CurChar == GlobalPrefix) && (!Literal) && (SqlText[CurPos + 1] != GlobalPrefix))
                {
                    StartPos = CurPos;

                    if (SqlText[CurPos + 1] == ObjectStartDelimiter)
                    {
                        while ((CurPos < Len - 1) && (SqlText[CurPos] != ObjectEndDelimiter))
                        {
                            CurPos++;
                            CurChar = SqlText[CurPos];
                        }

                        Name = SqlText.Substring(StartPos + 1, CurPos - (StartPos));
                        AppendChar = false;
                    }
                    else
                    {
                        while ((CurPos < Len - 1) && (Literal || (!IsNameDelimiter(CurChar))))
                        {
                            CurPos++;
                            CurChar = SqlText[CurPos];
                            if (IsLiteral(CurChar))
                            {
                                Literal = Literal ^ true;
                            }
                        }

                        Name = SqlText.Substring(StartPos + 1, CurPos - (StartPos + 1));
                    }
 
                    DbParameter Param = Command.CreateParameter();
                    Command.Parameters.Add(Param);
                    Param.ParameterName = Name;
                    Param.Value = DBNull.Value;

                    Name = PrefixToNative(Name);

                    SB.Append(Name);

                }
                else if ((CurChar == GlobalPrefix) && (!Literal) && (SqlText[CurPos + 1] == GlobalPrefix))
                {
                    CurPos++;
                    SB.Append(CurChar);
                }
                else if (IsLiteral(CurChar))
                {
                    Literal = Literal ^ true;
                }

                CurPos++;
                if (AppendChar)
                    SB.Append(CurChar);
            }


            /* assign CommandText */
            Command.CommandText = SB.ToString();


            /* assign DbParameter values */
            AssignParameters(Command, Params);

            if (RequiresNativeParamNames)
                PrefixToNative(Command);

        }

        /// <summary>
        /// Passes Params values to Command.Parameters.
        /// Params can be
        /// 1. either a comma separated list of parameters 
        /// 2. or the Params[0] element, that is the first element in Params, may be a DataRow, generic IDictionary, IList or Array
        /// and in that case no other Params elements are used.
        /// </summary>
        public virtual void AssignParameters(DbCommand Command, params object[] Params)
        {
            AssignParameters(Command.Parameters, Params);
        }
        /// <summary>
        /// Passes Params values to Parameters.
        /// Params can be
        /// 1. either a comma separated list of parameters 
        /// 2. or the Params[0] element, that is the first element in Params, may be a DataRow, generic IDictionary, IList or Array
        /// and in that case no other Params elements are used.
        /// </summary>
        public virtual void AssignParameters(IDataParameterCollection Parameters, params object[] Params)
        {
            if ((Params == null) || (Params.Length == 0) || (Parameters == null) || (Parameters.Count == 0))
                return;

            if (Params.Length > 0)
            {
                IDataParameter Parameter;
                for (int i = 0; i < Parameters.Count; i++)
                {
                    Parameter = (IDataParameter)Parameters[i];


                    if (Params[0] is DataRow)
                    {
                        DataRow Row = (DataRow)Params[0];
                        if (Row.Table.Columns.Contains(Parameter.ParameterName))
                        {
                            AssignParameter(Row.Table.Columns[Parameter.ParameterName], Parameter, Row[Parameter.ParameterName]);
#if DEBUG
                            string S = string.Format("{0} {1} {2} ", Parameter.ParameterName,
                                                                    Row.Table.Columns[Parameter.ParameterName].DataType.ToString(),
                                                                    Parameter.DbType.ToString());
                            object SqlDbType = TypeExtensions.GetPublicPropertyValue(Parameter, "SqlDbType");
                            if (SqlDbType != null)
                                S += SqlDbType.ToString();

                            System.Diagnostics.Debug.WriteLine(S);
#endif
                        }
                    }
                    else if (Params[0] is IDictionary<string, object>)
                    {
                        IDictionary<string, object> Dictionary = Params[0] as IDictionary<string, object>;
                        if (Dictionary.ContainsKey(Parameter.ParameterName))
                            AssignParameter(Parameter, Dictionary[Parameter.ParameterName]);
                    }
                    else if (Params[0] is IList)
                    {
                        AssignParameter(Parameter, ((IList)Params[0])[i]);
                    }
                    else if (Params[0] is Array)
                    {
                        AssignParameter(Parameter, ((object[])Params[0])[i]);
                    }
                    else
                    {
                        Parameter.Value = Params[i];
                        AssignParameter(Parameter, Params[i]);
                    }
                }

            }
        }
        /// <summary>
        /// Assignes Parameter from Value. Column is used to infer the type when Value is null or DbNull.
        /// </summary>
        public virtual void AssignParameter(DataColumn Column, IDataParameter Parameter, object Value)
        {
            if (Column == null)
                AssignParameter(Parameter, Value);
            else
            {
                if (!Sys.IsNull(Value))
                    Parameter.Value = Value;
                else
                {
                    Parameter.Value = DBNull.Value;
                    Parameter.DbType = TypeToDbType(Column.DataType);
                }
            }
        }
        /// <summary>
        /// Assignes Parameter from Value
        /// </summary>
        public virtual void AssignParameter(IDataParameter Parameter, object Value)
        {
            if (Sys.IsNull(Value))
                Parameter.Value = DBNull.Value;
            else
                Parameter.Value = Value;
        }
        /// <summary>
        /// Returns the DbType of the Source Type.
        /// </summary>
        public virtual DbType TypeToDbType(Type Source)
        {
            DbType Result;
            try
            {
                if ((Source == typeof(byte[])) || (Source == typeof(object)))
                    Result = DbType.Binary;
                else
                    Result = (DbType)Enum.Parse(typeof(DbType), Source.Name, true);
            }
            catch
            {
                Result = DbType.Object;
            }
            return Result;
        }

        /* miscs */
        /// <summary>
        /// If FieldName contains bad characters such as spaces etc, 
        /// it returns FieldName surrounded by double quotes
        /// </summary>
        public virtual string NormalizeFieldName(string FieldName)
        {
            if (!string.IsNullOrWhiteSpace(FieldName))
            {
                FieldName = FieldName.Trim();

                if (FieldName.IndexOfAny(badFieldCharactes) != -1)
                {
                    if (!FieldName.StartsWith(ObjectStartDelimiter.ToString()) && !FieldName.EndsWith(ObjectEndDelimiter.ToString()))
                    {
                        FieldName = ObjectStartDelimiter.ToString() + FieldName + ObjectEndDelimiter.ToString();
                    }
                    /* 
                    if (!FieldName.StartsWith("\"") && !FieldName.EndsWith("\""))
                    {
                        FieldName = "\"" + FieldName + "\"";
                    }
                    */
                }
            }

            return FieldName;
        }
        /// <summary>
        /// If FieldName is surrounded by double quotes, removes those quotes
        /// </summary>
        public virtual string DenormalizeFieldName(string FieldName)
        {
            if (!string.IsNullOrWhiteSpace(FieldName))
            {
                FieldName = FieldName.Trim();

                if (FieldName.StartsWith(ObjectStartDelimiter.ToString()))
                    FieldName = FieldName.Remove(0, 1);

                if (FieldName.EndsWith(ObjectEndDelimiter.ToString()))
                    FieldName = FieldName.Remove(FieldName.Length - 1, 1);
            }

            return FieldName;
        }

        /// <summary>
        /// Returns a parameter name properly prefixed.
        /// Ensures that the ParemeterName is prefixed by the NativePrefix, in regard to PrefixMode.
        /// If PrefixMode is PrefixMode.Prefixed it returns prefix + param, ie. @CUSTOMER_NAME or :CUSTOMER_NAME
        /// If PrefixMode is Positional it just returns the position mark, ie. ?
        /// </summary>
        public virtual string CreateParamName(string FieldName)
        {
            return PrefixToNative(FieldName);
        }

        /// <summary>
        /// Removes any prefix from the ParameterName
        /// </summary>
        public virtual string PrefixRemove(string ParameterName)
        {
            if ((ParameterName.StartsWith(NativePrefix.ToString())) || (ParameterName.StartsWith(GlobalPrefix.ToString())))
                return ParameterName.Substring(1, ParameterName.Length - 1);
            return ParameterName;
        }
        /// <summary>
        /// Ensures that the ParemeterName is prefixed by the NativePrefix, in regard to PrefixMode.
        /// If PrefixMode is PrefixMode.Prefixed it returns prefix + param, ie. @CUSTOMER_NAME or :CUSTOMER_NAME
        /// If PrefixMode is Positional it just returns the position mark, ie. ?
        /// </summary>
        public virtual string PrefixToNative(string ParameterName)
        {
            if (this.PrefixMode == PrefixMode.Prefixed)
            {
                return NativePrefix.ToString() + PrefixRemove(ParameterName);
            }
            else  // PrefixMod.Positional
            {
                return NativePrefix.ToString();
            }
        }
        /// <summary>
        /// Ensures that the ParameterName is prefixed by the DataProviders.GlobalPrefix
        /// </summary>
        public virtual string PrefixToGlobal(string ParameterName)
        {
            return GlobalPrefix.ToString() + PrefixRemove(ParameterName);
        }

        /// <summary>
        /// Removes any prefix from the ParameterName of each Parameter in the Command
        /// </summary>
        public void PrefixRemove(DbCommand Command)
        {
            foreach (DbParameter Parameter in Command.Parameters)
                Parameter.ParameterName = PrefixRemove(Parameter.ParameterName);
        }
        /// <summary>
        /// Ensures that the ParemeterName of each Parameter in the Command is prefixed by the NativePrefix, in regard to PrefixMode.
        /// If PrefixMode is PrefixMode.Prefixed it returns prefix + param, ie. @CUSTOMER_NAME or :CUSTOMER_NAME
        /// If PrefixMode is Positional it just returns the position mark, ie. ?
        /// </summary>
        public void PrefixToNative(DbCommand Command)
        {
            foreach (DbParameter Parameter in Command.Parameters)
                Parameter.ParameterName = PrefixToNative(Parameter.ParameterName);
        }
        /// <summary>
        /// Ensures that the ParameterName of each Parameter in the Command is prefixed by the DataProviders.GlobalPrefix
        /// </summary>
        public void PrefixToGlobal(DbCommand Command)
        {
            foreach (DbParameter Parameter in Command.Parameters)
                Parameter.ParameterName = PrefixToGlobal(Parameter.ParameterName);
        }

 
        /// <summary>
        /// Creates and returns a connection string. The various conStrXXXX formats
        /// are used as string formats. 
        /// </summary>
        public virtual string FormatConnectionString(string HostComputer, string FileOrDatabaseName)
        {
            return string.Empty;
        }
        /// <summary>
        /// Creates and returns a connection string. The various conStrXXXX formats
        /// are used as string formats. The localhost is used as host computer name
        /// </summary>
        public virtual string FormatConnectionString(string FileOrDatabaseName)
        {
            return FormatConnectionString("localhost", FileOrDatabaseName);
        }



        /* properties */
        /// <summary>
        /// The name of the provider, e.g. MsSql, Oracle, SQLite, etc.
        /// </summary>
        public string Name { get; }
        /// <summary>
        /// Gets the Description this Provider
        /// </summary>
        public virtual string Description { get; } = "no description";

        /// <summary>
        /// The provider factory.
        /// <para>If called and is not already assigned it tries to load the factory dynamically using reflection.</para>
        /// </summary>
        public virtual DbProviderFactory Factory
        {
            get
            {
                if (fFactory == null)
                {
                    fFactory = LoadDbProviderFactory(this.DbProviderFactoryTypeName, AssemblyFileName);
                }

                return fFactory;
            }
            set { fFactory = value; }
        }
        /// <summary>
        /// The file name of the provider assembly, e.g. FirebirdSql.Data.FirebirdClient.dll
        /// </summary>
        public virtual string AssemblyFileName { get; }
        /// <summary>
        /// The class name of the DbProviderFactory, e.g. FirebirdSql.Data.FirebirdClient.FirebirdClientFactory
        /// </summary>
        public virtual string DbProviderFactoryTypeName { get; }

        /// <summary>
        /// The type of the <see cref="SqlStore"/> class to use when creating <see cref="SqlStore"/> instances.
        /// </summary>
        public virtual Type SqlStoreClass { get; set; } = typeof(SqlStore);
        /// <summary>
        /// The server type, e.g. MsSql, SQLite, Oracle, etc.
        /// </summary>
        public virtual SqlServerType ServerType { get; } = SqlServerType.Unknown;
        /// <summary>
        /// Gets the MidwareType of this DataProvider
        /// </summary>
        public virtual MidwareType MidwareType { get; } = MidwareType.Direct;
        /// <summary>
        /// Gets the OidMode of this DataProvider
        /// </summary>
        public virtual OidMode OidMode { get; } = OidMode.Unknown;

        /// <summary>
        /// The character used in the provided Sql statements, to be used as a "global" prefix.
        /// <para>It is replaced back to what the provider expects to be, just before the command execution.</para>
        /// </summary>
        static public char GlobalPrefix { get; set; } = ':';
        /// <summary>
        /// The provider native variable prefix.  
        /// <para>For MsSql and Firebird is @</para>
        /// <para>For Oracle and SQLite is :</para>
        /// <para>For ODBC and ADO providers there is no prefix, just a ? as a positional placeholder for the parameter value.</para>
        /// </summary>
        public virtual char NativePrefix { get; } = '@';
        /// <summary>
        /// Indicates the kind of the parameter name and prefix the ADO.NET Data Provider uses
        /// </summary>
        public virtual PrefixMode PrefixMode { get; } = PrefixMode.Prefixed;
        /// <summary>
        /// The ADO.NET Data Provider expects no prefix or name at all. It uses the question mark as a positional placeholder
        /// </summary>
        public virtual bool PositionalPlaceholders { get; } = false;
        /// <summary>
        /// Returns true if parameters names should be normalized back to what the
        /// provider expects to be, after parameter value assignment
        /// </summary>
        public virtual bool RequiresNativeParamNames { get; } = false;

        /// <summary>
        /// Database object start delimiter.
        /// <para>For MsSql is [ while for all others is " </para>
        /// </summary>
        public virtual char ObjectStartDelimiter { get; } =  '"';
        /// <summary>
        /// Database object end delimiter
        /// <para>For MsSql is ] while for all others is " </para>
        /// </summary>
        public virtual char ObjectEndDelimiter { get; } = '"';

        /// <summary>
        /// The template for a connection string
        /// </summary>
        public virtual string ConnectionStringTemplate { get; } = ""; 

        /// <summary>
        /// Returns true if the database server supports transactions
        /// </summary>
        public virtual bool SupportsTransactions { get; } = true;
        /// <summary>
        /// Returns true if the provider can create a new database
        /// </summary>
        public virtual bool CanCreateDatabases { get; } = false;
        /// <summary>
        /// Returns true if the database server supports generators/sequencers
        /// </summary>
        public virtual bool SupportsGenerators { get; } = false;

        /// <summary>
        /// Returns a set (bit-field) of the supported <see cref="AlterColumnType"/>s.
        /// </summary>
        public virtual AlterColumnType SupportedAlterColumnTypes { get; } = (AlterColumnType)Bf.All(typeof(AlterColumnType));

        /// <summary>
        /// Keys used in connection string by this provider
        /// </summary>
        public virtual string[] ServerKeys { get; } = { };
        /// <summary>
        /// Keys used in connection string by this provider
        /// </summary>
        public virtual string[] DatabaseKeys { get; } = { };
        /// <summary>
        /// Keys used in connection string by this provider
        /// </summary>
        public virtual string[] UserNameKeys { get; } = { };
        /// <summary>
        /// Keys used in connection string by this provider
        /// </summary>
        public virtual string[] PasswordKeys { get; } = { };



        /// <summary>
        /// The PrimaryKey text
        /// </summary>
        public virtual string PrimaryKey { get; } = "integer not null primary key";
        /// <summary>
        /// The Varchar text
        /// </summary>
        public virtual string Varchar { get; } = "varchar";
        /// <summary>
        /// The NVarchar text
        /// </summary>
        public virtual string NVarchar { get; } = "varchar";
        /// <summary>
        /// The Float text
        /// </summary>
        public virtual string Float { get; } = "float";
        /// <summary>
        /// The Decimal text
        /// </summary>
        public virtual string Decimal { get; } = "decimal(18, 4)";
        /// <summary>
        /// The Date text
        /// </summary>
        public virtual string Date { get; } = "date";
        /// <summary>
        /// The DateTime text
        /// </summary>
        public virtual string DateTime { get; } = "timestamp";
        /// <summary>
        /// The Bool text
        /// </summary>
        public virtual string Bool { get; } = "integer";
        /// <summary>
        /// The Blob text
        /// </summary>
        public virtual string Blob { get; } = "";
        /// <summary>
        /// The TextBlob text
        /// </summary>
        public virtual string TextBlob { get; } = "";
        /// <summary>
        /// The NTextBlob text
        /// </summary>
        public virtual string NTextBlob { get; } = "";
        /// <summary>
        /// The NotNullable text
        /// </summary>
        public virtual string NotNullable { get; } = "not null";
        /// <summary>
        /// The Nullable text
        /// </summary>
        public virtual string Nullable { get; } = " ";
    }
}
