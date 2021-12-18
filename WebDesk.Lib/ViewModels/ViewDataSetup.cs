using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

using Tripous;


namespace WebLib.Models
{

    /// <summary>
    /// Type to be used as the generator of the data-setup attribute value for a default DataView razor view.
    /// </summary>
    public class ViewDataSetup
    {
        Dictionary<string, object> Properties = new Dictionary<string, object>();

        /* construction */
        /// <summary>
        /// Constructor.
        /// <para>Creates an instance with default values in ClassType = 'tp.DataView' and BrokerClass = 'tp.Broker'.</para>
        /// </summary>
        public ViewDataSetup()
        {
        }

        /* public */
        /// <summary>
        /// Serializes this instance in order to properly used as a data-setup html attribute.
        /// </summary>
        public string Serialize()
        {

            Dictionary<string, object> Result = new Dictionary<string, object>();

            if (!string.IsNullOrWhiteSpace(ClassType))
                Result["ClassType"] = ClassType;

            if (!string.IsNullOrWhiteSpace(BrokerClass))
                Result["BrokerClass"] = BrokerClass;

            if (!string.IsNullOrWhiteSpace(BrokerName))
                Result["BrokerName"] = BrokerName;

            Result["AutocreateControls"] = AutocreateControls;

            if (JS != null && JS.Count > 0)
                Result["JS"] = JS;

            if (CSS != null && CSS.Count > 0)
                Result["CSS"] = JS;


            string JsonText = Json.Serialize(Result);
            return JsonText;
        }
        /// <summary>
        /// Returns true if a specified key exists in custom properties 
        /// </summary>
        public bool ContainsKey(string Key)
        {
            return Properties.ContainsKey(Key);
        }

        /* properties */
        /// <summary>
        /// The javascript class of the view object.
        /// </summary>
        public string ClassType { get; set; } = "tp.DeskDataView";
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

        /// <summary>
        /// Custom properties
        /// </summary>
        public object this[string Key]
        {
            get { return Properties.ContainsKey(Key) ? Properties[Key] : null; }
            set { Properties[Key] = value; }
        }


    }
}
