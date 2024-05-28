using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Tripous.Logging
{


    /// <summary>
    /// A log listener that calls a function for each log entry passing the log text.
    /// </summary>
    public class LogLineListener : ILogListener
    {
        Dictionary<string, int> Lengths = new Dictionary<string, int>();
        Action<string> AppendLineProc;

        /// <summary>
        /// Pads a string with spaces to a max length. Truncates the string to max length if the string exceeds the limit.
        /// </summary>
        string Pad(string Text, int MaxLength)
        {
            if (string.IsNullOrWhiteSpace(Text))
                return "".PadRight(MaxLength);

            if (Text.Length > MaxLength)
                return Text.Substring(0, MaxLength);

            return Text.PadRight(MaxLength);
        }
        void WriteLine(string Text)
        {
            AppendLineProc(Text);
        }

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public LogLineListener(Action<string> AppendLineProc)
        {
            if (AppendLineProc == null)
                Sys.Throw("No AppendLine Proc provided to LogLineListener");

            this.AppendLineProc = AppendLineProc;

            // prepare the lengths table
            Lengths["TimeStamp"] = 24;
            Lengths["Host"] = 24;
            Lengths["User"] = 24;
            Lengths["Level"] = 12;
            Lengths["EventId"] = 14;
            Lengths["Source"] = 64;
            Lengths["Scope"] = 32;
        }


        /* public */
        /// <summary>
        /// Called by the Logger to pass LogInfo to a log listener.
        ///<para>
        /// CAUTION: The Logger calls its Listeners asynchronously, that is from inside a thread.
        /// Thus Listeners should synchronize the ProcessLogInfo() call. Controls need to check if InvokeRequired.
        /// </para>
        /// </summary>
        public void ProcessLog(LogEntry Info)
        {
            StringBuilder SB = new StringBuilder();
            SB.Append(Pad(Info.TimeStamp.ToString("yyyy-MM-dd HH:mm:ss.ff"), Lengths["TimeStamp"]));
            SB.Append(Pad(Info.Host, Lengths["Host"]));
            SB.Append(Pad(Info.User, Lengths["User"]));
            SB.Append(Pad(Info.Level.ToString(), Lengths["Level"]));
            SB.Append(Pad(Info.EventId.ToString(), Lengths["EventId"]));
            SB.Append(Pad(Info.Source, Lengths["Source"]));
            SB.Append(Pad(Info.ScopeId, Lengths["Scope"]));

            string Text = Info.Text;
            if (Info.Properties != null && Info.Properties.Count > 0)
            {
                Text = Text + " Properties = " + Json.Serialize(Info.Properties);
            }

            if (!string.IsNullOrWhiteSpace(Text))
            {
                SB.Append(Text.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " "));
            }

            if (!string.IsNullOrWhiteSpace(Info.ExceptionData))
            {
                SB.Append(Info.ExceptionData.Replace("\r\n", " ").Replace("\r", " ").Replace("\n", " "));
            }

            SB.AppendLine();

            WriteLine(SB.ToString());
        }
    }
}
