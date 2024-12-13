namespace Tripous.Data
{
    /// <summary>
    /// Indicates the type of the database server/data a connection uses
    /// </summary>
    public enum SqlServerType
    {
        /// <summary>
        /// Unknown server
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// SQLite database
        /// </summary>
        SQLite = 1,
        /// <summary>
        /// MS S server
        /// </summary>
        MsSql = 2,        
        /// <summary>
        /// Oracle server
        /// </summary>
        Oracle = 3,
        /// <summary>
        /// Firebird server
        /// </summary>
        Firebird = 4,
        /// <summary>
        /// PostgreSQL server
        /// </summary>
        PostgreSQL = 5,
        /// <summary>
        /// MySql
        /// </summary>
        MySql = 6,
        /// <summary>
        /// MS Access database
        /// </summary>
        Access = 7,
        /// <summary>
        /// MS Excel
        /// </summary>
        Excel = 8,
        /// <summary>
        /// dBase files
        /// </summary>
        Dbf = 9,
        
       

    }
}
