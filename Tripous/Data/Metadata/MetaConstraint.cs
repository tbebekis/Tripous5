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
    /// Represents a constraint, such as PrimaryKey, ForeignKey, UniqueyKey and Check constraint
    /// </summary>
    public class MetaConstraint : NamedItem, IMetaNode, IMetaFullText
    {
        private string foreignTable;
        private string fields;
        private string foreignFields;
        private string expression;
        private string deleteRule;
        private string updateRule;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public MetaConstraint()
        {
        }


        /* properties */
        /// <summary>
        /// Get the table this field belongs to
        /// </summary>
        public MetaTable Table { get { return CollectionOwner as MetaTable; } }
        /// <summary>
        /// Gets the display text for the field, i.e. FIELD_NAME datatype(size), null
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

                SB.AppendLine(string.Format("{0}: {1}", ConstraintType.ToString(), Name));
                SB.AppendLine(ConstraintType.ToString());

                if (!string.IsNullOrEmpty(Fields))
                    SB.AppendLine(string.Format("Fields: {0}", Fields));

                if (!string.IsNullOrEmpty(ForeignTable))
                    SB.AppendLine(string.Format("ForeignTable: {0}", ForeignTable));

                if (!string.IsNullOrEmpty(ForeignFields))
                    SB.AppendLine(string.Format("ForeignFields: {0}", ForeignFields));

                if (!string.IsNullOrEmpty(DeleteRule))
                    SB.AppendLine(string.Format("DeleteRule: {0}", DeleteRule));

                if (!string.IsNullOrEmpty(UpdateRule))
                    SB.AppendLine(string.Format("UpdateRule: {0}", UpdateRule));

                if (!string.IsNullOrEmpty(Expression))
                    SB.AppendLine(string.Format("Expression: {0}", Expression));

 

                return SB.ToString();

            }
        }
        /// <summary>
        /// The kind of this meta-node, i.e. Tables, Table, Columns, Column, etc
        /// </summary>
        public MetaNodeKind Kind { get { return MetaNodeKind.Constraint; } }
        /// <summary>
        /// A user defined value
        /// </summary>
        public object Tag { get; set; }
        /// <summary>
        /// Gets or sets the constraint type
        /// </summary>
        public MetaConstraintType ConstraintType { get; set; }
        /// <summary>
        /// Gets or sets the field names or a primary key, unique key or check constraint
        /// of fields that belong to the owner table (a semi-colon delimited list of names).
        /// </summary>
        public string Fields
        {
            get { return !string.IsNullOrEmpty(fields) ? fields : string.Empty; }
            set { fields = value; }
        }
        /// <summary>
        /// Gets or sets the foreign table name. Valid when this is a foreign key
        /// </summary>
        public string ForeignTable
        {
            get { return !string.IsNullOrEmpty(foreignTable) ? foreignTable : string.Empty; }
            set { foreignTable = value; }
        }
        /// <summary>
        /// Gets or sets the field names or a a foreign key constraint
        /// of fields that belong to the foreign table table (a semi-colon delimited list of names).
        /// </summary>
        public string ForeignFields
        {
            get { return !string.IsNullOrEmpty(foreignFields) ? foreignFields : string.Empty; }
            set { foreignFields = value; }
        }
        /// <summary>
        /// Gets or sets the expression of a check constraint
        /// </summary>
        public string Expression
        {
            get { return !string.IsNullOrEmpty(expression) ? expression : string.Empty; }
            set { expression = value; }
        }

        /// <summary>
        /// Gets or sets the delete rule of foreign key
        /// </summary>
        public string DeleteRule
        {
            get { return !string.IsNullOrEmpty(deleteRule) ? deleteRule : string.Empty; }
            set { deleteRule = value; }
        }
        /// <summary>
        /// Gets or sets the update rule of foreign key
        /// </summary>
        public string UpdateRule
        {
            get { return !string.IsNullOrEmpty(updateRule) ? updateRule : string.Empty; }
            set { updateRule = value; }
        }
    }
}
