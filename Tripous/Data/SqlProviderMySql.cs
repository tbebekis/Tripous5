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
using System.IO;


namespace Tripous.Data
{
    /// <summary>
    /// Describes a Sql provider
    /// <para>NOTE: Add the NuGet package https://www.nuget.org/packages/MySql.Data  </para>
    /// </summary>
    public class SqlProviderMySql: SqlProvider
    {
        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Factory">The provider factory</param>      
        public SqlProviderMySql(DbProviderFactory Factory = null)
            : base(MySql, Factory)
        {
        }

        /* alter column */
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string RenameColumnSql(string TableName, string ColumnName, string NewColumnName)
        {
            // alter table {TableName} rename column {ColumnName} to {NewColumnName} 
            return $"alter table {TableName} rename column {ColumnName} to {NewColumnName} ";
        }
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string SetColumnLengthSql(string TableName, string ColumnName, string ColumnDef)
        {
            // alter table {TableName} modify column {ColumnName} {ColumnDef}
            ColumnDef = ReplaceDataTypePlaceholders(ColumnDef);
            return $"alter table {TableName} modify column {ColumnName} {ColumnDef}";
        }

        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string SetNotNullSql(string TableName, string ColumnName, string DataType)
        {
            // update {TableName} set {ColumnName} = {DefaultExpression} where {ColumnName} is null; 
            // alter table {TableName} modify column {ColumnName} {DataType} not null
            DataType = ReplaceDataTypePlaceholders(DataType);
            return $"alter table {TableName} modify column {ColumnName} {DataType} not null";
        }
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string DropNotNullSql(string TableName, string ColumnName, string DataType)
        {
            // alter table {TableName} modify column {ColumnName} {DataType} null
            DataType = ReplaceDataTypePlaceholders(DataType);
            return $"alter table {TableName} modify column {ColumnName} {DataType} null";
        }

        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string SetColumnDefaultSql(string TableName, string ColumnName, string DefaultExpression)
        {
            // alter table {TableName} alter {ColumnName} set default {DefaultExpression}
            return $"alter table {TableName} alter {ColumnName} set default {DefaultExpression}";
        }
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string DropColumnDefaultSql(string TableName, string ColumnName)
        {
            // alter table {TableName} alter {ColumnName} drop default
            return $@"alter table {TableName} alter {ColumnName} drop default";
        }

        /* constraints */
 
        /// <summary>
        /// Returns an "alter table" SQL statement for dropping a unique constraint
        /// </summary>
        public override string DropUniqueConstraintSql(string TableName, string ConstraintName)
        {
            return $"alter table {TableName} drop index {ConstraintName}";
        }
 
        /// <summary>
        /// Returns an "alter table" SQL statement for dropping a foreign key constraint
        /// </summary>
        public override string DropForeignKeySql(string TableName, string ConstraintName)
        {
            return $"alter table {TableName} drop foreign key {ConstraintName}";
        }

