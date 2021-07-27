/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;


namespace Tripous
{
    /// <summary>
    /// Represents an item in an exchangable list
    /// </summary>
    public interface IInExListerItem
    {
        /* properties */
        /// <summary>
        /// Gets the Id
        /// </summary>
        object Id { get; }
        /// <summary>
        /// Gets the Title
        /// </summary>
        string Title { get; }
        /// <summary>
        /// Gets the Tag
        /// </summary>
        object Tag { get; }
    }
}
