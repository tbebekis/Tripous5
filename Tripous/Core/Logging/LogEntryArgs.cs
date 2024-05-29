using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tripous.Logging
{
    public class LogEntryArgs : EventArgs
    {
        public LogEntryArgs(LogEntry Entry)
        {
            this.Entry = Entry;
        }

        public LogEntry Entry { get; }
    }
}
