/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
 

namespace Tripous.Data.Metadata
{

    /// <summary>
    /// Represents a metadata collection or item, i.e MetaTables or MetaTable
    /// </summary>
    public interface IMetaNode
    {

        /// <summary>
        /// Gets the text this instance provides for display purposes
        /// </summary>
        string DisplayText { get; }
        /// <summary>
        /// The kind of this meta-node, i.e. Tables, Table, Columns, Column, etc
        /// </summary>
        MetaNodeKind Kind { get; }
        /// <summary>
        /// A user defined value
        /// </summary>
        object Tag { get; set; }
    }


 
}
