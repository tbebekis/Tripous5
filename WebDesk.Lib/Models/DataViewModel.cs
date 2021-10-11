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
    /// A model for a standard data-view.
    /// <para>A standard data-view has three parts: Brower, Edit and Filters.</para>
    /// <para>The Browser part contains/is the browser grid.</para>
    /// <para>The Filters part contains the filter controls.</para>
    /// <para>The Edit part contains a pager (TabControl) with one or more tab-pages for editing/inserting data.</para>
    /// </summary>
    public class DataViewModel 
    {
        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public DataViewModel(ViewDef ViewDef)
        {
            this.ViewDef = ViewDef;
            this.Setup.BrokerName = ViewDef.BrokerName;
        }

        /* properties */
        /// <summary>
        /// The definition of the view
        /// </summary>
        public ViewDef ViewDef { get; }
        /// <summary>
        /// Used as the generator of the data-setup attribute value for a default DataView razor view.
        /// </summary>
        public DataViewSetup Setup { get; } = new DataViewSetup();

          
    }

 
}
