/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Tripous.Data
{

    /// <summary>
    /// The holder of all schema versions of a certain Schema.
    /// <para>A <see cref="Schema"/> has a unique domain name and a connection name. </para>
    /// <para>The <see cref="Schema.Domain"/> could be System, Application, or the unique identifier name of an external plugin. </para>
    /// <para>The <see cref="Schema.ConnectionName"/> connection name, used in creating the relevant <see cref="SqlStore"/> instance that executes the schema.</para>
    /// <para>A <see cref="Schema"/> has also a list of <see cref="SchemaVersion"/> items. </para>
    /// <para>A <see cref="SchemaVersion"/> item has an integer version,  
    /// and collections of <see cref="SchemaItem"/> items for tables and views,
    /// along with collections of statements to be executed before and after the schema version is executed.</para>
    /// </summary>
    public class Schema  
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public Schema()
        {
        }

        /* public */
        /// <summary>
        /// Finds or adds, if not exists, a <see cref="SchemaVersion"/>
        /// </summary>
        public SchemaVersion FindOrAdd(int Version)
        {
            SchemaVersion Result = Versions.FirstOrDefault(item => (Version == item.Version));
            if (Result == null)
                Result = Add(Version);

            return Result;
        }
        /// <summary>
        /// Returns the a schema version item with a specified version, if any else null
        /// </summary>
        public SchemaVersion Find(int Version)
        {
            return Versions.FirstOrDefault(item => (Version == item.Version));
        }
        /// <summary>
        /// True if a schema version item with a specified version is already registered.
        /// </summary>
        public bool Contains(int Version)
        {
            return Find(Version) != null;
        }

        /// <summary>
        /// Returns the first schema version items w having a version equal to or greater than the specified Version, if any, else null.
        /// </summary>
        public SchemaVersion FindGreaterOrEqual(int Version)
        {
            return Versions.FirstOrDefault(item => (item.Version >= Version));
        }
        /// <summary>
        /// Checks to see if there is already a schema version with greater or equal Version. 
        /// <para>Throws an exception if it finds one.</para>
        /// </summary>
        public void Check(int Version)
        {
            SchemaVersion Item = FindGreaterOrEqual(Version);
            if (Item != null)
                Sys.Throw($"Invalid database schema version: Domain: {Domain}, ConnectionName {ConnectionName}, Version {Version}");
        }

        /// <summary>
        /// Adds a new schema version
        /// </summary>
        public SchemaVersion Add(int Version)
        {
            Check(Version);
            SchemaVersion Result = new SchemaVersion();
            Result.Version = Version;
            Versions.Add(Result);
            return Result;
        }

        /* execution methods */
        /// <summary>
        /// Executes the versions of a schema against a database.
        /// </summary>
        /// <param name="Connection">The database connection to execute against.</param>
        /// <param name="VersionList">VersionList is a by Version sorted list of schemas for that database</param>
        void ExecuteSchema(SqlConnectionInfo Connection, List<SchemaVersion> VersionList)
        {
            /* get the current version of the database  from the dbIni table */
            DbIni Ini = Db.MainIni;
            string Entry = string.Format("Database.Version.{0}.{1}", Connection.Name, Domain);
            int Version = Ini.ReadInteger(Entry, -1);


            foreach (SchemaVersion Item in VersionList)
            {
                if (Item.Version > Version)
                {
                    SchemaExecutor.Execute(Connection, Item);
                    Db.Metastores.Find(Connection.Name).ReLoad();

                    /* write the version to the dbIni */
                    Ini.WriteInteger(Entry, Item.Version);  
                }
            }
        }
        /// <summary>
        /// Executes all schemas.
        /// </summary>
        internal void Execute()
        { 
            List<SqlConnectionInfo> Connections = new List<SqlConnectionInfo>(Db.Connections);

            SqlConnectionInfo Connection = Db.Connections.FirstOrDefault(item => item.Name.IsSameText(ConnectionName));
            List<SchemaVersion> VersionList = Versions.OrderBy(item => item.Version).ToList();

            ExecuteSchema(Connection, VersionList);
        }


        /* property */
        /// <summary>
        /// Domain is the name of the register (client code) that has registered the schema.
        /// <para>It could be System, Application, or the unique identifier name of an external plugin.</para>
        /// </summary>
        public string Domain { get; set; }
        /// <summary>
        /// The name of the database connection this schema is applied against.
        /// </summary>
        public string ConnectionName { get; set; }
        /// <summary>
        /// The registered versions
        /// </summary>
        public List<SchemaVersion> Versions { get; set; } = new List<SchemaVersion>();
    }
}
