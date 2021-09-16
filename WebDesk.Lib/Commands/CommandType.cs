using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace WebDesk
{
    /// <summary>
    /// The command type. What a command does when it is called.
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum CommandType
    {
        /// <summary>
        /// Displays a Ui. The command's Tag is the name of Ui View
        /// </summary>
        Ui = 0,
        /// <summary>
        /// Executes a procedure. The command's Tag is the name procedure
        /// </summary>
        Proc = 1,
    }
}
