﻿/*--------------------------------------------------------------------------------------        
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
using System.IO;


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


        /* methods */
        /// <summary>
        /// Returns true if the database exists
        /// </summary>
        public bool DatabaseExists(string ServerName, string DatabaseName, string UserName, string Password)
        {
            string CS = $"Server={ServerName}; Port=5432; Database={DatabaseName}; User Id={UserName}; Password={Password}";
            return CanConnect(CS, true);
        }
        /// <summary>
        /// Creates a new database
        /// </summary>
        /// <summary>
        /// Creates a new database
        /// </summary>
        public override bool CreateDatabase(string ServerName, string DatabaseName, string UserName, string Password)
        {
            bool Result = false;
            if (!DatabaseExists(ServerName, DatabaseName, UserName, Password))
            {
                string CS = $"Server={ServerName}; Port=5432; User Id={UserName}; Password={Password}";

                using (var Con = Factory.CreateConnection())
                {
                    Con.ConnectionString = CS;
                    Con.Open();

                    using (var Cmd = Factory.CreateCommand())
                    {
                        Cmd.Connection = Con;

                        Cmd.CommandText = $"create database {DatabaseName}";
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
        /// Applies the specified RowLimit to the specified SelectSql according to the server technology.
        /// </summary>
        public override void ApplyRowLimit(SelectSql SelectSql, int RowLimit)
        {
            string S;

            // select * from T where Id > 10 order by Id limit 400 



            S = $@" 
limit {RowLimit}";

            if (!string.IsNullOrEmpty(SelectSql.OrderBy.Trim()))
                SelectSql.OrderBy += S;
            else if (!string.IsNullOrEmpty(SelectSql.Having.Trim()))
                SelectSql.Having += S;
            else if (!string.IsNullOrEmpty(SelectSql.GroupBy.Trim()))
                SelectSql.GroupBy += S;
            else if (!string.IsNullOrEmpty(SelectSql.Where.Trim()))
                SelectSql.Where += S;

        }
        /// <summary>
        /// Returns an Sql statement for altering a table column
        /// </summary>
        public override string GetAlterTableColumnSql(AlterColumnType AlterType, string TableName, string ColumnName, string ColumnDef)
        {
            switch (AlterType)
            {
                // ALTER TABLE table_name ADD column_name datatype;
                case AlterColumnType.Add: return string.Format("alter table {0} add {1} {2}", TableName, ColumnName, ColumnDef);

                // ALTER TABLE table_name ALTER COLUMN column_name TYPE datatype;
                case AlterColumnType.Alter: return string.Format("alter table {0} alter column {1} {2}", TableName, ColumnName, ColumnDef);

                // ALTER TABLE table_name DROP COLUMN column_name;
                case AlterColumnType.Drop: return string.Format("alter table {0} drop column {1}", TableName, ColumnName);

                // ALTER TABLE table_name RENAME column_name TO new_column_name;
                case AlterColumnType.Rename: return string.Format("alter table {0} rename column {1} to {2}", TableName, ColumnName, ColumnDef);
            }

            return base.GetAlterTableColumnSql(AlterType, TableName, ColumnName, ColumnDef);
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
        /// The Decimal text
        /// </summary>
        public override string Decimal { get; } = "decimal(18, 4)";
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
        public override string Bool { get; } = "boolean";
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
