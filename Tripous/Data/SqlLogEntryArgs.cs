using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tripous.Logging;

namespace Tripous.Data
{
 
    public class SqlLogEntryArgs : EventArgs
    {
        public SqlLogEntryArgs(SqlLogEntry Entry)
        {
            this.Entry = Entry;
        }

        public SqlLogEntry Entry { get; }
    }
}
