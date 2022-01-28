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
    /// The request type.  Ui or Proc.  
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum RequestType
    {
        /// <summary>
        /// Requests a Ui. 
        /// </summary>
        Ui = 0,
        /// <summary>
        /// Request the execution of a procedure. 
        /// </summary>
        Proc = 1,
    }
}
