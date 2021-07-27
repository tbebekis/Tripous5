/*--------------------------------------------------------------------------------------        
                           Copyright © 2018 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Collections;
using System.ComponentModel;


namespace Tripous
{

    /// <summary>
    /// A collection of Descriptor elements
    /// </summary>
    public class Descriptors<T> : NamedItems<T> where T : Descriptor, new()
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public Descriptors()
        {
        }
    }

}
