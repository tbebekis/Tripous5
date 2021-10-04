﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Data
{


    /// <summary>
    /// Represents a column in any ui container.
    /// </summary>
    public class ViewColumnDef  
    {

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewColumnDef()
        {
        }
 
        /* properties */ 
        /// <summary>
        /// The data source name. When empty then it binds to its parent's source.
        /// </summary>
        public string TableName { get; set; }
 
        /// <summary>
        /// A list of control rows.  
        /// </summary>
        public List<ViewControlDef> Controls { get; } = new List<ViewControlDef>();
    }


 
}
