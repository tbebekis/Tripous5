using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Tripous
{
    /// <summary>
    /// An AJAX request
    /// </summary>
    public class AjaxRequest
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public AjaxRequest()
        {
        }

        /* public */
        /// <summary>
        /// Returns true when Params contains a specified key.
        /// </summary>
        public bool ParamsContainsKey(string Key)
        {
            return !string.IsNullOrWhiteSpace(Key) && Params != null && Params.ContainsKey(Key) && Params[Key] != null;
        }

        /* properties */
        /// <summary>
        /// Optional. The id of the request.
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Required. The name of the operation to execute
        /// </summary>
        public string OperationName { get; set; }
        /// <summary>
        /// Optional. Any parameteres coming along with the request.
        /// </summary>
        public Dictionary<string, object> Params { get; set; } = new Dictionary<string, object>();

        /// <summary>
        /// Returns true when this is a Ui request.
        /// </summary>
        [JsonIgnore]
        public bool IsUiRequest { get => ParamsContainsKey("Type") && Sys.IsSameText(Params["Type"].ToString(), RequestTypeUi); }
        /// <summary>
        /// Returns true when this is a Proc request.
        /// </summary>
        [JsonIgnore]
        public bool IsProcRequest { get => ParamsContainsKey("Type") && Sys.IsSameText(Params["Type"].ToString(), RequestTypeProc); }
        /// <summary>
        /// Returns true when this is a Proc request.
        /// </summary>
        [JsonIgnore]
        public bool IsSingleInstance { get => ParamsContainsKey("IsSingleInstance") && Convert.ToBoolean(Params["IsSingleInstance"]); }

        /// <summary>
        /// The string literal that classifies a request as a Ui request.
        /// </summary>
        static public string RequestTypeUi { get; set; } = "Ui";
        /// <summary>
        /// The string literal that classifies a request as a Proc request.
        /// </summary>
        static public string RequestTypeProc { get; set; } = "Proc";

    }


}
