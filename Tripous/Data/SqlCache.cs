using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace Tripous.Data
{
    /// <summary>
    /// A cache of <see cref="DataTable"/> instances, containing schema of a named sql statement, per connection.
    /// </summary>
    static public class SqlCache
    {
        static object syncLock = new LockObject();
        static Dictionary<string, Dictionary<string, DataTable>> Cache = new Dictionary<string, Dictionary<string, DataTable>>();

        /// <summary>
        /// Returns true if there is a <see cref="DataTable"/> with schema information of a Sql statement specified by a name, under a connection specified by name. 
        /// </summary>
        static public bool Contains(string ConnectionName, string StatementName)
        {
            return Find(ConnectionName, StatementName) != null;
        }
        /// <summary>
        /// Finds and returns a schema <see cref="DataTable"/>  if any, else null.
        /// </summary>
        static public DataTable Find(string ConnectionName, string StatementName)
        {
            if (Cache.ContainsKey(ConnectionName))
            {
                Dictionary<string, DataTable> ConnectionStatements = Cache[ConnectionName];

                if (ConnectionStatements.ContainsKey(StatementName))
                    return ConnectionStatements[StatementName];
            }

            return null;
        }
        /// <summary>
        /// Adds a schema <see cref="DataTable"/> to the cache.
        /// </summary>
        static public void Add(string ConnectionName, string StatementName, DataTable SchemaTable)
        {
            Dictionary<string, DataTable> ConnectionStatements = null;

            if (!Cache.ContainsKey(ConnectionName))
            {
                ConnectionStatements = new Dictionary<string, DataTable>();
                Cache[ConnectionName] = ConnectionStatements;
            }
            else
            {
                ConnectionStatements = Cache[ConnectionName];
            } 

            ConnectionStatements[StatementName] = SchemaTable;
        }
    }
}
