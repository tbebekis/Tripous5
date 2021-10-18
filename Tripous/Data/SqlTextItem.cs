using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data
{
    /// <summary>
    /// Represents the text of an Sql statement
    /// </summary>
    public class SqlTextItem
    {
        static int Counter = 0;


        /// <summary>
        /// Constructor
        /// </summary>
        public SqlTextItem()
        {
        }
        /// <summary>
        /// Constructor
        /// </summary>
        public SqlTextItem(string SqlText, string ConnectionName = null, string Name = null)
        {
            this.SqlText = SqlText;
            this.ConnectionName = !string.IsNullOrWhiteSpace(ConnectionName) ? ConnectionName : Sys.DEFAULT;
            this.Name = !string.IsNullOrWhiteSpace(Name) ? Name : "Sql" + Counter++.ToString();
        }


        /* properties */
        /// <summary>
        /// The name of the statement
        /// </summary>
        public string Name { get; set; } = "Sql" + Counter++.ToString();
        /// <summary>
        /// The statement's text
        /// </summary>
        public string SqlText { get; set; }
        /// <summary>
        /// The connection name
        /// </summary>
        public string ConnectionName { get; set; } = Sys.DEFAULT;

    }
}
