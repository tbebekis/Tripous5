/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;

namespace Tripous
{
    /// <summary>
    /// Represents an object with a unique Name  (Code property)
    /// </summary>
    public interface ICodeName
    {
        /// <summary>
        /// The unique "code name" of the method or constructor. 
        /// </summary>
        string Code { get; }
    }

}
