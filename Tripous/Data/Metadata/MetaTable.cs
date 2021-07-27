/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
 

namespace Tripous.Data.Metadata
{
    /// <summary>
    /// Represents schema information for a  table
    /// </summary>
    public class MetaTable : NamedItem, IMetaNodeListParent, IMetaNode, IMetaFullText
    {
        /* private */
        private MetaFields fields;
        private MetaConstraints constraints;
        private MetaIndexes indexes;
        private MetaTriggers triggers;
        private string schemaOwner;
 

        private DataTable _tblColumns;
        private DataTable _tblPrimaryKeys;
        private DataTable _tblForeignKeys;
        private DataTable _tblForeignKeyColumns;
        private DataTable _tblIndexes;
        private DataTable _tblIndexColumns;
        private DataTable _tblUniqueKeys;
        private DataTable _tblConstraints;
        private DataTable _tblTriggers;


        // Keys  Constraints Triggers    
        // Indexes      DatabaseIndex IndexConverter
        // Keys         ForeignKeyColumnConverter 

        /// <summary>
        /// Override
        /// </summary>
        protected override void DoClear()
        {
            base.DoClear();
            
            fields.Clear();
            constraints.Clear();
            indexes.Clear();
            triggers.Clear();

            _tblColumns = null;
            _tblPrimaryKeys = null;
            _tblForeignKeys = null;
            _tblForeignKeyColumns = null;
            _tblIndexes = null;
            _tblIndexColumns = null;
            _tblUniqueKeys = null;
            _tblConstraints = null;
            _tblTriggers = null;
        }

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public MetaTable()
        {
            fields = new MetaFields();
            fields.Owner = this;

            constraints = new MetaConstraints();
            constraints.Owner = this;

            indexes = new MetaIndexes();
            indexes.Owner = this;

            triggers = new MetaTriggers();
            triggers.Owner = this;
        }

        /* properties */
        /// <summary>
        /// Gets the owner metastore
        /// </summary>
        public Metastore Store { get { return CollectionOwner as Metastore; } }
        /// <summary>
        /// Gets the text this instance provides for display purposes
        /// </summary>
        public string DisplayText { get { return Name; } }
        /// <summary>
        /// Gets the full text
        /// </summary>
        public string FullText
        {
            get
            {

                StringBuilder SB = new StringBuilder();
                SB.AppendLine(string.Format("{0}: {1}", Kind.ToString(), Name));
                SB.AppendLine("-----------------------------------");

                foreach (MetaField F in this.Fields)
                    SB.AppendLine(F.DisplayText);

                return SB.ToString();

            }
        }
        /// <summary>
        /// The kind of this meta-node, i.e. Tables, Table, Columns, Column, etc
        /// </summary>
        public MetaNodeKind Kind { get { return MetaNodeKind.Table; } }
        /// <summary>
        /// A user defined value
        /// </summary>
        public object Tag { get; set; }
        /// <summary>
        /// Gets the collection of fields
        /// </summary>
        public MetaFields Fields
        {
            get
            {
                //fields.Load();
                return fields;
            }
        }
        /// <summary>
        /// Gets the collection of constraints
        /// </summary>
        public MetaConstraints Constraints
        {
            get
            {
                //constraints.Load();
                return constraints;
            }
        }

        /// <summary>
        /// Gets the collection of indexes
        /// </summary>
        public MetaIndexes Indexes
        {
            get
            {
                //indexes.Load();
                return indexes;
            }
        }
        /// <summary>
        /// Gets the collection of triggers
        /// </summary>
        public MetaTriggers Triggers
        {
            get
            {
                //triggers.Load();
                return triggers;
            }
        }
        /// <summary>
        /// Gets the lists
        /// </summary>
        public IMetaNodeList[] Lists { get { return new IMetaNodeList[] { Fields, Constraints, Indexes, Triggers }; } }

        /// <summary>
        /// Gets or sets the SchemaOwner
        /// </summary>
        public string SchemaOwner
        {
            get { return !string.IsNullOrEmpty(schemaOwner) ? schemaOwner : string.Empty; }
            set { schemaOwner = value; }
        }

        /* collection tables */
        /// <summary>
        /// Gets a DataTable with the relevant collection information
        /// </summary>
        public DataTable tblColumns
        {
            get
            {
                if (_tblColumns == null)
                    _tblColumns = Store.GetTableCollection(Name, "Columns");
                return _tblColumns;
            }
        }
        /// <summary>
        /// Gets a DataTable with the relevant collection information
        /// </summary>
        public DataTable tblPrimaryKeys
        {
            get
            {
                if (_tblPrimaryKeys == null)
                    _tblPrimaryKeys = Store.GetTableCollection(Name, "PrimaryKeys");
                return _tblPrimaryKeys;
            }
        }
        /// <summary>
        /// Gets a DataTable with the relevant collection information
        /// </summary>
        public DataTable tblForeignKeys
        {
            get
            {
                if (_tblForeignKeys == null)
                {
                    if (Store.IsCollectionSupported("ForeignKeys"))
                        _tblForeignKeys = Store.GetTableCollection(Name, "ForeignKeys");
                    else if (Store.IsCollectionSupported("Foreign Keys"))
                        _tblForeignKeys = Store.GetTableCollection(Name, "Foreign Keys");
                    else
                        _tblForeignKeys = new DataTable();
                }

                return _tblForeignKeys;
            }
        }
        /// <summary>
        /// Gets a DataTable with the relevant collection information
        /// </summary>
        public DataTable tblForeignKeyColumns
        {
            get
            {
                if (_tblForeignKeyColumns == null)
                    _tblForeignKeyColumns = Store.GetTableCollection(Name, "ForeignKeyColumns");
                return _tblForeignKeyColumns;
            }
        }
        /// <summary>
        /// Gets a DataTable with the relevant collection information
        /// </summary>
        public DataTable tblUniqueKeys
        {
            get
            {
                if (_tblUniqueKeys == null)
                    _tblUniqueKeys = Store.GetTableCollection(Name, "UniqueKeys");
                return _tblUniqueKeys;
            }
        }
        /// <summary>
        /// Gets a DataTable with the relevant collection information
        /// </summary>
        public DataTable tblConstraints
        {
            get
            {
                if (_tblConstraints == null)
                    _tblConstraints = Store.GetTableCollection(Name, "CheckConstraints");
                return _tblConstraints;
            }
        }
        /// <summary>
        /// Gets a DataTable with the relevant collection information
        /// </summary>
        public DataTable tblIndexes
        {
            get
            {
                if (_tblIndexes == null)
                    _tblIndexes = Store.GetTableCollection(Name, "Indexes");
                return _tblIndexes;
            }
        }
        /// <summary>
        /// Gets a DataTable with the relevant collection information
        /// </summary>
        public DataTable tblIndexColumns
        {
            get
            {
                if (_tblIndexColumns == null)
                    _tblIndexColumns = Store.GetTableCollection(Name, "IndexColumns");
                return _tblIndexColumns;
            }
        }
        /// <summary>
        /// Gets a DataTable with the relevant collection information
        /// </summary>
        public DataTable tblTriggers
        {
            get
            {
                if (_tblTriggers == null)
                    _tblTriggers = Store.GetTableCollection(Name, "Triggers");
                return _tblTriggers;
            }
        }

    }
}
