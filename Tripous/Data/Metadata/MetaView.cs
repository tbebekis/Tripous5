namespace Tripous.Data.Metadata
{


    /// <summary>
    /// Represents schema information for a view
    /// </summary>
    public class MetaView : NamedItem, IMetaNode, IMetaFullText
    {
        private string sourceCode;
        private string schemaOwner;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public MetaView()
        {
        }

        /* properties */
        /// <summary>
        /// Gets the owner metastore
        /// </summary>
        public Metastore Store { get { return CollectionOwner as Metastore; } }
        /// <summary>
        /// The kind of this meta-node, i.e. Tables, Table, Columns, Column, etc
        /// </summary>
        public MetaNodeKind Kind { get { return MetaNodeKind.View; } }
        /// <summary>
        /// Gets the text this instance provides for display purposes
        /// </summary>
        public string DisplayText { get { return Name; } }
        /// <summary>
        /// A user defined value
        /// </summary>
        public object Tag { get; set; }
        /// <summary>
        /// Gets the full text
        /// </summary>
        public string FullText
        {
            get
            {
                StringBuilder SB = new StringBuilder();

                SB.AppendLine(string.Format("{0}: {1}", Kind.ToString(), Name));
                if (!string.IsNullOrEmpty(SourceCode))
                {
                    SB.AppendLine("-----------------------------------");
                    SB.AppendLine(SourceCode);
                }

                return SB.ToString();

            }
        }
        /// <summary>
        /// Gets or sets the source code
        /// </summary>
        public string SourceCode
        {
            get { return !string.IsNullOrEmpty(sourceCode) ? sourceCode : string.Empty; }
            set { sourceCode = value; }
        }
        /// <summary>
        /// Gets or sets the SchemaOwner
        /// </summary>
        public string SchemaOwner
        {
            get { return !string.IsNullOrEmpty(schemaOwner) ? schemaOwner : string.Empty; }
            set { schemaOwner = value; }
        }
    }
}
