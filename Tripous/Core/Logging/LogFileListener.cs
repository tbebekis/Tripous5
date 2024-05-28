using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Tripous.Logging
{
    /// <summary>
    /// A log listener that writes log info to file(s)
    /// </summary>
    public class LogFileListener : ILogListener
    {
        int Counter = 0;
        string FilePath;
        Dictionary<string, int> Lengths = new Dictionary<string, int>();

        /// <summary>
        /// Writes a line of text to the current file.
        /// If the file reaches the size limit, creates a new file and uses that new file.
        /// </summary>
        void WriteLine(string Text)
        {
            if (!string.IsNullOrWhiteSpace(Text))
            {
                // check the file size after any 100 writes
                Counter++;
                if (Counter % 100 == 0)
                {
                    FileInfo FI = new FileInfo(FilePath);
                    if (FI.Length > (1024 * 1024 * MaxSizeInMB))
                    {
                        BeginFile();
                    }
                }

                Text = Text.Trim();
                Text += Environment.NewLine;
                File.AppendAllText(FilePath, Text);
            }

        }
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
        /// <summary>
        /// Creates a new disk file and writes the column titles
        /// </summary>
        void BeginFile()
        {
            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);

            FilePath = Path.Combine(Folder, "LOG_" + DateTime.UtcNow.ToFileName(true) + ".log");

            StringBuilder SB = new StringBuilder();
            SB.Append(Pad("TimeStamp UTC", Lengths["TimeStamp"]));
            SB.Append(Pad("Host", Lengths["Host"]));
            SB.Append(Pad("User", Lengths["User"]));
            SB.Append(Pad("Level", Lengths["Level"]));
            SB.Append(Pad("EventId", Lengths["EventId"]));
            SB.Append(Pad("Source", Lengths["Source"]));
            SB.Append(Pad("Scope", Lengths["Scope"]));
            SB.Append("Text");
            SB.AppendLine();

            File.WriteAllText(FilePath, SB.ToString());
        }

        /* construction */
        /// <summary>
        /// Constructor
        /// </summary>
        public LogFileListener(string Folder = "", int MaxSizeInMB = 5)
        {
            // prepare the lengths table
            Lengths["TimeStamp"] = 24;
            Lengths["Host"] = 24;
            Lengths["User"] = 24;
            Lengths["Level"] = 12;
            Lengths["EventId"] = 14;
            Lengths["Source"] = 64;
            Lengths["Scope"] = 32;

            this.Folder = !string.IsNullOrWhiteSpace(Folder) ? Folder : Logger.LogFolder;
            this.MaxSizeInMB = MaxSizeInMB < 1 ? 5 : MaxSizeInMB;

            // create the first file
            BeginFile();
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

        /* properties */
        /// <summary>
        /// The folder where log files are placed. Defaults to Sys.AppRootDataFolder/Logs
        /// </summary>
        public string Folder { get; private set; }
        /// <summary>
        /// The max size of a log file in MB. When a file reaches that size, a new one is created. Defaults to 5MB.
        /// </summary>
        public int MaxSizeInMB { get; private set; }
    }


}
