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
    /// <para>SEE: https://firebirdsql.org/file/documentation/html/en/refdocs/fblangref40/firebird-40-language-reference.html </para>
    /// <para>SEE: https://www.ibphoenix.com/files/Authentication_FB3.pdf </para>
    /// <para>SEE: https://stackoverflow.com/a/67902886/1779320 </para>
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


        /* alter column */
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string RenameColumnSql(string TableName, string ColumnName, string NewColumnName)
        {
            // alter table {TableName} alter column {ColumnName} to {NewColumnName} 
            return $"alter table {TableName} alter column {ColumnName} to {NewColumnName} ";
        }
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// <para>NOTE: Firebird column size changes by using the "type" keyword, NOT a full column definition.</para>
        /// <para>Example: <code>alter table TableName alter ColumnName type varchar(100)</code> </para>
        /// </summary>
        public override string SetColumnLengthSql(string TableName, string ColumnName, string DataType, string Required, string DefaultExpression)
        {
            // ALTER TABLE t1 ALTER c1 TYPE char(90);
            // alter table {TableName} alter column {ColumnName} type {DataType} {Required}   

            return $"alter table {TableName} alter column {ColumnName} type {DataType}";
        }

        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string SetNotNullSql(string TableName, string ColumnName, string DataType)
        {
            // update {TableName} set {ColumnName} = {DefaultExpression} where {ColumnName} is null; 
            // alter table {TableName} alter {ColumnName} set not null   
            return $"alter table {TableName} alter {ColumnName} set not null ";
        }
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string DropNotNullSql(string TableName, string ColumnName, string DataType)
        {
            // alter table {TableName} alter {ColumnName} drop not null 
            return $"alter table {TableName} alter {ColumnName} drop not null";
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
        /// </summary>
        public override bool CreateDatabase(string ConnectionString)
        {
            bool Result = false;

            if (!DatabaseExists(ConnectionString))
            {  
                string CS = ConnectionStringBuilder.ReplacePathPlaceholders(ConnectionString);

                using (var Con = Factory.CreateConnection())
                {
                    // FbConnection class - public static void CreateDatabase(string connectionString, int pageSize, bool forcedWrites, bool overwrite)
                    Type Type = Con.GetType();
                    MethodInfo Method = Type.GetMethod("CreateDatabase", new Type[] { typeof(string), typeof(int), typeof(bool), typeof(bool) });

                    if (Method != null)
                    {
                        Method.Invoke(null, new object[] { CS, 1024 * 16, true, false });
                        Result = true;

                        // NOTE: There is a problem here: Although the database is created any attempt to connect to it
                        // results in an exception. It seems that although the database is created, is not yet
                        // ready or attached or something. So the only solution I found is to wait for a while. 
                        for (int i = 0; i < 10; i++)
                        {
                            if (CanConnect(ConnectionString, ThrowIfNot: false))
                                break;

                            System.Threading.Thread.Sleep(1000);
                        }
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
            // NO
            // 
            // see: https://ib-aid.com/download/docs/firebird-language-reference-2.5/fblangref25-ddl-tbl.html#fblangref25-ddl-tbl-create

            /* NO, do NOT add the "CHARACTER SET UTF8"
               Collation name is the last entry in a Column Def, just after the Column Constraint.
               see: https://ib-aid.com/download/docs/firebird-language-reference-2.5/fblangref25-ddl-tbl.html#fblangref25-ddl-tbl-create 

                <regular_col_def> ::=
                  colname {<datatype> | domainname}
                  [DEFAULT {literal | NULL | <context_var>}]
                  [NOT NULL]
                  [<col_constraint>]
                  [COLLATE collation_name] 
             */

            /*
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
             */


            return base.ReplaceDataTypePlaceholders(SqlText);
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
        /// The template for a connection string.
        /// <para>WARNING: Database without a path goes to C:\Windows\System32 folder by default. </para>
        /// </summary>
        public override string ConnectionStringTemplate { get; } = @"DataSource={0}; Database={1}; User={2}; Password={3}; Charset=UTF8;";
        /// <summary>
        /// Super user name
        /// </summary>
        public override string SuperUser { get; } = "SYSDBA";
        /// <summary>
        /// Super user password
        /// </summary>
        public override string SuperUserPassword { get; } = "masterkey";

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
