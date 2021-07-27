/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;


namespace Tripous.Data.Metadata
{

    /// <summary>
    /// An IMetaNode that is IMetaNodeList parent, i.e. A MetaTable which has Fields, Indexes etc lists.
    /// </summary>
    public interface IMetaNodeListParent : IMetaNode
    {
        /// <summary>
        /// Gets the lists
        /// </summary>
        IMetaNodeList[] Lists { get; }

    }

}
