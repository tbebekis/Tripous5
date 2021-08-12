﻿/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Linq;

namespace Tripous.Data
{
    /// <summary>
    /// Represents a version of a database schema.
    /// </summary>
    public class SchemaVersion  
    {
 
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public SchemaVersion()
        { 
        }

        /* public */
        /// <summary>
        /// Adds a table
        /// </summary>
        public void AddTable(string SqlText)
        {
            if (!string.IsNullOrEmpty(SqlText)) // because often we leave empty declarations in schema registration functions
            {
                SchemaItem Item = new SchemaItem();
                Item.Name = Sql.ExtractTableName(SqlText);
                Item.SqlText = SqlText;
                Tables.Add(Item);
            }

        }
        /// <summary>
        /// Adds a view
        /// </summary>
        public void AddView(string Name, string SqlText)
        {
            SchemaItem Item = new SchemaItem();
            Item.Name = Name;
            Item.SqlText = SqlText;
            Views.Add(Item);
        }
        /// <summary>
        /// Adds a statement
        /// </summary>
        public void AddStatementBefore(string SqlText)
        {
            StatementsBefore.Add(SqlText);
        }
        /// <summary>
        /// Adds a statement
        /// </summary>
        public void AddStatementAfter(string SqlText)
        {
            StatementsAfter.Add(SqlText);
        }

        /// <summary>
        /// Override
        /// </summary>
        public override string ToString()
        {
            return $"{Version}";
        }

        /* properties */
        /// <summary>
        /// Gets or sets the version of the schema.
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// A list of table items.
        /// </summary>
        public List<SchemaItem> Tables { get; set; } = new List<SchemaItem>();
        /// <summary>
        /// A list of view items.
        /// </summary>
        public List<SchemaItem> Views { get; set; } = new List<SchemaItem>();
        /// <summary>
        /// A list of Sql statements to be executed before this schema execution
        /// </summary>
        public List<string> StatementsBefore { get; set; } = new List<string>();
        /// <summary>
        /// A list of Sql statements to be executed after this schema execution
        /// </summary>
        public List<string> StatementsAfter { get; set; } = new List<string>();
    }
}
