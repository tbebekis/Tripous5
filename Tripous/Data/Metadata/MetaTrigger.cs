namespace Tripous.Data.Metadata
{

    /// <summary>
    /// Represents schema information for a trigger in a table
    /// </summary>
    public class MetaTrigger : NamedItem, IMetaNode, IMetaFullText
    {
        private string triggerType;
        private string triggerEvent;
        private string sourceCode;

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public MetaTrigger()
        {
        }


        /* properties */
        /// <summary>
        /// Get the table this field belongs to
        /// </summary>
        public MetaTable Table { get { return CollectionOwner as MetaTable; } }
        /// <summary>
        /// Gets the display text for item
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
                if (!string.IsNullOrEmpty(TriggerEvent))
                    SB.AppendLine(string.Format("{0}: {1}", TriggerType, TriggerEvent));

                if (!string.IsNullOrEmpty(SourceCode))
                {
                    SB.AppendLine("-----------------------------------");
                    SB.AppendLine(SourceCode);
                }

                return SB.ToString();
            }
        }
        /// <summary>
        /// The kind of this meta-node, i.e. Tables, Table, Columns, Column, etc
        /// </summary>
        public MetaNodeKind Kind { get { return MetaNodeKind.Trigger; } }
        /// <summary>
        /// A user defined value
        /// </summary>
        public object Tag { get; set; }
        /// <summary>
        /// Gets or sets the type (before, after)
        /// </summary>
        public string TriggerType
        {
            get { return !string.IsNullOrEmpty(triggerType) ? triggerType : string.Empty; }
            set { triggerType = value; }
        }
        /// <summary>
        /// Gets or sets the event (delete, update)
        /// </summary>
        public string TriggerEvent
        {
            get { return !string.IsNullOrEmpty(triggerEvent) ? triggerEvent : string.Empty; }
            set { triggerEvent = value; }
        }
        /// <summary>
        /// Gets or sets the source code
        /// </summary>
        public string SourceCode
        {
            get { return !string.IsNullOrEmpty(sourceCode) ? sourceCode : string.Empty; }
            set { sourceCode = value; }
        }
    }
}
