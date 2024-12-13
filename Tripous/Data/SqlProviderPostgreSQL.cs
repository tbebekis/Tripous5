namespace Tripous.Data
{

    /// <summary>
    /// Describes a Sql provider
    /// <para>NOTE: Add the NuGet package https://www.nuget.org/packages/Npgsql/  </para>
    /// </summary>
    public class SqlProviderPostgreSQL: SqlProvider
    {
        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Factory">The provider factory</param>      
        public SqlProviderPostgreSQL(DbProviderFactory Factory = null)
            : base(PostgreSQL, Factory)
        {
        }


        /* alter column */
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string RenameColumnSql(string TableName, string ColumnName, string NewColumnName)
        {
            // alter table {TableName} rename column {ColumnName} to {NewColumnName} 
            return $"alter table {TableName} rename column {ColumnName} to {NewColumnName}  ";
        }
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string SetColumnLengthSql(string TableName, string ColumnName, string DataType, string Required, string DefaultExpression)
        {
            // alter table {TableName} alter column {ColumnName} type {DataType}  
            return $"alter table {TableName} alter column {ColumnName} type {DataType} ";
        }

        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string SetNotNullSql(string TableName, string ColumnName, string DataType)
        {
            // update {TableName} set {ColumnName} = {DefaultExpression} where {ColumnName} is null; 
            // alter table {TableName} alter column {ColumnName} set not null  
            return $"alter table {TableName} alter column {ColumnName} set not null";
        }
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string DropNotNullSql(string TableName, string ColumnName, string DataType)
        {
            // alter table {TableName} alter column {ColumnName} drop not null 
            return $"alter table {TableName} alter column {ColumnName} drop not null";
        }

        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string SetColumnDefaultSql(string TableName, string ColumnName, string DefaultExpression)
        {
            // alter table {TableName} alter column {ColumnName} set default {DefaultExpression}
            return $"alter table {TableName} alter column {ColumnName} set default {DefaultExpression}";
        }
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string DropColumnDefaultSql(string TableName, string ColumnName)
        {
            // alter table {TableName} alter column {ColumnName} drop default
            return $@"alter table {TableName} alter column {ColumnName} drop default";
        }
 

        /* methods */
        /// <summary>
        /// Creates a new database, if not exists. Returns true only if creates the database.
        /// <para> By default databases are stored in <code>C:\Program Files\PostgreSQL\{VERSION}\data</code> or<code>/var/lib/pgsql/data</code></para>
        /// <para>The<code>postgresql.conf</code> contains a "data_directory" entry which defines where databases are stored.</para>
        /// <para>Also the <code>pg_env.bat</code> sets that "data_directory" with the PGDATA environment variable, e.g. <code>C:\Program Files\PostgreSQL\{VERSION}\data</code> </para>
        /// <para>Inside that "data" folder there is a "data/base" folder which is where the databases can be found.</para>
        /// <para>Inside that "base" folder there is a folder for each database. The folder name of a database is the OID of the database returned when executing 
        /// <code>SELECT* FROM pg_catalog.pg_database</code> in the postgres database.</para>
        /// <para>WARNING: It seems that database names are case-sensitive, with a twist. A connection string as
        /// <code>"Server=localhost; Database=MyDataBase; User Id=postgres; Password=password;"</code>
        /// results to a "mydatabase" in the <code>pg_catalog.pg_database</code> after creating the database. </para>
        /// <para>After that the server cannot find a database named "MyDataBase" and you get the "PostgreSQL 3D000: database does not exist" error.</para>
        /// <para>CAUTION: Always define database name in lower case in the connection string.</para>
        /// </summary>
        public override bool CreateDatabase(string ConnectionString)
        {
            bool Result = false;

            if (!DatabaseExists(ConnectionString))
            {
                ConnectionStringBuilder CSB = new ConnectionStringBuilder(ConnectionString);
                string DatabaseName = CSB.Database;

                CSB["Database"] = "postgres";
                string CS = CSB.ConnectionString;

                using (var Con = OpenConnection(CS))
                {
                    using (var Cmd = Con.CreateCommand())
                    {
                        Cmd.CommandText = $"create database {DatabaseName}";
                        Cmd.ExecuteNonQuery();

                        // NOTE: There is a problem here: Although the database is created any attempt to connect to it
                        // results in an exception. It seems that although the database is created, is not yet
                        // ready or attached or something. So the only solution I found is to wait for a while. 
                        for (int i = 0; i < 10; i++)
                        {
                            if (CanConnect(ConnectionString, ThrowIfNot: false))
                                break;

                            System.Threading.Thread.Sleep(1000);
                        } 

                        Result = true;
                    }
                }
            }
 
            return Result;
        }
        /// <summary>
        /// Applies the specified RowLimit to the specified SelectSql according to the server technology.
        /// </summary>
        public override void ApplyRowLimit(SelectSql SelectSql, int RowLimit)
        {
            RowLimit = NormalizeRowLimit(RowLimit);
            if (RowLimit <= 0)
                return;

            string S;

            // select * from T where Id > 10 order by Id limit 400 
 
            S = $@" 
limit {RowLimit}";

            if (!string.IsNullOrWhiteSpace(SelectSql.OrderBy))
                SelectSql.OrderBy += S;
            else if (!string.IsNullOrWhiteSpace(SelectSql.Having))
                SelectSql.Having += S;
            else if (!string.IsNullOrWhiteSpace(SelectSql.GroupBy))
                SelectSql.GroupBy += S;
            else if (!string.IsNullOrWhiteSpace(SelectSql.Where))
                SelectSql.Where += S;

        }

        /// <summary>
        /// Returns the current date and time of the database server
        /// </summary>
        public override DateTime GetServerDateTime(SqlStore Store)
        {
            DateTime Result = base.GetServerDateTime(Store);
            object Value = Store.SelectResult("select CURRENT_TIMESTAMP", Result);
            Result = Convert.ToDateTime(Value);
            return Result;
        }
        /// <summary>
        /// Returns the last id produced by an INSERT Sqlt statement.
        /// <para>It should be used only with databases that support identity (auto-increment) columns</para>
        /// </summary>
        public override int LastId(SqlStore Store, DbTransaction Transaction, string TableName)
        {
            string SqlText = string.Format("SELECT LASTVAL() AS RESULT", TableName);
            int Result = Store.IntegerResult(Transaction, SqlText, 0);
            return Result;
        }


        /// <summary>
        /// Returns true if the GeneratorName exists in a database.
        /// </summary>
        public override bool GeneratorExists(string ConnectionString, string GeneratorName)
        {
            string SqlText = $"SELECT count(sequence_name) FROM information_schema.sequences WHERE sequence_name = '{GeneratorName}' ;";  
            return this.IntegerResult(ConnectionString, SqlText, -1) > 0;
        }
        /// <summary>
        /// Creates the GeneratorName generator to the database.
        /// </summary>
        public override void CreateGenerator(string ConnectionString, string GeneratorName)
        {
            GeneratorName = GeneratorName.ToUpper(System.Globalization.CultureInfo.InvariantCulture);
            string SqlText = $"CREATE SEQUENCE IF NOT EXISTS {GeneratorName} ;";
            this.ExecSql(ConnectionString, SqlText);
        }
        /// <summary>
        /// Attempts to set a generator/sequencer to Value.
        /// <para>DANGEROOUS.</para>
        /// </summary>
        public override void SetGeneratorTo(string ConnectionString, string GeneratorName, int Value)
        {
            GeneratorName = GeneratorName.ToUpper(System.Globalization.CultureInfo.InvariantCulture);
            string SqlText = $"SELECT setval('{GeneratorName}', {Value}) ;";
            this.ExecSql(ConnectionString, SqlText);
        }
        /// <summary>
        /// Attempts to set a generator/sequencer or identity column to Value.
        /// <para>VERY DANGEROOUS.</para>
        /// </summary>
        public override void SetTableGeneratorTo(string ConnectionString, string TableName, int Value)
        {
            if (GeneratorExists(ConnectionString, "G_" + TableName))
                SetGeneratorTo(ConnectionString, "G_" + TableName, Value);
        }
        /// <summary>
        /// Returns the next value of the GeneratorName generator.
        /// </summary>
        public override int NextIdByGenerator(SqlStore Store, DbTransaction Transaction, string GeneratorName)
        { 
            GeneratorName = GeneratorName.ToUpper(System.Globalization.CultureInfo.InvariantCulture);
            string SqlText = $"SELECT nextval('{GeneratorName}') ;";  
            return Store.IntegerResult(Transaction, SqlText, -1);
        }



        /* properties */
        /// <summary>
        /// Gets the Description this Provider
        /// </summary>
        public override string Description { get; } = "PostgreSQL";

        /// <summary>
        /// The file name of the provider assembly, e.g. FirebirdSql.Data.FirebirdClient.dll
        /// </summary>
        public override string AssemblyFileName { get; } = "Npgsql.dll";
        /// <summary>
        /// The class name of the DbProviderFactory, e.g. FirebirdSql.Data.FirebirdClient.FirebirdClientFactory
        /// </summary>
        public override string DbProviderFactoryTypeName { get; } = "Npgsql.NpgsqlFactory";

        /// <summary>
        /// The type of the <see cref="SqlStore"/> class to use when creating <see cref="SqlStore"/> instances.
        /// </summary>
        public override Type SqlStoreClass { get; set; } = typeof(SqlStore);
        /// <summary>
        /// The server type, e.g. MsSql, SQLite, Oracle, etc.
        /// </summary>
        public override SqlServerType ServerType { get; } = SqlServerType.PostgreSQL;
        /// <summary>
        /// Gets the MidwareType of this DataProvider
        /// </summary>
        public override MidwareType MidwareType { get; } = MidwareType.Direct;
        /// <summary>
        /// Gets the OidMode of this DataProvider
        /// </summary>
        public override OidMode OidMode { get; } = OidMode.Along;

        /// <summary>
        /// The provider native variable prefix.  
        /// <para>For MsSql and Firebird is @</para>
        /// <para>For Oracle and SQLite is :</para>
        /// <para>For ODBC and ADO providers there is no prefix, just a ? as a positional placeholder for the parameter value.</para>
        /// </summary>
        public override char NativePrefix { get; } = '@';
        /// <summary>
        /// Indicates the kind of the parameter name and prefix the ADO.NET Data Provider uses
        /// </summary>
        public override PrefixMode PrefixMode { get; } = PrefixMode.Prefixed;
        /// <summary>
        /// The ADO.NET Data Provider expects no prefix or name at all. It uses the question mark as a positional placeholder
        /// </summary>
        public override bool PositionalPlaceholders { get; } = false;
        /// <summary>
        /// Returns true if parameters names should be normalized back to what the
        /// provider expects to be, after parameter value assignment
        /// </summary>
        public override bool RequiresNativeParamNames { get; } = false;

        /// <summary>
        /// Database object start delimiter.
        /// <para>For MsSql is [ while for all others is " </para>
        /// </summary>
        public override char ObjectStartDelimiter { get; } = '"';
        /// <summary>
        /// Database object end delimiter
        /// <para>For MsSql is ] while for all others is " </para>
        /// </summary>
        public override char ObjectEndDelimiter { get; } = '"';

        /// <summary>
        /// The template for a connection string
        /// </summary>
        public override string ConnectionStringTemplate { get; } = @"Server={0}; Database={1}; User Id={2}; Password={3};";
        /// <summary>
        /// Super user name
        /// </summary>
        public override string SuperUser { get; } = "postgres";
        /// <summary>
        /// Super user password.
        /// <para>NOTE: No super user default password.</para>
        /// </summary>
        public override string SuperUserPassword { get; } = "";

        /// <summary>
        /// Returns true if the database server supports transactions
        /// </summary>
        public override bool SupportsTransactions { get; } = true;
        /// <summary>
        /// Returns true if the provider can create a new database
        /// </summary>
        public override bool CanCreateDatabases { get; } = true;
        /// <summary>
        /// Returns true if the database server supports generators/sequencers
        /// </summary>
        public override bool SupportsGenerators { get; } = true;
        /// <summary>
        /// Returns true when the database server supports auto-increment integer fields.
        /// </summary>
        public override bool SupportsAutoIncFields { get; } = true;

        /// <summary>
        /// Keys used in connection string by this provider
        /// </summary>
        public override string[] ServerKeys { get; } = new string[] { "Server" };
        /// <summary>
        /// Keys used in connection string by this provider
        /// </summary>
        public override string[] DatabaseKeys { get; } = new string[] { "Database" };
        /// <summary>
        /// Keys used in connection string by this provider
        /// </summary>
        public override string[] UserNameKeys { get; } = new string[] { "User Id" };
        /// <summary>
        /// Keys used in connection string by this provider
        /// </summary>
        public override string[] PasswordKeys { get; } = new string[] { "Password" };

        /// <summary>
        /// The PrimaryKey text
        /// </summary>
        public override string PrimaryKey { get; } = "serial not null primary key";
        /// <summary>
        /// Auto-increment field, when supported, else exception.
        /// </summary>
        public override string AutoInc { get { return "serial"; } }
        /// <summary>
        /// The Varchar text
        /// </summary>
        public override string Varchar { get; } = "varchar";
        /// <summary>
        /// The NVarchar text
        /// </summary>
        public override string NVarchar { get; } = "varchar";
        /// <summary>
        /// The Float text
        /// </summary>
        public override string Float { get; } = "double precision";
        /// <summary>
        /// The Decimal text. 
        /// <para>Implied Precision and Scale <c>(18, 4)</c></para>
        /// <para>Example: <c>@DECIMAL</c> becomes <c>decimal(18, 4)</c></para>
        /// </summary>
        public override string Decimal { get; } = "decimal(18, 4)";
        /// <summary>
        /// The Decimal text. 
        /// <para>The user provides the Precision and Scale explicitly.</para>
        /// <para>Example: <c>@DECIMAL_(10, 2)</c> becomes <c>decimal(10, 2)</c></para>
        /// </summary>
        public override string Decimal_ { get; } = "decimal";
        /// <summary>
        /// The Date text
        /// </summary>
        public override string Date { get; } = "date";
        /// <summary>
        /// The DateTime text
        /// </summary>
        public override string DateTime { get; } = "timestamp";
        /// <summary>
        /// The Bool text
        /// </summary>
        public override string Bool { get; } = "integer"; // "boolean";
        /// <summary>
        /// The Blob text
        /// </summary>
        public override string Blob { get; } = "bytea";
        /// <summary>
        /// The TextBlob text
        /// </summary>
        public override string TextBlob { get; } = "text";
        /// <summary>
        /// The NTextBlob text
        /// </summary>
        public override string NTextBlob { get; } = "text";
        /// <summary>
        /// The NotNullable text
        /// </summary>
        public override string NotNullable { get; } = "not null";
        /// <summary>
        /// The Nullable text
        /// </summary>
        public override string Nullable { get; } = " ";
    }
}
