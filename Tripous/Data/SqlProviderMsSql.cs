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

namespace Tripous.Data
{

    /// <summary>
    /// Describes a Sql provider
    /// <para>NOTE: Add the NuGet package https://www.nuget.org/packages/System.Data.SqlClient </para>
    /// </summary>
    public class SqlProviderMsSql: SqlProvider
    {
        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Factory">The provider factory</param>      
        public SqlProviderMsSql(DbProviderFactory Factory = null)
            : base(MsSql, Factory)
        {
        }
 

        /* public */
        /// <summary>
        /// Returns true if the database exists
        /// </summary>
        public bool DatabaseExists(string ServerName, string DatabaseName, string UserName, string Password)
        {
            string CS = string.Format("Data Source={0}; Initial Catalog={1}; User ID={2}; Password={3}; ", ServerName, DatabaseName, UserName, Password);
            return CanConnect(CS, true);
        }
        /// <summary>
        /// Creates a new database
        /// </summary>
        public override bool CreateDatabase(string ServerName, string DatabaseName, string UserName, string Password)
        {
            bool Result = false;
            if (!DatabaseExists(ServerName, DatabaseName, UserName, Password))
            {
                string CS;
                if (!string.IsNullOrEmpty(UserName) && !string.IsNullOrEmpty(Password))
                    CS = string.Format("Data Source={0}; Initial Catalog=master; User ID={1}; Password={2}; ", ServerName, UserName, Password);
                else
                    CS = string.Format("Data Source={0}; Initial Catalog=master; Integrated Security=SSPI; ", ServerName);

                using (var Con = Factory.CreateConnection())
                {
                    Con.ConnectionString = CS;
                    Con.Open();

                    using (var Cmd = Factory.CreateCommand())
                    {
                        Cmd.Connection = Con;

                        Cmd.CommandText = string.Format("create database \"{0}\"  ", DatabaseName);
                        Cmd.ExecuteNonQuery();

                        /*  NOTE: There is a problem here: Although the database is created any attempt to connect to it
                            results in an exception. It seems that although the database is created, is not yet
                            ready or attached or something. So the only solution I found is to wait for a while. */
                        System.Threading.Thread.Sleep(7000);

                        Result = true;
                    }

                }
            }

            return Result;
        }

        /// <summary>
        /// Returns the current date and time of the database server
        /// </summary>
        public override DateTime GetServerDateTime(SqlStore Store)
        {
            DateTime Result = base.GetServerDateTime(Store);
            object Value = Store.SelectResult("SELECT CURRENT_TIMESTAMP", Result);
            Result = Convert.ToDateTime(Value);
            return Result;
        }
        /// <summary>
        /// Applies the specified RowLimit to the specified SelectSql according to the server technology.
        /// </summary>
        public override void ApplyRowLimit(SelectSql SelectSql, int RowLimit)
        {
            bool Flag;
            string S;

            // SELECT TOP 10 * FROM T
            // S = string.Format("TOP {0} " + SelectSql.Select, RowLimit);

            S = SelectSql.Select.Trim();
            Flag = S.StartsWithText("distinct");
            if (Flag)
                S = S.Remove(0, "distinct".Length);

            S = string.Format("TOP {0}   " + Environment.NewLine + "  ", RowLimit) + S;

            if (Flag)
                S = "distinct " + Environment.NewLine + "  " + S;

            SelectSql.Select = S;
        }
        /// <summary>
        /// Returns an Sql statement for altering a table column
        /// </summary>
        public override string GetAlterTableColumnSql(AlterColumnType AlterType, string TableName, string ColumnName, string ColumnDef)
        {
            switch (AlterType)
            {
                case AlterColumnType.Add: return string.Format("alter table {0} add {1} {2}", TableName, ColumnName, ColumnDef);
                case AlterColumnType.Alter: return string.Format("alter table {0} alter column {1} {2}", TableName, ColumnName, ColumnDef);
                case AlterColumnType.Drop: return string.Format("alter table {0} drop column {1}", TableName, ColumnName);
                case AlterColumnType.Rename: return string.Format("aexec sp_rename @objname = '{0}.{1}', @newname = '{2}', @objtype = 'COLUMN' ", TableName, ColumnName, ColumnDef);
            }

            return base.GetAlterTableColumnSql(AlterType, TableName, ColumnName, ColumnDef);
        }
        /// <summary>
        /// Concatenates two strings.
        /// <para>Example: SELECT FirstName || ' ' || LastName As FullName FROM Customers </para>
        /// <para>Oracle, Firebird, SQLite: || </para>
        /// <para>MsSql, Access : + </para>
        /// </summary>
        public override string Concat(string A, string B, string Separator)
        {
            return string.Format("{0} +{1}+ {2}", A, Separator, B);
        }
        /// <summary>
        /// Concatenates two strings.
        /// <para>Example: SELECT FirstName || LastName As FullName FROM Customers </para>
        /// <para>Oracle, Firebird, SQLite: || </para>
        /// <para>MsSql, Access : + </para>
        /// </summary>
        public override string Concat(string A, string B)
        {
            return string.Format("{0} + {1}", A, B);
        }

