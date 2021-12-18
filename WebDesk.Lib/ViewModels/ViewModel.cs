using System;
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
        public ViewModel(ViewDef ViewDef)
        {
            this.ViewDef = ViewDef;
            this.DataSetup.BrokerName = ViewDef.BrokerName;
        }


        /// <summary>
        /// Returns the <see cref="DataSetup"/> of this instance serialized in order to properly used as a data-setup html attribute.
        /// </summary>
        public string GetDataSetupText()
        {
            return DataSetup.Serialize();
        }

        /* properties */
        /// <summary>
        /// The definition of the view
        /// </summary>
        public ViewDef ViewDef { get; }
        /// <summary>
        /// Used as the generator of the data-setup attribute value for a default DataView razor view.
        /// </summary>
        public ViewDataSetup DataSetup { get; } = new ViewDataSetup();
    }

}
