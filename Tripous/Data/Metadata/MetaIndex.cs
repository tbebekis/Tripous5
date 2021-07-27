/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Text;


namespace Tripous.Data.Metadata
{

    /// <summary>
    /// Represents schema information for an index in a table
    /// </summary>
    public class MetaIndex : NamedItem, IMetaNode, IMetaFullText
    {
        private string indexType;
        private string fields;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public MetaIndex()
        {
        }

        /* properties */
        /// <summary>
        /// Get the table this field belongs to
        /// </summary>
        public MetaTable Table { get { return CollectionOwner as MetaTable; } }
        /// <summary>
        /// Gets the display text for the item
        /// </summary>
        public string DisplayText
        {
            get
            {
                return Name;
            }

        }
        /// <summary>
        /// Gets the full text
        /// </summary>
        public string FullText
        {
            get
            {
                StringBuilder SB = new StringBuilder();

                SB.AppendLine(string.Format("{0}: {1}", Kind.ToString(), Name));
                if (!string.IsNullOrEmpty(IndexType))
                    SB.AppendLine(string.Format("IndexType: {0}", IndexType));
                if (!string.IsNullOrEmpty(Fields))
                    SB.AppendLine(string.Format("Fields: {0}", Fields));

                //SB.AppendLine(string.Format("IsUnique: {0}", IsUnique? "YES": "no"));
   

                return SB.ToString();

            }
        }
        /// <summary>
        /// The kind of this meta-node, i.e. Tables, Table, Columns, Column, etc
        /// </summary>
        public MetaNodeKind Kind { get { return MetaNodeKind.Index; } }
        /// <summary>
        /// A user defined value
        /// </summary>
        public object Tag { get; set; }
        /// <summary>
        /// Gets or sets the type of the index.
        /// </summary>
        public string IndexType
        {
            get { return !string.IsNullOrEmpty(indexType) ? indexType : string.Empty; }
            set { indexType = value; }
        }
        /// <summary>
        /// Gets or sets whether this is a unique index
        /// </summary>
        public bool IsUnique { get; set; }
        /// <summary>
        /// Gets or sets the field names or a primary key, unique key or check constraint
        /// of fields that belong to the owner table (a semi-colon delimited list of names).
        /// </summary>
        public string Fields
        {
            get { return !string.IsNullOrEmpty(fields) ? fields : string.Empty; }
            set { fields = value; }
        }
    }
}
