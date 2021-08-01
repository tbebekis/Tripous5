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
    /// <para>NOTE: Add the NuGet package https://www.nuget.org/packages/FirebirdSql.Data.FirebirdClient/  </para>
    /// </summary>
    public class SqlProviderFirebird: SqlProvider
    {
        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Factory">The provider factory</param>      
        public SqlProviderFirebird(DbProviderFactory Factory = null)
            : base(Firebird, Factory)
        {
        }

        /* methods */
        /// <summary>
        /// Returns true if the database exists
        /// </summary>
        public bool DatabaseExists(string ServerName, string DatabaseName, string UserName, string Password)
        {
            string CS = string.Format("DataSource={0}; Database={1}; User={2}; Password={3}; Charset=UTF8;", ServerName, DatabaseName, UserName, Password);
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
                string CS = string.Format("DataSource={0}; Database={1}; User={2}; Password={3}; Charset=UTF8;", ServerName, DatabaseName, UserName, Password);

                using (DbConnection Con = OpenConnection(CS))
                {
                    Type Type = Con.GetType();
                    MethodInfo Method = Type.GetMethod("CreateDatabase", new Type[] { typeof(string), typeof(int), typeof(bool), typeof(bool) });

                    if (Method != null)
                    {
                        Method.Invoke(null, new object[] { CS, 1024 * 16, true, false });
                        Result = true;

                        System.Threading.Thread.Sleep(3000);
                    }
                }
            }

            return Result;
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

                SqlText = SqlText.Insert(Pos + 1, " CHARACTER SET UTF8 ");  // CHARACTER SET UNICODE_FSS

                Index = SqlText.IndexOf(CNVARCHAR, Pos);
            }

            return base.ReplaceDataTypePlaceholders(SqlText);
        }
        /// <summary>
        /// Applies the specified RowLimit to the specified SelectSql according to the server technology.
        /// </summary>
        public override void ApplyRowLimit(SelectSql SelectSql, int RowLimit)
        {
            bool Flag;
            string S;

            // SELECT FIRST x [SKIP y] ... [rest of query]
            // S = string.Format("FIRST {0} " + SelectSql.Select, RowLimit);
            S = SelectSql.Select.Trim();
            Flag = S.StartsWithText("distinct");
            if (Flag)
                S = S.Remove(0, "distinct".Length);

            S = string.Format("FIRST {0} " + Environment.NewLine + "  ", RowLimit) + S;

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
                case AlterColumnType.Drop: return string.Format("alter table {0} drop {1}", TableName, ColumnName);
                case AlterColumnType.Rename: return string.Format("alter table {0} alter column {1} to {2}", TableName, ColumnName, ColumnDef);
            }

            return base.GetAlterTableColumnSql(AlterType, TableName, ColumnName, ColumnDef);
        }

        /// <summary>
        /// Returns the current date and time of the database server
        /// </summary>
        public override DateTime GetServerDateTime(SqlStore Store)
        {
            DateTime Result = base.GetServerDateTime(Store);
            object Value = Store.SelectResult("select current_timestamp from rdb$database", Result);
            Result = Convert.ToDateTime(Value);
            return Result;
        }


        /// <summary>
        /// Returns true if the GeneratorName exists in a database.
        /// </summary>
        public override bool GeneratorExists(string ConnectionString, string GeneratorName)
        {
            string SqlText = string.Format("select count(RDB$GENERATOR_NAME) as CountResult from RDB$GENERATORS where RDB$GENERATOR_NAME = '{0}' ", GeneratorName);
            return this.IntegerResult(ConnectionString, SqlText, -1) > 0;
        }
        /// <summary>
        /// Creates the GeneratorName generator to the database.
        /// </summary>
        public override void CreateGenerator(string ConnectionString, string GeneratorName)
        {
            GeneratorName = GeneratorName.ToUpper(System.Globalization.CultureInfo.InvariantCulture);
            string SqlText = "create generator " + GeneratorName;
            this.ExecSql(ConnectionString, SqlText);
        }
        /// <summary>
        /// Attempts to set a generator/sequencer to Value.
        /// <para>DANGEROOUS.</para>
        /// </summary>
        public override void SetGeneratorTo(string ConnectionString, string GeneratorName, int Value)
        {
            GeneratorName = GeneratorName.ToUpper(System.Globalization.CultureInfo.InvariantCulture);

            string SqlText = string.Format("set generator {0} to {1}", GeneratorName, Value);

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
            string SqlText = string.Format("SELECT GEN_ID({0}, 1) as NEXT_ID FROM RDB$DATABASE", GeneratorName);
            return Store.IntegerResult(Transaction, SqlText, -1);
        }


        /* properties */
        /// <summary>
        /// Gets the Description this Provider
        /// </summary>
        public override string Description { get; } = "Firebird";

        /// <summary>
        /// The file name of the provider assembly, e.g. FirebirdSql.Data.FirebirdClient.dll
        /// </summary>
        public override string AssemblyFileName { get; } = "FirebirdSql.Data.FirebirdClient.dll";
        /// <summary>
        /// The class name of the DbProviderFactory, e.g. FirebirdSql.Data.FirebirdClient.FirebirdClientFactory
        /// </summary>
        public override string DbProviderFactoryTypeName { get; } = "FirebirdSql.Data.FirebirdClient.FirebirdClientFactory";

        /// <summary>
        /// The type of the <see cref="SqlStore"/> class to use when creating <see cref="SqlStore"/> instances.
        /// </summary>
        public override Type SqlStoreClass { get; set; } = typeof(SqlStore);
        /// <summary>
        /// The server type, e.g. MsSql, SQLite, Oracle, etc.
        /// </summary>
        public override SqlServerType ServerType { get; } = SqlServerType.Firebird;
        /// <summary>
        /// Gets the MidwareType of this DataProvider
        /// </summary>
        public override MidwareType MidwareType { get; } = MidwareType.Direct;
        /// <summary>
        /// Gets the OidMode of this DataProvider
        /// </summary>
        public override OidMode OidMode { get; } = OidMode.Before;

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
        public override string ConnectionStringTemplate { get; } = @"DataSource={0}; Database={1}; User={2}; Password={3}; Charset=UTF8;";

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
        /// Keys used in connection string by this provider
        /// </summary>
        public override string[] ServerKeys { get; } = new string[] { "DataSource" };
        /// <summary>
        /// Keys used in connection string by this provider
        /// </summary>
        public override string[] DatabaseKeys { get; } = new string[] { "Database" };
        /// <summary>
        /// Keys used in connection string by this provider
        /// </summary>
        public override string[] UserNameKeys { get; } = new string[] { "User" };
        /// <summary>
        /// Keys used in connection string by this provider
        /// </summary>
        public override string[] PasswordKeys { get; } = new string[] { "Password" };

        /// <summary>
        /// The PrimaryKey text
        /// </summary>
        public override string PrimaryKey { get; } = "integer  not null primary key";
        /// <summary>
        /// The Varchar text
        /// </summary>
        public override string Varchar { get; } = "varchar";
        /// <summary>
        /// The NVarchar text
        /// </summary>
        public override string NVarchar { get; } = "varchar";  // VARCHAR(x) CHARACTER SET UNICODE_FSS
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
        public override string Bool { get; } = "integer";
        /// <summary>
        /// The Blob text
        /// </summary>
        public override string Blob { get; } = "BLOB SUB_TYPE 0 SEGMENT SIZE 80";                   // https://www.ibphoenix.com/resources/documents/general/doc_54
        /// <summary>
        /// The TextBlob text
        /// </summary>
        public override string TextBlob { get; } = "BLOB SUB_TYPE TEXT SEGMENT SIZE 80";
        /// <summary>
        /// The NTextBlob text
        /// </summary>
        public override string NTextBlob { get; } = "BLOB SUB_TYPE TEXT SEGMENT SIZE 80";
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
