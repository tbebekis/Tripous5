using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Tripous.Data
{
    public class SqlLogEntry
    {
        static int fCounter = 0;
        const string cStartLine     = "/*==============================================================";
        const string cEndLine       = "--------------------------------------------------------------*/";
       

        public SqlLogEntry(DateTime StartTimeUtc, string SqlText, string Source, string Scope, string EventId, string ParamsText)
        {
            fCounter++;
            this.Counter = fCounter;

            this.StartTimeUtc = StartTimeUtc;
            this.EndTimeUtc = DateTime.UtcNow;
            ElapsedTime = EndTimeUtc - StartTimeUtc;

            this.Source = Source;
            this.Scope = Scope;
            this.EventId = EventId;

            this.SqlText = SqlText;
            this.ParamsText = ParamsText;
        }
        public SqlLogEntry(DateTime StartTimeUtc, string SqlText, string Source, string Scope)
            : this(StartTimeUtc, SqlText, Source, Scope, string.Empty, string.Empty)
        {           
        }
        public SqlLogEntry(DateTime StartTimeUtc, string SqlText, string Source)
            : this(StartTimeUtc, SqlText, Source, string.Empty, string.Empty, string.Empty)
        { 
        }

        public override string ToString()
        {
            StringBuilder SB = new StringBuilder();

            SB.AppendLine($"[{Counter}]");

            SB.AppendLine(cStartLine);
            SB.AppendLine($"Start Time Utc: {StartTimeUtc.ToString("HH:mm:ss")}");
            SB.AppendLine($"Elapsed Time: {ElapsedTime.ToString(@"dd\.hh\:mm\:ss")}");
            SB.AppendLine($"Source: {Source}");
            SB.AppendLine($"Scope: {Scope}");
            SB.AppendLine($"EventId: {EventId}");
            SB.AppendLine($"SqlText");
            SB.AppendLine(SqlText);
            if (!string.IsNullOrWhiteSpace(ParamsText))
            {
                SB.AppendLine("Parameters");
                SB.AppendLine(ParamsText);
            }
                
            SB.AppendLine(cEndLine);
            

            return SB.ToString();
        }

        public int Counter { get; }
        public DateTime StartTimeUtc { get; }
        public DateTime EndTimeUtc { get; }
        public TimeSpan ElapsedTime { get; }
        public string Source { get; }
        public string Scope { get; }
        public string EventId { get;  }
        public string SqlText { get;  }
        public string ParamsText { get; }
    }
}
