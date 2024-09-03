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
    /// <para>NOTE: Add the NuGet package https://www.nuget.org/packages/Oracle.ManagedDataAccess/ 
    /// or https://www.nuget.org/packages/Oracle.ManagedDataAccess.Core/ </para>
    /// </summary>
    public class SqlProviderOracle : SqlProvider
    {
        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="Factory">The provider factory</param>      
        public SqlProviderOracle(DbProviderFactory Factory = null)
            : base(Oracle, Factory)
        {
        }

        /* alter column */
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string RenameColumnSql(string TableName, string ColumnName, string NewColumnName)
        {
            // alter table {TableName} rename column {ColumnName} to {NewColumnName}
            return $"alter table {TableName} rename column {ColumnName} to {NewColumnName}";
        }
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string SetColumnLengthSql(string TableName, string ColumnName, string DataType, string Required, string DefaultExpression)
        {
            // alter table {TableName} modify {ColumnName} {DataType} {Required}
            return $"alter table {TableName} modify {ColumnName} {DataType} {Required}";
        }

        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string SetNotNullSql(string TableName, string ColumnName, string DataType)
        {
            // update {TableName} set {ColumnName} = {DefaultExpression} where {ColumnName} is null; 
            // alter table {TableName} modify {ColumnName} {DataType} not null
            return $"alter table {TableName} modify {ColumnName} {DataType} not null";
        }
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string DropNotNullSql(string TableName, string ColumnName, string DataType)
        {
            // alter table {TableName} modify {ColumnName} {DataType} null
            return $"alter table {TableName} modify {ColumnName} {DataType} null";
        }

        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string SetColumnDefaultSql(string TableName, string ColumnName, string DefaultExpression)
        {
            // alter table {TableName} modify {ColumnName} default {DefaultExpression}
            return $"alter table {TableName} modify {ColumnName} default {DefaultExpression}";
        }
        /// <summary>
        /// Returns an "alter column" SQL statement.
        /// </summary>
        public override string DropColumnDefaultSql(string TableName, string ColumnName)
        {
            // alter table {TableName} alter column {ColumnName} drop default
            return $@"alter table {TableName} alter column {ColumnName} drop default";
        }

 

        /* public */
        /// <summary>
        /// Returns the current date and time of the database server
        /// </summary>
        public override DateTime GetServerDateTime(SqlStore Store)
        {
            DateTime Result = base.GetServerDateTime(Store);
            object Value = Store.SelectResult("SELECT TO_CHAR(SYSDATE, 'YYYY-MM-DD HH24:MI:SS') FROM Dual", Result);
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

            string S;

            // SELECT * from T WHERE ROWNUM <= 10 
            // S = string.IsNullOrEmpty(SelectSql.Where.Trim()) ? "ROWNUM <= {0}" : "ROWNUM <= {0} and " + SelectSql.Where;
            // S = string.Format(S, RowLimit);

            S = string.Format("ROWNUM <= {0}", RowLimit);
            if (!string.IsNullOrWhiteSpace(SelectSql.Where.Trim()))
                S = S + Environment.NewLine + "     and " + SelectSql.Where;

            SelectSql.Where = S;
        }
 
        /// <summary>
        /// Quotes and formats a date value as a string, properly for use with an Sql statement
        /// </summary>
        public override string QSDate(DateTime Value)
        {
            // to_date('2010-12-14:09:56:53', 'YYYY-MM-DD:HH24:MI:SS')
            return string.Format("to_date('{0}', 'YYYY-MM-DD')", Value.ToString("yyyy-MM-dd"));
        }
        /// <summary>
        /// Quotes and formats a date-time value as a string, properly for use with an Sql statement
        /// </summary>
        public override string QSDateTime(DateTime Value)
        {
            // to_date('2010-12-14:09:56:53', 'YYYY-MM-DD:HH24:MI:SS')
            return string.Format("to_date('{0}', 'YYYY-MM-DD:HH24:MI:SS')", Value.ToString("yyyy-MM-dd HH:mm:ss"));
        }


        /// <summary>
        /// Returns true if the GeneratorName exists in a database.
        /// </summary>
        public override bool GeneratorExists(string ConnectionString, string GeneratorName)
        {
            string SqlText = string.Format("select count(SEQUENCE_NAME) as CountResult from ALL_SEQUENCES where SEQUENCE_NAME = '{0}' ", GeneratorName);
            return this.IntegerResult(ConnectionString, SqlText, -1) > 0;
        }
        /// <summary>
        /// Creates the GeneratorName generator to the database.
        /// </summary>
        public override void CreateGenerator(string ConnectionString, string GeneratorName)
        {
            GeneratorName = GeneratorName.ToUpper(System.Globalization.CultureInfo.InvariantCulture);
            string SqlText = "CREATE SEQUENCE " + GeneratorName;
            this.ExecSql(ConnectionString, SqlText);
        }
        /// <summary>
        /// Attempts to set a generator/sequencer to Value.
        /// <para>DANGEROOUS.</para>
        /// </summary>
        public override void SetGeneratorTo(string ConnectionString, string GeneratorName, int Value)
        {
            /* see: 
                   http://asktom.oracle.com/pls/asktom/f?p=100:11:0::::P11_QUESTION_ID:1119633817597
                   http://stackoverflow.com/questions/51470/how-do-i-reset-a-sequence-in-oracle
            */

            GeneratorName = GeneratorName.ToUpper(System.Globalization.CultureInfo.InvariantCulture);


            /* always to zero */
            /* get the current value */
            string SqlText = string.Format("select {0}.NEXTVAL from DUAL", GeneratorName);
            int OldValue = this.IntegerResult(ConnectionString, SqlText, -1);

            /* subtract it  */
            if (OldValue > 0)
            {
                SqlText = string.Format("alter sequence {0} increment by -{1}  minvalue 0", GeneratorName, OldValue);
                this.ExecSql(ConnectionString, SqlText);


                /* select again */
                SqlText = string.Format("select {0}.NEXTVAL from DUAL", GeneratorName);
                Value = this.IntegerResult(ConnectionString, SqlText, -1);
            }


            /* reset it */
            SqlText = string.Format("alter sequence {0} increment by {1} minvalue 0", GeneratorName, Value);
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
            string SqlText = string.Format("select {0}.NEXTVAL from DUAL", GeneratorName);
            return Store.IntegerResult(Transaction, SqlText, -1);
        }
 

        /* properties */
        /// <summary>
        /// Gets the Description this Provider
        /// </summary>
        public override string Description { get; } = "Oracle (Native)";

        /// <summary>
        /// The file name of the provider assembly, e.g. FirebirdSql.Data.FirebirdClient.dll
        /// </summary>
        public override string AssemblyFileName { get; } = "Oracle.ManagedDataAccess.dll";
        /// <summary>
        /// The class name of the DbProviderFactory, e.g. FirebirdSql.Data.FirebirdClient.FirebirdClientFactory
        /// </summary>
        public override string DbProviderFactoryTypeName { get; } = "Oracle.ManagedDataAccess.Client.OracleClientFactory";

        /// <summary>
        /// The type of the <see cref="SqlStore"/> class to use when creating <see cref="SqlStore"/> instances.
        /// </summary>
        public override Type SqlStoreClass { get; set; } = typeof(SqlStore);
        /// <summary>
        /// The server type, e.g. MsSql, SQLite, Oracle, etc.
        /// </summary>
        public override SqlServerType ServerType { get; } = SqlServerType.Oracle;
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
        public override string ConnectionStringTemplate { get; } = @"Data Source={0}; User Id={1}; Password={2};";
        /// <summary>
        /// Super user name
        /// </summary>
        public override string SuperUser { get; } = "sysdba";
        /// <summary>
        /// Super user password
        /// </summary>
        public override string SuperUserPassword { get; } = "oracle";

        /// <summary>
        /// Returns true if the database server supports transactions
        /// </summary>
        public override bool SupportsTransactions { get; } = true;
        /// <summary>
        /// Returns true if the provider can create a new database
        /// </summary>
        public override bool CanCreateDatabases { get; } = false;
        /// <summary>
        /// Returns true if the database server supports generators/sequencers
        /// </summary>
        public override bool SupportsGenerators { get; } = true;
        /// <summary>
        /// Returns true when the database server supports auto-increment integer fields.
        /// </summary>
        public override bool SupportsAutoIncFields { get; } = false;

        /// <summary>
        /// Keys used in connection string by this provider
        /// </summary>
        public override string[] ServerKeys { get; } = new string[] { "Data Source" };
        /// <summary>
        /// Keys used in connection string by this provider
        /// </summary>
        public override string[] DatabaseKeys { get; } = new string[] { };
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
        public override string PrimaryKey { get; } = "integer not null primary key";
        /// <summary>
        /// Auto-increment field, when supported, else exception.
        /// </summary>
        public override string AutoInc { get { Sys.Throw("Auto-increment fields NOT supported by Oracle, use Sequence instead."); return ""; } }
        /// <summary>
        /// The Varchar text
        /// </summary>
        public override string Varchar { get; } = "varchar2";
        /// <summary>
        /// The NVarchar text
        /// </summary>
        public override string NVarchar { get; } = "nvarchar2";
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
        public override string Blob { get; } = "blob";
        /// <summary>
        /// The TextBlob text
        /// </summary>
        public override string TextBlob { get; } = "clob"; // nclob 
        /// <summary>
        /// The NTextBlob text
        /// </summary>
        public override string NTextBlob { get; } = "nclob";
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
