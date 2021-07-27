using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous
{


    /// <summary>
    /// A delegate to be used in situations where a "generic" event handler is needed 
    /// with an indeterminate number of parameters.
    /// </summary>
    public delegate void BroadcasterDelegate(string EventName, IDictionary<string, object> Args);

}