        /* methods */
        /// <summary>
        /// Creates a new database, if not exists. Returns true only if creates the database.
        /// </summary>
        public override bool CreateDatabase(string ConnectionString)
        {
            bool Result = false;

            if (!DatabaseExists(ConnectionString))
            {
                ConnectionStringBuilder CSB = new ConnectionStringBuilder(ConnectionString);
                string DatabaseName = CSB.Database;
                string CS = CSB.RemoveDatabaseEntry();

                using (var Con = OpenConnection(CS))
                {
                    using (var Cmd = Con.CreateCommand())
                    {
                        Cmd.CommandText = string.Format("CREATE DATABASE IF NOT EXISTS `{0}`;", DatabaseName);
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

            // select * from T where Id > 10 order by Id limit 0, 400 
 
            S = $@" 
limit 0, {RowLimit}";

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
        /// Replaces data type place-holders contained in the SqlText statement
        /// according to datatypes of the database server.
        /// </summary>
        public override string ReplaceDataTypePlaceholders(string SqlText)
        {
            SqlText = SqlText.Replace("  ", " ");

            int Plus = CNVARCHAR.Length;
            int Index = SqlText.IndexOf(CNVARCHAR, 0);
            int Pos;
            while (Index != -1)
            {
                Pos = SqlText.IndexOf(')', Index + Plus);
                if (Pos == -1)
                    throw new ApplicationException("Invalid create table statement");

                SqlText = SqlText.Insert(Pos + 1, " CHARACTER SET UTF8 ");

                Index = SqlText.IndexOf(CNVARCHAR, Pos);
            }

            return base.ReplaceDataTypePlaceholders(SqlText);
        }


        /* properties */
        /// <summary>
        /// Gets the Description this Provider
        /// </summary>
        public override string Description { get; } = "MySql";

        /// <summary>
        /// The file name of the provider assembly, e.g. FirebirdSql.Data.FirebirdClient.dll
        /// </summary>
        public override string AssemblyFileName { get; } = "MySql.Data.dll";
        /// <summary>
        /// The class name of the DbProviderFactory, e.g. FirebirdSql.Data.FirebirdClient.FirebirdClientFactory
        /// </summary>
        public override string DbProviderFactoryTypeName { get; } = "MySql.Data.MySqlClient.MySqlClientFactory";

        /// <summary>
        /// The type of the <see cref="SqlStore"/> class to use when creating <see cref="SqlStore"/> instances.
        /// </summary>
        public override Type SqlStoreClass { get; set; } = typeof(SqlStore);
        /// <summary>
        /// The server type, e.g. MsSql, SQLite, Oracle, etc.
        /// </summary>
        public override SqlServerType ServerType { get; } = SqlServerType.MySql;
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
        public override char ObjectStartDelimiter { get; } = '`';
        /// <summary>
        /// Database object end delimiter
        /// <para>For MsSql is ] while for all others is " </para>
        /// </summary>
        public override char ObjectEndDelimiter { get; } = '`';

        /// <summary>
        /// The template for a connection string
        /// </summary>
        public override string ConnectionStringTemplate { get; } = @"Server={0}; Database={1}; Uid={2}; Pwd={3};";

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
        public override string[] UserNameKeys { get; } = new string[] { "Uid" };
        /// <summary>
        /// Keys used in connection string by this provider
        /// </summary>
        public override string[] PasswordKeys { get; } = new string[] { "Pwd" };

        /// <summary>
        /// The PrimaryKey text
        /// </summary>
        public override string PrimaryKey { get; } = "int NOT NULL AUTO_INCREMENT";
        /// <summary>
        /// The Varchar text
        /// </summary>
        public override string Varchar { get; } = "VARCHAR";
        /// <summary>
        /// The NVarchar text
        /// </summary>
        public override string NVarchar { get; } = "VARCHAR"; // VARCHAR(20) CHARACTER SET utf8,
        /// <summary>
        /// The Float text
        /// </summary>
        public override string Float { get; } = "FLOAT";
        /// <summary>
        /// The Decimal text
        /// </summary>
        public override string Decimal { get; } = "DECIMAL(18, 4)";
        /// <summary>
        /// The Date text
        /// </summary>
        public override string Date { get; } = "DATE";
        /// <summary>
        /// The DateTime text
        /// </summary>
        public override string DateTime { get; } = "DATETIME";
        /// <summary>
        /// The Bool text
        /// </summary>
        public override string Bool { get; } = "BOOL";
        /// <summary>
        /// The Blob text
        /// </summary>
        public override string Blob { get; } = "LONGBLOB";
        /// <summary>
        /// The TextBlob text
        /// </summary>
        public override string TextBlob { get; } = "LONGTEXT";
        /// <summary>
        /// The NTextBlob text
        /// </summary>
        public override string NTextBlob { get; } = "LONGTEXT CHARACTER SET UTF8";
        /// <summary>
        /// The NotNullable text
        /// </summary>
        public override string NotNullable { get; } = "NOT NULL";
        /// <summary>
        /// The Nullable text
        /// </summary>
        public override string Nullable { get; } = " ";

    }
}
