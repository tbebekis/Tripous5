namespace Tripous.Data.Metadata
{

    /// <summary>
    /// Indicates the kind of a node in a metadata (schema) tree
    /// </summary>
    public enum MetaNodeKind
    {
        /// <summary>
        /// None
        /// </summary>
        None,
        /// <summary>
        /// Databases
        /// </summary>
        Databases,
        /// <summary>
        /// Database
        /// </summary>
        Database,
        /// <summary>
        /// Tables
        /// </summary>
        Tables,
        /// <summary>
        /// Table
        /// </summary>
        Table,
        /// <summary>
        /// Fields
        /// </summary>
        Fields,
        /// <summary>
        /// Field
        /// </summary>
        Field,
        /// <summary>
        /// Indexes
        /// </summary>
        Indexes,
        /// <summary>
        /// Index
        /// </summary>
        Index, 
        /// <summary>
        /// Constraints
        /// </summary>
        Constraints,
        /// <summary>
        /// Constraint
        /// </summary>
        Constraint,
        /// <summary>
        /// Triggers
        /// </summary>
        Triggers,
        /// <summary>
        /// Trigger
        /// </summary>
        Trigger,
        /// <summary>
        /// Views
        /// </summary>
        Views,
        /// <summary>
        /// View
        /// </summary>
        View,
        /// <summary>
        /// Procedures
        /// </summary>
        Procedures,
        /// <summary>
        /// Procedure
        /// </summary>
        Procedure,
        /// <summary>
        /// Types
        /// </summary>
        Types,
        /// <summary>
        /// Type
        /// </summary>
        Type,
    }
}
