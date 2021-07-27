using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Reflection;

namespace Tripous
{
    /// <summary>
    /// DbConnectionsExtensions
    /// </summary>
    static public class DbConnectionsExtensions
    {
        static int TableCounter = 0;

        /// <summary>
        /// Returns the DbProviderFactory of a DbConnection.
        /// </summary>
        static public DbProviderFactory GetFactory(this DbConnection Con)
        {
            if (Con != null)
            {
                return Sys.GetProperty(Con, "DbProviderFactory") as DbProviderFactory;
            }

            return null;
        }
        /// <summary>
        /// Creates and returns a DbDataAdapter
        /// </summary>
        static public DbDataAdapter CreateAdapter(this DbConnection Con)
        {
            DbProviderFactory Factory = Con.GetFactory();
            if (Factory != null)
                return Factory.CreateDataAdapter();
            return null;
        }

        /// <summary>
        /// Executes SqlText an returns a DataTable.
        /// <para>WARNING: SqlText shoud be a SELECT statement</para>
        /// </summary>
        static public DataTable Select(this DbConnection Con, string SqlText, string Sort = "")
        {

            DataTable Result = null;

            if (Con.State == ConnectionState.Closed)
                Con.Open();

            using (DbCommand Cmd = Con.CreateCommand())
            {
                using (DbDataAdapter Adapter = Con.CreateAdapter())
                {
                    Adapter.SelectCommand = Cmd;
                    Cmd.CommandText = SqlText;
                    TableCounter++;
                    Result = new DataTable("Table" + TableCounter.ToString());
                    Adapter.Fill(Result);
                }
            }

            if (!string.IsNullOrWhiteSpace(Sort))
                Result.DefaultView.Sort = Sort;

            return Result;
        }
        /// <summary>
        /// Executes SqlText
        /// <para>SqlText should be an INSERT, UPDATE, DELETE, CREATE TABLE, ALTER TABLE etc. statement</para>
        /// </summary>
        static public void ExecSql(this DbConnection Con, string SqlText)
        {
            if (Con.State == ConnectionState.Closed)
                Con.Open();

            using (DbCommand Cmd = Con.CreateCommand())
            {
                Cmd.CommandText = SqlText;
                Cmd.ExecuteNonQuery();
            }
        }
        /// <summary>
        /// Executes SqlText and returns the first DataRow of the result set.
        /// <para>WARNING: If SqlText returns no rows at all then this method returns null.</para>
        /// </summary>
        static public DataRow SelectResults(this DbConnection Con, string SqlText)
        {
            DataTable Table = Con.Select(SqlText);
            if (Table.Rows.Count > 0)
                return Table.Rows[0];
            else
                return null;
        }
        /// <summary>
        /// Ideal for executing SELECT statements of the type "select FIELD_NAME from TABLE_NAME"
        /// </summary>
        static public object SelectResult(this DbConnection Con, string SqlText, object Default)
        {
            object Result = Default;

            DataRow Row = Con.SelectResults(SqlText);
            if ((Row != null) && !Row.IsNull(0))
            {
                Result = Row[0];
            }

            return Result;
        }
        /// <summary>
        /// Ideal for executing SELECT statements of the type "select FIELD_NAME from TABLE_NAME"
        /// </summary>
        static public object SelectResult(this DbConnection Con, string SqlText)
        {
            return Con.SelectResult(SqlText, DBNull.Value);
        }
        /// <summary>
        /// Ideal for executing SELECT statements of the type "select count(ID) as COUNT_ID from TABLE_NAME where ID = 1234"
        /// </summary>
        static public int IntegerResult(this DbConnection Con, string SqlText, int Default = -1)
        {
            string S = Con.SelectResult(SqlText, Default).ToString();
            int Result = 0;
            if (int.TryParse(S, out Result))
                return Result;
            return Default;
        }
        /// <summary>
        /// Executes the specified SELECT statement, collects the first field of each row (by field index),
        /// and returns the collected list as an array of integers.
        /// </summary>
        static public int[] SelectIds(this DbConnection Con, string SqlText)
        {
            DataTable Table = Con.Select(SqlText);
            List<int> List = new List<int>();
            foreach (DataRow Row in Table.Rows)
                List.Add(Row.AsInteger(0));
            return List.ToArray();
        }
    }
}