        /// <summary>
        /// Attempts to set a generator/sequencer or identity column to Value.
        /// <para>VERY DANGEROOUS.</para>
        /// </summary>
        public override void SetTableGeneratorTo(string ConnectionString, string TableName, int Value)
        {
            string SqlText = string.Format("DBCC CHECKIDENT ({0}, RESEED, {1})", TableName, Value);
            this.ExecSql(ConnectionString, SqlText);
        }
        /// <summary>
        /// Returns the last id produced by an INSERT Sqlt statement.
        /// <para>It should be used only with databases that support identity (auto-increment) columns</para>
        /// </summary>
        public override int LastId(SqlStore Store, DbTransaction Transaction, string TableName)
        {
            string SqlText = string.Format("SELECT IDENT_CURRENT('{0}') AS RESULT", TableName);
            int Result = Store.IntegerResult(Transaction, SqlText, 0);
            return Result;
        }


        /* properties */
        /// <summary>
        /// Gets the Description this Provider
        /// </summary>
        public override string Description { get; } = "Microsoft Sql Server";

        /// <summary>
        /// The file name of the provider assembly, e.g. FirebirdSql.Data.FirebirdClient.dll
        /// </summary>
        public override string AssemblyFileName { get; } = "System.Data.SqlClient.dll";
        /// <summary>
        /// The class name of the DbProviderFactory, e.g. FirebirdSql.Data.FirebirdClient.FirebirdClientFactory
        /// </summary>
        public override string DbProviderFactoryTypeName { get; } = "System.Data.SqlClient.SqlClientFactory";
 
        /// <summary>
        /// The type of the <see cref="SqlStore"/> class to use when creating <see cref="SqlStore"/> instances.
        /// </summary>
        public override Type SqlStoreClass { get; set; } = typeof(SqlStore);
        /// <summary>
        /// The server type, e.g. MsSql, SQLite, Oracle, etc.
        /// </summary>
        public override SqlServerType ServerType { get; } = SqlServerType.MsSql;
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
        public override char ObjectStartDelimiter { get; } = '[';
        /// <summary>
        /// Database object end delimiter
        /// <para>For MsSql is ] while for all others is " </para>
        /// </summary>
        public override char ObjectEndDelimiter { get; } = ']';

        /// <summary>
        /// The template for a connection string
        /// </summary>
        public override string ConnectionStringTemplate { get; } = "Data Source={0}; Initial Catalog={1}; User ID={2}; Password={3};";
        
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
        public override string[] ServerKeys { get; } = new string[] { "Data Source", "Server", "Address", "Addr", "Network Address" };
        /// <summary>
        /// Keys used in connection string by this provider
        /// </summary>
        public override string[] DatabaseKeys { get; } = new string[] { "Initial Catalog", "Database" };
        /// <summary>
        /// Keys used in connection string by this provider
        /// </summary>
        public override string[] UserNameKeys { get; } = new string[] { "User ID", "UID" };
        /// <summary>
        /// Keys used in connection string by this provider
        /// </summary>
        public override string[] PasswordKeys { get; } = new string[] { "Password", "PWD" };

        /// <summary>
        /// The PrimaryKey text
        /// </summary>
        public override string PrimaryKey { get; } = "integer identity not null primary key";
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
        public override string Bool { get; } = "bit";
        /// <summary>
        /// The Blob text
        /// </summary>
        public override string Blob { get; } = "image";
        /// <summary>
        /// The TextBlob text
        /// </summary>
        public override string TextBlob { get; } = "ntext";
        /// <summary>
        /// The NTextBlob text
        /// </summary>
        public override string NTextBlob { get; } = "ntext";
        /// <summary>
        /// The NotNullable text
        /// </summary>
        public override string NotNullable { get; } = "not null";
        /// <summary>
        /// The Nullable text
        /// </summary>
        public override string Nullable { get; } = "null";
    }
}
