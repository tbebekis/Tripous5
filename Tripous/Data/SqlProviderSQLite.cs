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
using System.IO;
using System.Reflection;

using System.Data.SQLite;

namespace Tripous.Data
{
    /// <summary>
    /// Describes a Sql provider
    /// <para>NOTE: Tripous already references System.Data.SQLite.dll (.Net Standard 2.0) </para>
    /// </summary>
    public class SqlProviderSQLite : SqlProvider
    {

        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Factory">The provider factory</param>      
        public SqlProviderSQLite(DbProviderFactory Factory = null)
            : base(SQLite, Factory)
        {
        }

        /* public */
        /// <summary>
        /// Returns true if the database represented by the specified database exists, by checking the connection.
        /// </summary>
        public override bool DatabaseExists(string ConnectionString)
        {
            ConnectionStringBuilder CSB = new ConnectionStringBuilder(ConnectionString);
            string FilePath = CSB.Database;
            FilePath = ConnectionStringBuilder.ReplacePathPlaceholders(FilePath);
            return !string.IsNullOrWhiteSpace(FilePath) && File.Exists(FilePath);
        }
        /// <summary>
        /// Creates a new database, if not exists. Returns true only if creates the database.
        /// </summary>
        public override bool CreateDatabase(string ConnectionString)
        {
            bool Result = false;

            ConnectionStringBuilder CSB = new ConnectionStringBuilder(ConnectionString);
            string FilePath = CSB.Database;
            FilePath = ConnectionStringBuilder.ReplacePathPlaceholders(FilePath);

            if (!string.IsNullOrWhiteSpace(FilePath) && !File.Exists(FilePath))
            {
                string Folder = Path.GetDirectoryName(FilePath);

                if (!string.IsNullOrWhiteSpace(Folder) && !Directory.Exists(Folder))
                    Directory.CreateDirectory(Folder);

                SQLiteConnection.CreateFile(FilePath);
                Result = true;
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
            else
                SelectSql.From += S;

        }

        /// <summary>
        /// Returns an Sql statement for altering a table column
        /// </summary>
        public override string GetAlterTableColumnSql(AlterColumnType AlterType, string TableName, string ColumnName, string ColumnDef)
        {
            switch (AlterType)
            {
                case AlterColumnType.Add: return string.Format("alter table {0} add column {1} {2}", TableName, ColumnName, ColumnDef);
                    // NOT SUPPORTED case AlterColumnType.Alter:  
                    // NOT SUPPORTED case AlterColumnType.Drop: return "";
                    // NOT SUPPORTED case AlterColumnType.Rename: return "";
            }

            return base.GetAlterTableColumnSql(AlterType, TableName, ColumnName, ColumnDef);
        }
        /// <summary>
        /// Attempts to set a generator/sequencer or identity column to Value.
        /// <para>VERY DANGEROOUS.</para>
        /// </summary>
        public override void SetTableGeneratorTo(string ConnectionString, string TableName, int Value)
        {
            string SqlText = string.Format("update sqlite_sequence set seq = {0} where name = '{1}'", Value, TableName);
            this.ExecSql(ConnectionString, SqlText);
        }
        /// <summary>
        /// Returns the last id produced by an INSERT Sqlt statement.
        /// <para>It should be used only with databases that support identity (auto-increment) columns</para>
        /// </summary>
        public override int LastId(SqlStore Store, DbTransaction Transaction, string TableName)
        {
            string SqlText = string.Format("select seq AS RESULT from sqlite_sequence where name = '{0}' ", TableName);
            int Result = Store.IntegerResult(Transaction, SqlText, 0);
            return Result;
        }
 
        /* properties */
        /// <summary>
        /// Gets the Description this Provider
        /// </summary>
        public override string Description { get; } = "SQLite3";

        /// <summary>
        /// The provider factory.
        /// <para>If called and is not already assigned it tries to load the factory dynamically using reflection.</para>
        /// </summary>
        public override DbProviderFactory Factory
        {
            get
            {
                if (fFactory == null)
                {
                    fFactory = System.Data.SQLite.SQLiteFactory.Instance;
                }

                return fFactory;
            }
            set { fFactory = value; }
        }

        /// <summary>
        /// The file name of the provider assembly, e.g. FirebirdSql.Data.FirebirdClient.dll
        /// </summary>
        public override string AssemblyFileName { get; } = "System.Data.SQLite.dll";
        /// <summary>
        /// The class name of the DbProviderFactory, e.g. FirebirdSql.Data.FirebirdClient.FirebirdClientFactory
        /// </summary>
        public override string DbProviderFactoryTypeName { get; } = "System.Data.SQLite.SQLiteFactory";

        /// <summary>
        /// The type of the <see cref="SqlStore"/> class to use when creating <see cref="SqlStore"/> instances.
        /// </summary>
        public override Type SqlStoreClass { get; set; } = typeof(SqlStore);
        /// <summary>
        /// The server type, e.g. MsSql, SQLite, Oracle, etc.
        /// </summary>
        public override SqlServerType ServerType { get; } = SqlServerType.SQLite;
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
        public override char NativePrefix { get; } = ':';
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
        public override string ConnectionStringTemplate { get; } = @"Data Source=""{0}"";";

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
        public override bool SupportsGenerators { get; } = false;

        /// <summary>
        /// Keys used in connection string by this provider
        /// </summary>
        public override string[] ServerKeys { get; } = new string[] { };
        /// <summary>
        /// Keys used in connection string by this provider
        /// </summary>
        public override string[] DatabaseKeys { get; } = new string[] { "Data Source" };
        /// <summary>
        /// Keys used in connection string by this provider
        /// </summary>
        public override string[] UserNameKeys { get; } = new string[] { };
        /// <summary>
        /// Keys used in connection string by this provider
        /// </summary>
        public override string[] PasswordKeys { get; } = new string[] { };

        /// <summary>
        /// The PrimaryKey text
        /// </summary>
        public override string PrimaryKey { get; } = "integer not null primary key autoincrement";
        /// <summary>
        /// The Varchar text
        /// </summary>
        public override string Varchar { get; } = "varchar";
        /// <summary>
        /// The NVarchar text
        /// </summary>
        public override string NVarchar { get; } = "nvarchar";
        /// <summary>
        /// The Float text
        /// </summary>
        public override string Float { get; } = "float";
        /// <summary>
        /// The Decimal text
        /// </summary>
        public override string Decimal { get; } = "decimal(18, 4)";
        /// <summary>
        /// The Date text
        /// </summary>
        public override string Date { get; } = "datetime";
        /// <summary>
        /// The DateTime text
        /// </summary>
        public override string DateTime { get; } = "datetime";
        /// <summary>
        /// The Bool text
        /// </summary>
        public override string Bool { get; } = "integer";
        /// <summary>
        /// The Blob text
        /// </summary>
        public override string Blob { get; } = "blob";
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
