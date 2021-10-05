using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

using Tripous;


namespace WebLib
{

    /// <summary>
    /// Type to be used as the generator of the data-setup attribute value for a default DataView razor view.
    /// </summary>
    public class DataViewSetup
    {
        /* construction */
        /// <summary>
        /// Constructor.
        /// <para>Creates an instance with default values in ClassType = 'tp.DataView' and BrokerClass = 'tp.Broker'.</para>
        /// </summary>
        public DataViewSetup()
        {
        }

        /* public */
        /// <summary>
        /// Serializes this instance in order to properly used as a data-setup html attribute.
        /// </summary>
        public string Serialize()
        {
            string JsonText = Json.Serialize(this);
            return JsonText;
        }

        /* properties */
        /// <summary>
        /// The javascript class of the view object.
        /// </summary>
        public string ClassType { get; set; } = "app.DeskDataView";
        /// <summary>
        /// The javascript class of the broker object.
        /// </summary>
        public string BrokerClass { get; set; } = "tp.Broker";
        /// <summary>
        /// The registered name of the broker.
        /// </summary>
        public string BrokerName { get; set; }

        /// <summary>
        /// When true, controls are auto-created.
        /// <para>Defaults to false.</para>
        /// </summary>
        public bool AutocreateControls { get; set; } = false;

        /// <summary>
        /// A list of javascript files this view needs in order to function properly.
        /// </summary>
        public List<string> JS { get; set; } = new List<string>();
        /// <summary>
        /// A list of csss files this view needs in order to function properly.
        /// </summary>
        public List<string> CSS { get; set; } = new List<string>();
    }
}
