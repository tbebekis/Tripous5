/*--------------------------------------------------------------------------------------        
                           Copyright © 2016 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;
 
using Tripous.Data;

namespace Tripous.Model
{
    /// <summary>
    /// JsonBroker
    /// </summary>
    public class JsonBroker
    {
 

        /* construction */
        /// <summary>
        /// Constructor.
        /// </summary>
        public JsonBroker()
        {
        }
        /// <summary>
        /// Constructor.
        /// </summary>
        public JsonBroker(Broker Source)
        {
            BrokerDescriptor Descriptor = Source is SqlBroker ? (Source as SqlBroker).Descriptor : null;

            this.Name = Descriptor != null ? Descriptor.Name : string.Empty;
            this.IsListBroker = Source.IsListBroker;
            this.IsMasterBroker = Source.IsMasterBroker;
            this.State = Source.State;
            this.ConnectionName = Descriptor != null? Descriptor.ConnectionName : string.Empty;
            this.MainTableName = Source.MainTableName;
            this.LinesTableName = Source.LinesTableName;
            this.SubLinesTableName = Source.SubLinesTableName;
            this.EntityId = Source.EntityId;
            this.EntityName = Source.EntityName;
            this.PrimaryKeyField = Descriptor != null ? Descriptor.PrimaryKeyField : "Id";
            this.GuidOids = Descriptor != null ? Descriptor.GuidOids : false;

            // QueryDescriptor names
            if (Descriptor != null)
            {
                foreach (QueryDescriptor QD in Descriptor.Queries)
                    QueryNames.Add(QD.Name);

                SelectList.AddRange(Descriptor.GetMergedSelectSqlList());
            }

            // tables
            TableDescriptor TableDescriptor;
            JsonDataTable Table;
            foreach (MemTable SourceTable in Source.Tables)
            {
                TableDescriptor = Descriptor != null ? Descriptor.FindTableDescriptor(SourceTable.TableName) : null;
                Table = new JsonDataTable(SourceTable, TableDescriptor);
                Tables.Add(Table);
            }
        }

        /* properties */
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// True if this is a list broker
        /// </summary>
        public bool IsListBroker { get; set; }
        /// <summary>
        /// True if this is a master broker.
        /// </summary>
        public bool IsMasterBroker { get; set; }
        /// <summary>
        /// Returns the "data State" of the broker. It could be Insert, Edit or None.
        /// <para>The State remains Insert or Edit after the Insert() or Edit() is called. 
        /// A call to Commit() sets the State to Edit. </para>
        /// </summary>
        public DataMode State { get; set; }
        /// <summary>
        /// Gets or sets the name of the connection (database)
        /// </summary>
        public string ConnectionName { get; set; } 
        /// <summary>
        /// Returns the table name of the main table
        /// </summary>
        public string MainTableName { get; set; }
        /// <summary>
        /// Returns the table name of the Lines table
        /// </summary>
        public string LinesTableName { get; set; }
        /// <summary>
        /// Returns the table name of the SubLines table
        /// </summary>
        public string SubLinesTableName { get; set; }
        /// <summary>
        /// An integer Id from the SYS_ENTITY table 
        /// <para>It may points to an application Entity (for example Customer, Order, Employee, etc)</para>
        /// <para>Defaults to 0, meaning no entity Id.</para>
        /// <para>NOTE: EntityId is used by forms in order to call SysAction and Document services. No EntityId, no such services.</para>
        /// </summary>
        public int EntityId { get; set; }
        /// <summary>
        /// The name of the Entity this broker represents
        /// </summary>
        public string EntityName { get; set; }
        /// <summary>
        /// Returns the primary key field name of the table
        /// </summary>
        public string PrimaryKeyField { get; set; }
        /// <summary>
        /// When is true indicates that the OID is a Guid string.  
        /// </summary>
        public bool GuidOids { get; set; }
        /// <summary>
        /// A list of QueryDescriptor names
        /// </summary>
        public List<string> QueryNames { get; set; } = new List<string>();
        /// <summary>
        /// Tables
        /// </summary>
        public List<JsonDataTable> Tables { get; set; } = new List<JsonDataTable>();
        /// <summary>
        /// Tables
        /// </summary>
        public List<SelectSql> SelectList { get; set; } = new List<SelectSql>();

    }
}
