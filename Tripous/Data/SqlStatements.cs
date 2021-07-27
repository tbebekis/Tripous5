/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;

namespace Tripous.Data
{

    /// <summary>
    /// Represents the full group of DML S statements of a module
    /// </summary>
    public class SqlStatements : Assignable
    {


        /// <summary>
        /// Initializes the properties of this instance
        /// </summary>
        protected override void DoClear()
        {
            base.DoClear();

            BrowseSelect.Clear();
            RowSelect = "";
            Insert = "";
            Update = "";
            Delete = "";
        }


        /// <summary>
        /// Constructor
        /// </summary>
        public SqlStatements()
        {
        }


        /// <summary>
        /// Gets the SELECT statement of a Browse part of a module
        /// </summary>
        public SelectSql BrowseSelect { get; private set; } = new SelectSql();
        /// <summary>
        /// Gets or sets the SELECT statement of an Edit part of a module. i.e "select * from TABLE_NAME where ID = SomeValue"
        /// </summary>
        public string RowSelect { get; set; } = "";
        /// <summary>
        /// Gets or sets the INSERT statement of an item in a module
        /// </summary>
        public string Insert { get; set; } = "";
        /// <summary>
        /// Gets or sets the UPDATE statement of an item in a module
        /// </summary>
        public string Update { get; set; } = "";
        /// <summary>
        /// Gets or sets the DELETE statement of an item in a module
        /// </summary>
        public string Delete { get; set; } = "";
    }


}
