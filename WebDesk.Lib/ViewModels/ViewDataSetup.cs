using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

using Tripous;
using Tripous.Data;

namespace WebLib.Models
{

    /// <summary>
    /// Type to be used as the generator of the data-setup attribute value for a default DataView razor view.
    /// </summary>
    public class ViewDataSetup
    {
        
        ViewDef ViewDef;

        /* construction */
        /// <summary>
        /// Constructor.
        /// <para>Creates an instance with default values in ClassType = 'tp.DataView' and BrokerClass = 'tp.Broker'.</para>
        /// </summary>
        public ViewDataSetup()
        {
        }
        /// <summary>
        /// Constructor.
        /// <para>Creates an instance with default values from a specified <see cref="ViewDef"/> .</para>
        /// </summary>
        public ViewDataSetup(ViewDef ViewDef)
        {
            this.ViewDef = ViewDef; 
        }

        List<string> MergeLists(List<string> A, List<string> B)
        {
            List<string> Result = new List<string>();

            if (A != null && A.Count > 0)
                Result.AddRange(A);

            if (B != null && B.Count > 0)
            {
                foreach (var S in B)
                {
                    if (!Result.Contains(S))
                        Result.Add(S);
                }
            }

            return Result;
        }

        /* public */
        /// <summary>
        /// Serializes this instance in order to properly used as a data-setup html attribute.
        /// </summary>
        public string Serialize()
        {
            Dictionary<string, object> Result = new Dictionary<string, object>();

            if (ViewDef != null)
                ViewDef.AssignTo(Result);

            if (!string.IsNullOrWhiteSpace(ClassType))
                Result["ClassType"] = ClassType;

            if (!string.IsNullOrWhiteSpace(BrokerClass))
                Result["BrokerClass"] = BrokerClass;

            if (!string.IsNullOrWhiteSpace(BrokerName))
                Result["BrokerName"] = BrokerName;

            if (AutocreateControls)
                Result["AutocreateControls"] = AutocreateControls;

            List<string> TempList;

            // JS
            List<string> ViewJS = Result.ContainsKey("JS")? Result["JS"] as List<string>: null;
            TempList = MergeLists(ViewJS, JS);
            if (TempList.Count > 0)
                Result["JS"] = TempList;
            else
                Result.Remove("JS");

            // CSS
            List<string> ViewCSS = Result.ContainsKey("CSS") ? Result["CSS"] as List<string> : null;
            TempList = MergeLists(ViewCSS, JS);
            if (TempList.Count > 0)
                Result["CSS"] = TempList;
            else
                Result.Remove("CSS"); 

            // CssClasses
            List<string> ViewCssClasses = Result.ContainsKey("CssClasses") ? Result["CssClasses"] as List<string> : null;
            TempList = MergeLists(ViewCssClasses, CssClasses);
            if (TempList.Count > 0)
                Result["CssClasses"] = string.Join(" ", TempList.ToArray());

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
        /// A list of css classes for the view
        /// </summary>
        public List<string> CssClasses { get; set; } = new List<string>() { "tp-DeskView" };

        /// <summary>
        /// Custom properties helper.
        /// <para>Custom properties for more properties of the Control part of the data-setup attribute.</para>
        /// </summary>
        public object this[string Key]
        {
            get { return Properties.ContainsKey(Key) ? Properties[Key] : null; }
            set { Properties[Key] = value; }
        }
        /// <summary>
        /// Dictionary with custom properties. 
        /// <para>Custom properties for more properties of the Control part of the data-setup attribute.</para>
        /// </summary>
        public Dictionary<string, object> Properties { get; } = new Dictionary<string, object>();
    }


    
}
