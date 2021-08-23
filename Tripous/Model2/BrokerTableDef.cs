using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Model2
{

    /// <summary>
    /// A broker table definition
    /// </summary>
    public class BrokerTableDef
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public BrokerTableDef()
        {

        }

        /* public */
        /// <summary>
        /// Returns a string representation of this instance.
        /// </summary>
        public override string ToString()
        {
            return Name;
        }

        /* properties */
        /// <summary>
        /// The table name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// An alias of this table
        /// </summary>
        public string Alias { get; set; }

        /// <summary>
        /// Gets or sets the name of the primary key field of this table.
        /// </summary>
        public string PrimaryKeyField { get; set; } = "Id";
        /// <summary>
        /// Gets or sets the field name of a field belonging to a master table.
        /// <para>Used when this table is a detail table in a master-detail relation or when this is a join table.</para>
        /// </summary>
        public string MasterKeyField { get; set; } = "Id";

        /// <summary>
        /// The fields of this table
        /// </summary>
        public List<BrokerFieldDef> Fields { get; set; } = new List<BrokerFieldDef>();
        /// <summary>
        /// The list of join tables. 
        /// </summary>
        public List<BrokerTableDef> JoinTables { get; set; } = new List<BrokerTableDef>();
        /// <summary>
        /// The main table of a Broker (Item) is selected as 
        /// <para>  <c>select * from TABLE_NAME where ID = :ID</c></para>
        /// <para>
        /// If the table contains foreign keys, for instance CUSTOMER_ID etc, then those foreign tables are NOT joined. 
        /// The programmer who designs the UI just creates a Locator where needed.
        /// </para>
        /// <para>
        /// But there is always the need to have data from those foreign tables in many situations, i.e. in reports.
        /// </para>
        /// <para>
        /// StockTables are used for that. They are selected each time after the select of the main broker table (Item)          
        /// </para>
        /// </summary>
        public List<BrokerQueryDef> StockTables { get; set; } = new List<BrokerQueryDef>();

    }

}
