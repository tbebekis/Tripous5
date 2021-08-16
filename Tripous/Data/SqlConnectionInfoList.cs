using System;
using System.Collections.Generic;
using System.Text;


namespace Tripous.Data
{
 
    /// <summary>
    /// Connection settings. For serializing connection settings to a settings file.
    /// </summary>
    public class SqlConnectionInfoList
    {

        /// <summary>
        /// Constructor.
        /// <para>NOTE: It loads connections, if the connections file exists.</para>
        /// </summary>
        public SqlConnectionInfoList()
        {
            Load(); // it'll load connections, if the file exists
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        public SqlConnectionInfoList(List<SqlConnectionInfo> SourceSqlConnections)
        {
            this.SqlConnections = SourceSqlConnections;
        }

        /// <summary>
        /// Load. 
        /// <para>If no file path is specified the default file path for Sql Connection settings is used, see <see cref="SysConfig.SqlConnectionsFilePath"/>.</para>
        /// </summary>
        public void Load(string FilePath = "")
        {
            if (string.IsNullOrWhiteSpace(FilePath))
                FilePath = SysConfig.SqlConnectionsFilePath;

            Json.LoadFromFile(this, FilePath);
        }
        /// <summary>
        /// Save
        /// <para>If no file path is specified the default file path for Sql Connection settings is used, see <see cref="SysConfig.SqlConnectionsFilePath"/>.</para>
        /// </summary>
        public void Save(string FilePath = "")
        {
            if (string.IsNullOrWhiteSpace(FilePath))
                FilePath = SysConfig.SqlConnectionsFilePath;

            Json.SaveToFile(this, FilePath);
        }

        /// <summary>
        /// A list of database connections
        /// </summary>
        public List<SqlConnectionInfo> SqlConnections { get; set; } = new List<SqlConnectionInfo>();
    }

}
