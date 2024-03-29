﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tripous;
using Tripous.Data;

namespace WebLib.Models
{
 
    /// <summary>
    /// A model for a view.
    /// </summary>
    public class ViewModel
    {
         /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public ViewModel(ViewDef Def)
        {
            this.Def = Def; 
        }
 
        /* properties */
        /// <summary>
        /// The definition of the view
        /// </summary>
        public ViewDef Def { get; } 
    }
    
}
