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

        /// <summary>
        /// Constructor
        /// </summary>
        public ViewColumnDef()
        {

        }

        /// <summary>
        /// The data source name. When empty then it binds to its parent's source.
        /// </summary>
        public string SourceName { get; set; }

        /// <summary>
        /// Width percent of text in rows.
        /// </summary>
        public int TextSplit { get; set; } = 35;

        /// <summary>
        /// A list of control rows.  
        /// </summary>
        public List<ViewControlDef> Controls { get; } = new List<ViewControlDef>();
    }


 
}
