/*--------------------------------------------------------------------------------------        
                            Copyright © 2013 Theodoros Bebekis
                               teo.bebekis@gmail.com 
--------------------------------------------------------------------------------------*/

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

namespace Tripous.Data
{
    /// <summary>
    /// Base collection for the data sub-system
    /// </summary>
    public class DbItems<T> : NamedItems<T> where T : class, INamedItem, new()
    {
        /// <summary>
        /// For thread synchronization
        /// </summary>
        protected override void Lock()
        {
            //Db.Lock();
        }
        /// <summary>
        /// For thread synchronization
        /// </summary>
        protected override void UnLock()
        {
            //Db.UnLock();
        }


        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public DbItems()
        {
        }
    }
}
