using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data
{
    /// <summary>
    /// Represents the full group of DML statements 
    /// </summary>
    public class TableSqls
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public TableSqls()
        {

        }


        /* public */
        /// <summary>
        /// Empties all statements
        /// </summary>
        public void Clear()
        {
            SelectSql = "";
            DeleteSql = "";

            SelectByMasterIdSql = "";

            SelectRowSql = "";
            InsertRowSql = "";
            UpdateRowSql = "";
            DeleteRowSql = "";
        }

        /// <summary>
        /// SELECT statement, e.g. select * from TABLE_NAME
        /// </summary>
        public string SelectSql { get; set; }
        /// <summary>
        /// DELETE statement, e.g. delete from TABLE_NAME
        /// </summary>
        public string DeleteSql { get; set; }

        /// <summary>
        /// SELECT statement for a detail table, e.g. select * from TABLE_NAME  where FOREIGN_KEY_FIELD = :SomeValue
        /// </summary>
        public string SelectByMasterIdSql { get; set; }

        /// <summary>
        /// SELECT a row statement, e.g. select * from TABLE_NAME where Id = :Id
        /// </summary>
        public string SelectRowSql { get; set; }
        /// <summary>
        /// INSERT a row statement, e.g. insert into  TABLE_NAME (FIELD_0, FIELD_N, ...) values (:FIELD_0, :FIELD_N, ...)
        /// </summary>
        public string InsertRowSql { get; set; }
        /// <summary>
        /// UPDATE a row statement, e.g. update TABLE_NAME set FIELD_= :FIELD_0, FIELD_N = :FIELD_N where Id = :Id
        /// </summary>
        public string UpdateRowSql { get; set; }
        /// <summary>
        /// UPDATE a row statement, e.g. delete from TABLE_NAME where Id = :Id
        /// </summary>
        public string DeleteRowSql { get; set; }
    }
}
