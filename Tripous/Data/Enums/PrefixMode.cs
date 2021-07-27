/*--------------------------------------------------------------------------------------        
                           Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/
using System;
using System.Data;
using System.Data.Common;

namespace Tripous.Data
{
    /// <summary>
    /// Indicates the kind of the parameter name and prefix the native ADO.NET Data Provider uses
    /// </summary>
    public enum PrefixMode
    {
        /// <summary>
        /// The ADO.NET Data Provider expects a parameter name with a prefix, ie. @CUSTOMER_NAME or :CUSTOMER_NAME
        /// </summary>
        Prefixed,
        /// <summary>
        /// The ADO.NET Data Provider expects no prefix or name at all. It uses the question mark as a positional placeholder
        /// </summary>
        Positional,
    }
}
