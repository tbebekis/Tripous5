namespace Tripous.Data
{

    /// <summary>
    /// Connection string settings
    /// </summary>
    public class SqlConnectionInfo
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public SqlConnectionInfo()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public SqlConnectionInfo(string Name, string Provider, string Server, string Database, string UserName, string Password)
        {
            this.Name = Name;
            this.Provider = Provider;
            SqlProvider SqlProvider = GetSqlProvider();  
            this.ConnectionString = SqlProvider.CreateConnectionString(Server, Database, UserName, Password);
        }

        /* public */
        /// <summary>
        /// Returns the <see cref="SqlProvider"/> of this connection string. If the provider is not registered with <see cref="SqlProviders"/> an exception is thrown.
        /// </summary>
        public SqlProvider GetSqlProvider()
        { 
            return SqlProviders.GetSqlProvider(Provider);
        }
        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return $"{Provider}:{Name}";
        }
        /// <summary>
        /// Returns true if this connection info is valid and can connect to a database.
        /// </summary>
        public bool CanConnect(bool ThrowIfNot = false)
        {
            return GetSqlProvider().CanConnect(ConnectionString, ThrowIfNot);
        }

        /// <summary>
        /// Returns a clone of this instance
        /// </summary>
        public SqlConnectionInfo Clone()
        {
            SqlConnectionInfo Result = new SqlConnectionInfo();
            Sys.AssignObject(this, Result);
            return Result;
        }

        /* properties */
        /// <summary>
        /// The name of the connection. The default connection should be named DEFAULT.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// The name of the provider. Valid values: MsSql, Oracle, Firebird, SQLite, MySql and PostgreSQL.
        /// </summary>
        public string Provider { get; set; }
        /// <summary>
        /// The connection string
        /// </summary>
        public string ConnectionString { get; set; }
        /// <summary>
        /// Whether to create table generators/sequences automatically. For databases that support generators/sequences such as Oracle and Firebird.
        /// </summary>
        public bool AutoCreateGenerators { get; set; } = false;
        /// <summary>
        /// The time in seconds to wait for an SELECT/INSERT/UPDATE/DELETE/CREATE TABLE ect. command to execute. Zero means the default timeout.
        /// </summary>
        public int CommandTimeoutSeconds { get; set; } = 300;
    }
}
