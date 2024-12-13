namespace Tripous.Data
{
    /// <summary>
    /// Helper
    /// </summary>
    static public class SqlStores
    {

        static SqlStore fDefault;

        /* create sql stores */
        /// <summary>
        /// Returns the <see cref="SqlStore"/> for the Default database connection
        /// </summary>
        static public SqlStore CreateDefaultSqlStore()
        {
            return CreateSqlStore(Db.GetDefaultConnectionInfo());
        }
        /// <summary>
        /// Creates and returns a <see cref="SqlStore"/>
        /// </summary>
        static public SqlStore CreateSqlStore(SqlConnectionInfo ConnectionInfo)
        {
            SqlProvider Provider = ConnectionInfo.GetSqlProvider();
            SqlStore Result = Activator.CreateInstance(Provider.SqlStoreClass, new object[] { ConnectionInfo }) as SqlStore;
            return Result;
        }
        /// <summary>
        /// Creates and returns a <see cref="SqlStore"/>
        /// </summary>
        static public SqlStore CreateSqlStore(string ConnectionName)
        {
            return CreateSqlStore(Db.GetConnectionInfo(ConnectionName));
        }


        /// <summary>
        /// Returns the default <see cref="SqlStore"/>.
        /// </summary>
        static public SqlStore Default
        {
            get
            {
                if (fDefault == null)
                    fDefault = CreateDefaultSqlStore();
                return fDefault;
            }
        }

    }
}
