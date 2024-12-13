namespace Tripous.Data
{
    /// <summary>
    /// A registry-like static class for <see cref="SqlProvider"/> instances.
    /// </summary>
    public static class SqlProviders
    {
        /* private */
        static List<SqlProvider> Providers = new List<SqlProvider>();


        /// <summary>
        /// Static constructor.
        /// <para>Registers all sql providers.</para>
        /// </summary>
        static SqlProviders()
        {
            RegisterSqlProvider(new SqlProviderSQLite());
            RegisterSqlProvider(new SqlProviderMsSql());
            RegisterSqlProvider(new SqlProviderMySql());
            RegisterSqlProvider(new SqlProviderPostgreSQL());
            RegisterSqlProvider(new SqlProviderFirebird());
            RegisterSqlProvider(new SqlProviderOracle());
        }

 

        /// <summary>
        /// Registers a <see cref="SqlProvider"/>
        /// </summary>
        static public void RegisterSqlProvider(SqlProvider Provider)
        {
            if (!ContainsSqlProvider(Provider.Name))
                Providers.Add(Provider);
        }
 
        /// <summary>
        /// Finds and returns a <see cref="SqlProvider"/> by name, if any, else null.
        /// </summary>
        static public SqlProvider FindSqlProvider(string ProviderName)
        {
            return Providers.FirstOrDefault(item => string.Compare(item.Name, ProviderName, StringComparison.InvariantCultureIgnoreCase) == 0);
        }
        /// <summary>
        /// Returns a provider specified by name, if any, else throws an exception.
        /// </summary>
        static public SqlProvider GetSqlProvider(string ProviderName)
        {
            SqlProvider Provider = FindSqlProvider(ProviderName);
            if (Provider == null)
                throw new ApplicationException($"Sql Provider not registered: {ProviderName}");
            return Provider;
        }
        /// <summary>
        /// Returns true if a <see cref="SqlProvider"/>  specified by its name, is registered.
        /// </summary>
        static public bool ContainsSqlProvider(string ProviderName)
        {
            return FindSqlProvider(ProviderName) != null;
        }
    }
}
