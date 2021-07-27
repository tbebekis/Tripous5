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
    /// Represents a versioned database schema under a domain.
    /// <para>Domain is the name of the register that has registered this version.
    /// It could be System or the unique identifier name of an external plugin.</para>
    /// </summary>
    public class SchemaVersion : Assignable
    {
        string domain;
        string fConnectionName;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public SchemaVersion()
        {
            Tables = new NamedItems<SchemaItem>();
            Views = new NamedItems<SchemaItem>();

            StatementsBefore = new List<string>();
            StatementsAfter = new List<string>();

            Tables.Owner = this;
            Views.Owner = this;
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
            return string.Format("{0}.{1}.{2}", Domain, ConnectionName, Version);
        }

        /* properties */
        /// <summary>
        /// Domain is the name of the register (client code) that has registered this version.
        /// It could be System or the unique identifier name of an external plugin.
        /// </summary>
        public string Domain
        {
            get { return domain ?? string.Empty; }
            set { domain = value; }
        }
        /// <summary>
        /// The name of the database connection this version is applied against.
        /// </summary>
        public string ConnectionName
        {
            get { return fConnectionName ?? string.Empty; }
            set { fConnectionName = value; }
        }
        /// <summary>
        /// Gets or sets the version of the schema.
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// A list of table items.
        /// </summary>
        public NamedItems<SchemaItem> Tables { get; private set; }
        /// <summary>
        /// A list of view items.
        /// </summary>
        public NamedItems<SchemaItem> Views { get; private set; }
        /// <summary>
        /// A list of Sql statements to be executed before this schema execution
        /// </summary>
        public List<string> StatementsBefore { get; private set; }
        /// <summary>
        /// A list of Sql statements to be executed after this schema execution
        /// </summary>
        public List<string> StatementsAfter { get; private set; }
    }
}
