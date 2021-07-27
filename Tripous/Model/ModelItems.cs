/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;


namespace Tripous.Model
{


    /// <summary>
    /// Base collection for the business model sub-system
    /// </summary>
    public class ModelItems<T> : NamedItems<T> where T : class, INamedItem, new()
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ModelItems()
        {
        }
    }
}
