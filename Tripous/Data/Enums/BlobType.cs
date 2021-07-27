/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;


namespace Tripous.Data
{
    /// <summary>
    /// Indicates the type of a blob mode of a pop up form
    /// </summary>
    [Flags]
    public enum BlobType
    {
        /// <summary>
        /// None
        /// </summary>
        None = 0,
        /// <summary>
        /// Blob is of type text
        /// </summary>
        Text = 1,
        /// <summary>
        /// Blob is of type image
        /// </summary>
        Image = 2,
        /// <summary>
        /// Blob is of type binary
        /// </summary>
        Binary = 4,
    }
}
