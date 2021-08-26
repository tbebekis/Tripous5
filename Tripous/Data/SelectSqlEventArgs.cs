/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System; 

 

namespace Tripous.Data
{
    /// <summary>
    /// EventArgs for executing a  SelectSql
    /// </summary>
    public class SelectSqlEventArgs : EventArgs
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public SelectSqlEventArgs(SelectSql SelectSql)
        {
            this.SelectSql = SelectSql;
        }

        /* properties */
        /// <summary>
        /// The SELECT statement the Browser is going to execute.
        /// </summary>
        public SelectSql SelectSql { get; private set; }
        /// <summary>
        /// Client code may set this property to true in order to cancel the SELECT execution
        /// </summary>
        public bool Cancel { get; set; }

    }
}
