/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;


namespace Tripous.Data.Metadata
{

    /// <summary>
    /// Represents a node in a metadata (schema) tree that has loadable children.
    /// <para>That is a Tables, Columns, Indexes etc node.</para>
    /// </summary>
    public interface IMetaNodeList : IMetaNode
    {
        /* methods */
        /// <summary>
        /// Loads the metadata information if it is not already loaded
        /// </summary>
        void Load();
        /// <summary>
        /// Forces the loading of metadata information even if it is already loaded
        /// </summary>
        void ReLoad();

        /* properties */
        /// <summary>
        /// True if the metadata is alreade loaded
        /// </summary>
        bool IsLoaded { get; }
        /// <summary>
        /// Gets the node list
        /// </summary>
        IMetaNode[] Nodes { get; }
    }
}
