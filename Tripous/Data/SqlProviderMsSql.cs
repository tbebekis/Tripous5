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
    /// <para>NOTE: Add the NuGet package https://www.nuget.org/packages/Microsoft.Data.SqlClient </para>
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
        /// Creates a new database, if not exists. Returns true only if creates the database.
        /// </summary>
        public override bool CreateDatabase(string ConnectionString)
        {
            bool Result = false;

            if (!DatabaseExists(ConnectionString))
            {
                ConnectionStringBuilder CSB = new ConnectionStringBuilder(ConnectionString);
                string DatabaseName = CSB.Database;

                CSB["Initial Catalog"] = "master";
                string CS = CSB.ConnectionString;

                using (var Con = OpenConnection(CS))
                {
                    using (var Cmd = Con.CreateCommand())
                    {
                        Cmd.CommandText = string.Format("create database \"{0}\"  ", DatabaseName);
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

        /* alter column */
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string RenameColumnSql(string TableName, string ColumnName, string NewColumnName)
        {
            // exec sp_rename N'TableName.ColumnName', 'NewColumnName', 'COLUMN'
            return $"exec sp_rename N'{TableName}.{ColumnName}', '{NewColumnName}', 'COLUMN'"; 
        }
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string SetColumnLengthSql(string TableName, string ColumnName, string DataType, string Required, string DefaultExpression)
        {
            // alter table {TableName} alter column {ColumnName} {DataType} {Required}
            return $"alter table {TableName} alter column {ColumnName} {DataType} {Required}";
        }

        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string SetNotNullSql(string TableName, string ColumnName, string DataType)
        {
            // update TableName set ColumnName = DefaultValue where ColumnName is null;
            //  alter table {TableName} alter column {ColumnName} {DataType} not null
            return $"alter table {TableName} alter column {ColumnName} {DataType} not null";
        }
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string DropNotNullSql(string TableName, string ColumnName, string DataType)
        {
            // alter table {TableName} alter column {ColumnName} {DataType} null
            return $"alter table {TableName} alter column {ColumnName} {DataType} null";
        }

        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string SetColumnDefaultSql(string TableName, string ColumnName, string DefaultExpression)
        {
            // alter table {TableName} add default {DefaultExpression} for {ColumnName}
            return $"alter table {TableName} add default {DefaultExpression} for {ColumnName}";
        }
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string DropColumnDefaultSql(string TableName, string ColumnName)
        { 
            return $@"
declare @ConstraintName nvarchar(100);

select @ConstraintName = OBJECT_NAME([default_object_id]) 
from SYS.COLUMNS
where [object_id] = OBJECT_ID('{TableName}') AND [name] = '{ColumnName}';

exec('ALTER TABLE {TableName} DROP CONSTRAINT ' +  @ConstraintName)
";
        }
 

        /* miscs */
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
            RowLimit = NormalizeRowLimit(RowLimit);
            if (RowLimit <= 0)
                return;

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
        public override string AssemblyFileName { get; } = "Microsoft.Data.SqlClient.dll";
        /// <summary>
        /// The class name of the DbProviderFactory, e.g. FirebirdSql.Data.FirebirdClient.FirebirdClientFactory
        /// </summary>
        public override string DbProviderFactoryTypeName { get; } = "Microsoft.Data.SqlClient.SqlClientFactory";
 
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
        public override string ConnectionStringTemplate { get; } = "Data Source={0}; Initial Catalog={1}; User ID={2}; Password={3}; TrustServerCertificate=true;";
        /// <summary>
        /// Super user name
        /// </summary>
        public override string SuperUser { get; } = "sa";
        /// <summary>
        /// Super user password
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
        public override bool SupportsGenerators { get; } = false;
        /// <summary>
        /// Returns true when the database server supports auto-increment integer fields.
        /// </summary>
        public override bool SupportsAutoIncFields { get; } = true;

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
        /// Auto-increment field, when supported, else exception.
        /// </summary>
        public override string AutoInc { get; } = "integer identity";
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
        public override string TextBlob { get; } = "nvarchar(max)"; // ntext
        /// <summary>
        /// The NTextBlob text
        /// </summary>
        public override string NTextBlob { get; } = "nvarchar(max)"; // ntext
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
