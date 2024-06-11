using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous
{
    public class BroadcasterArgs: EventArgs
    {
        internal BroadcasterArgs(string EventName, object Sender, IDictionary<string, object> Params)
        {
            this.EventName = EventName;
            this.Sender = Sender;
            if (Params == null)
                Params = new Dictionary<string, object>();
            this.Params = Params;
        }
        internal BroadcasterArgs(string EventName, object Sender) : this(EventName, Sender, null)
        {
        }
        internal BroadcasterArgs(string EventName): this(EventName, null, null)
        {
        }

        public string EventName { get; }
        public object Sender { get; }
        public IDictionary<string, object> Params { get; }

    }
}
